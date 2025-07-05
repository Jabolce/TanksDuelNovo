using UnityEngine;

public class SpeedBoostPad : MonoBehaviour
{
    public float boostAmount = 3f;
    public float duration = 2f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        ISpeedBuff speedBuff = other.GetComponent<ISpeedBuff>();
        if (speedBuff != null)
        {
            speedBuff.ModifySpeed(boostAmount, duration);
        }
    }

    // For 3D, change OnTriggerEnter2D → OnTriggerEnter
}
