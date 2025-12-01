using UnityEngine;

[CreateAssetMenu(menuName = "Powerups/Point Buff")]
public class PointBuff : PowerupEffect
{
    public int amount = 100;
    public float forceAmount = 300f;
    public override void Apply(GameObject target)
    {
        ScoreManager scoreManager = FindAnyObjectByType<ScoreManager>();
        if (scoreManager != null)
        {
            PlayerController controller = target.GetComponent<PlayerController>();
            if (controller == null)
            {
                controller = target.GetComponentInParent<PlayerController>();
            }
            controller.pointBuffFeedbacks.PlayFeedbacks();

            //actually adding points
            int scoreMultiplier = scoreManager.scoreMultiplier;
            scoreManager.AddScore(amount * scoreMultiplier);
            scoreManager.ShowPoints(amount * scoreMultiplier);

            //adding force upwards
            Rigidbody2D playerRb = target.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {

                //stop the fall
                playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, 0f);
                playerRb.gravityScale = 1;
                controller.isSlamming = false;
                controller.FallingFeedback.StopFeedbacks();

                //add the force
                playerRb.AddForce(Vector2.up * forceAmount);
            }
            else if (playerRb == null)
            {
                //additional step of adding rb if we collided with the surfer(child object)
                playerRb = target.GetComponentInParent<Rigidbody2D>();


                //stop the fall
                playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, 0f);
                playerRb.gravityScale = 1;
                controller.isSlamming = false;
                controller.FallingFeedback.StopFeedbacks();

                //add the force
                playerRb.AddForce(Vector2.up * forceAmount);
            }
        }
    }
}
