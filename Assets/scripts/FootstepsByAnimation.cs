using UnityEngine;

public class FootstepsByAnimation : MonoBehaviour
{
    public AudioSource audioSource; 
    public Animator anim;
    public string walkAnimationName = "walk";
    public float stepInterval = 0.5f;

    private float timer;

    void Update()
    {
        if (anim == null || audioSource == null) return;

        if (anim.GetCurrentAnimatorStateInfo(0).IsName(walkAnimationName))
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
    }
}
