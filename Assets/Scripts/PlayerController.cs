using System.Collections;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms.Impl;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float speedIncreaseRate = 0.5f; // units per second
    public float speedCap = 23f;
    public float minSpeedCap = 20f;
    public float maxSpeedCap = 30f;

    public Transform groundCheck;
    public Vector2 groundCheckBounds = new Vector2(0.5f, 0.1f);
    public LayerMask groundLayer;

    private Rigidbody2D rb;

    private float horizontalInput;

    private bool CountingDownFromDoublePoints = false;

    [Header("RotationStuff")]
    public float torque = 50f; // How much input affects angular velocity
    public float angularDamping = 2f; // How quickly rotation slows down
    public float maxAngularVelocity = 500f; // Clamp for angular velocity


    [Header("FlipStuff")]
    public int pointsPerFlip = 100;
    private float airRotationAccumulated = 0f;
    private float lastRotation = 0f;
    private bool isAirborne = false;

    [Header("GroundSlamStuff")]
    public float slamGravityMultiplier = 5f; // Makes the player fall very fast
    public float slamVelocityY = -25f;       // Initial downward speed of the slam
    [HideInInspector]public bool isSlamming = false;

    [Header("InvicibilityStuff")]
    public bool isInvincible = false;
    public GameObject shieldIcon;


    [Header("Feedbacks")]
    public MMF_Player SlamFeedback;
    public MMFeedbacks FallingFeedback;
    public MMF_Player TouchWaveFeedback;
    public MMFeedbacks flipFeedbacks;
    public MMFeedbacks speedBuffFeedbacks;
    public MMFeedbacks pointBuffFeedbacks;
    public MMFeedbacks doublePointBuffFeedbacks;
    public MMFeedbacks shieldFeedback;



    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        speedCap = Random.Range(minSpeedCap, maxSpeedCap);
    }
    private void Update()
    {
        // Get horizontal input from keyboard (left/right arrows or A/D)
        horizontalInput = (Keyboard.current.leftArrowKey.isPressed || Keyboard.current.aKey.isPressed) ? -1f :
                          (Keyboard.current.rightArrowKey.isPressed || Keyboard.current.dKey.isPressed) ? 1f : 0f;

        bool grounded = IsGrounded();

        if (!grounded)
        {
            // Player is in air
            if (!isAirborne)
            {
                // Start counting from when they first leave ground
                isAirborne = true;
                airRotationAccumulated = 0f;
                lastRotation = transform.eulerAngles.z;
            }

            // Calculate how much rotation happened since last frame
            float currentRotation = transform.eulerAngles.z;
            float delta = Mathf.DeltaAngle(lastRotation, currentRotation);
            airRotationAccumulated += Mathf.Abs(delta);
            lastRotation = currentRotation;
        }
        else
        {
            // Player lands
            if (isAirborne)
            {
                isAirborne = false;

                int flipsCompleted = Mathf.FloorToInt(airRotationAccumulated / 300f );

                if (flipsCompleted > 0)
                {
                    flipFeedbacks.PlayFeedbacks();
                    int earned = flipsCompleted * pointsPerFlip;
                    ScoreManager scoreManager = FindAnyObjectByType<ScoreManager>();
                    scoreManager.AddScore(earned);
                    scoreManager.ShowPoints(earned);
                }
            }
        }

        //speed stuff
        SpeedStuff();

        if (Keyboard.current.spaceKey.wasPressedThisFrame && !isSlamming && isAirborne)
        {
            StartSlam();
        }
    }
    void StartSlam()
    {
        isSlamming = true;
        FallingFeedback.PlayFeedbacks();

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, slamVelocityY);
        rb.gravityScale *= slamGravityMultiplier; 
    }
    private void SpeedStuff()
    {
        if (Mathf.RoundToInt(moveSpeed) < Mathf.RoundToInt(speedCap))
        {
            moveSpeed += speedIncreaseRate * Time.deltaTime;
        }
        else if (Mathf.RoundToInt(moveSpeed) > Mathf.RoundToInt(speedCap))
        {
            moveSpeed -= speedIncreaseRate * Time.deltaTime;
        }
        else
        {
            speedCap = Random.Range(minSpeedCap, maxSpeedCap);
        }
    }
    private bool IsGrounded()
    {
        return Physics2D.OverlapBox(groundCheck.position, groundCheckBounds, 0, groundLayer);

    }
    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveSpeed, rb.linearVelocity.y);

        // Apply torque based on input for rotational inertia
        rb.AddTorque(-horizontalInput * torque, ForceMode2D.Force);

        // Apply angular damping (friction)
        if (horizontalInput == 0)
        {
            rb.angularVelocity = Mathf.MoveTowards(rb.angularVelocity, 0, angularDamping);
        }

        // Clamp angular velocity manually
        rb.angularVelocity = Mathf.Clamp(rb.angularVelocity, -maxAngularVelocity, maxAngularVelocity);

    }

    public void Die()
    {
        FindAnyObjectByType<ScoreManager>().SaveFinalScore();
        moveSpeed = 0f;
        DisconnectAndFlyChild("surfer");
    }
    private void DisconnectAndFlyChild(string childName)
    {
        Transform child = transform.Find(childName);
        if (child != null)
        {
            // Unparent the child
            child.parent = null;

            // Add Rigidbody2D if missing
            Rigidbody2D childRb = child.GetComponent<Rigidbody2D>();
            if (childRb == null)
                childRb = child.gameObject.AddComponent<Rigidbody2D>();

            childRb.mass = 1f;

            Vector2 flyDirection = Vector2.up + Vector2.right;
            float flyForce = 800f;
            childRb.AddForce(flyDirection.normalized * flyForce);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wave"))
        {
            TouchWaveFeedback.PlayFeedbacks();
        }

        if (isSlamming && collision.gameObject.CompareTag("Wave"))
        {
            rb.gravityScale /= slamGravityMultiplier;

            isSlamming = false;
            FallingFeedback.StopFeedbacks();
            SlamFeedback.PlayFeedbacks();
        }

        if (collision.gameObject.CompareTag("Seagull"))
        {
            if (isInvincible)
            {
                return;
            }
            //loads end screen level
            FindFirstObjectByType<LevelLoader>().LoadEndResults(2.5f);
            //playfeedbacks
            Die();
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(groundCheck.position, groundCheckBounds);
    }
    public void DoublePoints(float amountOfTime)
    {
        if (CountingDownFromDoublePoints)
        {
            //resetting the coroutine if we already have double points
            StopCoroutine(DoublePointsEnum(amountOfTime));
        }

        StartCoroutine(DoublePointsEnum(amountOfTime));
        CountingDownFromDoublePoints = true;
    }
    private IEnumerator DoublePointsEnum(float amountOfTime)
    {
        ScoreManager scoreManager = FindAnyObjectByType<ScoreManager>();
        if (scoreManager != null)
        {
            scoreManager.scoreMultiplier *= 2;

            pointsPerFlip *= 2;
            doublePointBuffFeedbacks.PlayFeedbacks();
        }

        Timer countdown = gameObject.GetComponent<Timer>();

        countdown.StartCountdown(amountOfTime);
        yield return new WaitForSeconds(amountOfTime);
        
        scoreManager.scoreMultiplier /= 2;
        pointsPerFlip /= 2;
    }

    public void ShieldBuff(float amountOfTime)
    {
        if (isInvincible)
        {
            //resetting the coroutine if we already have shield
            StopCoroutine(ShieldBuffEnum(amountOfTime));
        }
        StartCoroutine(ShieldBuffEnum(amountOfTime));
        isInvincible = true;
    }
    private IEnumerator ShieldBuffEnum(float amountOfTime)
    {
        shieldFeedback.PlayFeedbacks();
        SpriteRenderer shieldIconRenderer = shieldIcon.GetComponent<SpriteRenderer>();
        shieldIconRenderer.enabled = true;
        yield return new WaitForSeconds(amountOfTime);

        for(int i = 0; i < 15; i++)
        {
            shieldIconRenderer.enabled = false;
            yield return new WaitForSeconds(0.15f);
            shieldIconRenderer.enabled = true;
            yield return new WaitForSeconds(0.15f);
        }

        shieldIconRenderer.enabled = false;
        isInvincible = false;
    }

}
