using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Powerups/HealthBuff")]
public class HealthBuff : PowerUpEffect
{
    public int healthAmount;

    public override void Apply(GameObject target)
    {
        IHealable healable = target.GetComponent<IHealable>();
        if (healable != null)
        {
            healable.Heal(healthAmount);
        }
    }
}
