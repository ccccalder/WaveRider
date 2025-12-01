using UnityEngine;

[CreateAssetMenu(menuName = "Powerups/Shield Buff")]

public class ShieldBuff : PowerupEffect
{
    public float shieldDurationPlusTwo = 13f;
    public override void Apply(GameObject target)
    {
        PlayerController controller = target.GetComponent<PlayerController>();
        if (controller == null)
        {
            controller = target.GetComponentInParent<PlayerController>();
        }

        controller.ShieldBuff(shieldDurationPlusTwo);
    }
}
