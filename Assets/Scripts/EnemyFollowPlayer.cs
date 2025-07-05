using UnityEngine;
using Pathfinding;

public class EnemyFollowPlayer : MonoBehaviour
{
    public Transform target;
    private AIPath aiPath;

    void Start()
    {
        aiPath = GetComponent<AIPath>();
        aiPath.canMove = true;
    }

    void Update()
    {
        if (target != null)
        {
            aiPath.destination = target.position;
        }
    }
}
