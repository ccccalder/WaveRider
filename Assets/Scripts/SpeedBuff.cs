using UnityEngine;

[CreateAssetMenu(menuName = "Powerups/Speed Buff")]

public class SpeedBuff : PowerupEffect
{
    public int amount = 40;
    public int permanentAmount = 5;
    public override void Apply(GameObject target)
    {
        PlayerController controller = target.GetComponent<PlayerController>();
        if (controller == null)
        {
            controller = target.GetComponentInParent<PlayerController>();
        }
        if (controller != null)
        {
            controller.moveSpeed += amount;
            controller.maxSpeedCap += permanentAmount;
            controller.minSpeedCap += permanentAmount;
            controller.speedCap += permanentAmount;
            controller.speedBuffFeedbacks.PlayFeedbacks();
        }
    }
}
