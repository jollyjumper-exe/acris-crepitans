using UnityEngine;

public class Avatar : MonoBehaviour
{
    public PlayerManager playerManager;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            playerManager.ReportCollision();
        }
    }
}
