using UnityEngine;

[CreateAssetMenu(menuName = "Powerups/MachineGunBuff")]
public class MachineGunBuff : PowerUpEffect
{
    public float duration = 5f;

    public override void Apply(GameObject target)
    {
        IMachineGun gunner = target.GetComponent<IMachineGun>();
        if (gunner != null)
        {
            gunner.EnableMachineGun(duration);
        }
    }

}