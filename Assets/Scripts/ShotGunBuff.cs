using UnityEngine;

[CreateAssetMenu(menuName = "Powerups/ShotgunBuff")]
public class ShotgunBuff : PowerUpEffect
{
    public float duration = 5f;

    public override void Apply(GameObject target)
    {
        IShotGun shotgun = target.GetComponent<IShotGun>();
        if (shotgun != null)
        {
            shotgun.EnableShotgun(duration);
        }
    }
}
 