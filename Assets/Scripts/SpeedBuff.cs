using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Powerups/SpeedBuff")]
public class SpeedBuff : PowerUpEffect
{
    public float speedAmount = 2f;
    public float duration = 5f;

    public override void Apply(GameObject target)
    {

        ISpeedBuff mod = target.GetComponent<ISpeedBuff>();
        if (mod != null)
        {
            mod.ModifySpeed(speedAmount, duration);
        }
    }
}
