using MoreMountains.Feedbacks;
using UnityEngine;

public class Surfer : MonoBehaviour
{
    public MMF_Player deathFeedback;
    public float timeBeforeRestartAnim = 1f;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wave"))
        {
            if(gameObject.GetComponentInParent<PlayerController>().isInvincible)
            {
                //do nothing if shielded
                return;
            }
            if (deathFeedback != null)
                deathFeedback.PlayFeedbacks();

            //loads end screen level
            FindFirstObjectByType<LevelLoader>().LoadEndResults(timeBeforeRestartAnim);

            //makes movespeed = 0
            if (gameObject.GetComponentInParent<PlayerController>() != null)
                gameObject.GetComponentInParent<PlayerController>().Die();
        }
        if (collision.gameObject.CompareTag("Seagull"))
        {
            if (gameObject.GetComponentInParent<PlayerController>().isInvincible)
            {
                //do nothing if shielded
                return;
            }

            //makes movespeed = 0
            if (gameObject.GetComponentInParent<PlayerController>() != null)
                gameObject.GetComponentInParent<PlayerController>().Die();
        }
    }
}
