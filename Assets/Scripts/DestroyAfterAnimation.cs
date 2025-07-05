using UnityEngine;

public class DestroyAfterAnimation : MonoBehaviour
{
    void Start()
    {
        // Get the length of the currently playing animation clip
        float animationLength = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;

        // Destroy the object after the animation is done
        Destroy(gameObject, animationLength);
    }
}
