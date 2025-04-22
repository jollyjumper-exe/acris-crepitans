using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    public Animator animator;
    public float activationDistance = 3f;
    public float particleTriggerDistance = 1f;
    public float yAboveOffset = 1f;
    public ParticleSystem electricityParticles;

    private Transform player;
    private bool isAwake = false;
    private bool hasPlayedParticles = false;

    void Start()
    {
        GameObject foundPlayer = GameObject.FindGameObjectWithTag("PlayerAvatar");
        if (foundPlayer)
        {
            player = foundPlayer.transform;
        }
    }

    void Update()
    {
        if (!player) return;

        float distance = Vector3.Distance(player.position, transform.position);
        bool playerAbove = player.position.y > transform.position.y + yAboveOffset;

        if (!isAwake && distance < activationDistance && !playerAbove)
        {
            animator.SetTrigger("turnOn");
            isAwake = true;
        }
        else if (isAwake && playerAbove)
        {
            animator.SetTrigger("turnOff");
            isAwake = false;
            hasPlayedParticles = false; // reset
        }

        // ⚡ Trigger electricity particles when player is super close
        if (isAwake && !hasPlayedParticles && distance < particleTriggerDistance)
        {
            if (electricityParticles)
            {
                electricityParticles.Play();
                hasPlayedParticles = true;
            }
        }
    }
}
