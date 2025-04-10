using UnityEngine;

public class Avatar : MonoBehaviour
{
    public PlayerManager playerManager;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision");
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            playerManager.ReportCollision();
        }
    }
}
