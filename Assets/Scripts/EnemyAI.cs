using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour, ISpeedBuff
{
    public Transform player;
    public float moveSpeed = 2f;

    private float originalSpeed;
    private bool isSpeedModified = false;

    void Start()
    {
        originalSpeed = moveSpeed;
    }

    void Update()
    {
        if (player == null) return;

        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    public void ModifySpeed(float amount, float duration)
    {
        if (isSpeedModified) return;

        moveSpeed += amount;
        isSpeedModified = true;
        StartCoroutine(ResetSpeed(duration));
    }

    private IEnumerator ResetSpeed(float duration)
    {
        yield return new WaitForSeconds(duration);
        moveSpeed = originalSpeed;
        isSpeedModified = false;
    }
}
