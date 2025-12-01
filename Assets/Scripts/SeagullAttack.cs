using MoreMountains.Feedbacks;
using UnityEngine;

public class SeagullAttack : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 10f;
    private Rigidbody2D rb;
    private BoxCollider2D coll;

    private bool isDead = false;

    [Header("Feedbacks")]
    public MMFeedbacks hitPlayerFeedback;

    private Vector3 startPosition;
    private Transform targetPlayer;

    void Start()
    {
        isDead = false;
        startPosition = transform.position;
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;

        if (playerObj != null)
        {
            targetPlayer = playerObj.transform;
        }
    }

    void Update()
    {
        if (targetPlayer == null) return;
        if (isDead) return;
        rb.linearVelocity = Vector2.left * speed;

        if (transform.position.x < targetPlayer.position.x - 25f)
        {
            Destroy(gameObject);
        }
    }
    // Handle collision with the player
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Die();
        }
    }


    private void Die()
    {
        if (isDead)
        {
            return;
        }
        isDead = true;
        speed = 0f;
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 2;
        Vector2 flyDirection = Vector2.up + Vector2.right;
        float flyForce = 500f; ;
        rb.AddForce(flyDirection.normalized * flyForce);
        hitPlayerFeedback.PlayFeedbacks();

    }
}
