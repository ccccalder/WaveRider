using UnityEngine;

[CreateAssetMenu(menuName = "Powerups/doublePointsBuff")]

public class doublePointBuff : PowerupEffect
{
    public float timeOfBuff = 10f;
    public override void Apply(GameObject target)
    {
        PlayerController playerController = target.GetComponent<PlayerController>();
        if (playerController == null)
        {
            playerController = target.GetComponentInParent<PlayerController>();
        }
        if (playerController != null)
        {
            playerController.DoublePoints(timeOfBuff);
            
            playerController.doublePointBuffFeedbacks.PlayFeedbacks();
        }
    }
}
