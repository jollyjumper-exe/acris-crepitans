using UnityEngine;

public class Avatar : MonoBehaviour
{
    public PlayerManager playerManager;

    private void OnTriggerEnter(Collider other)
    {
        playerManager.ReportCollision(other);
    }
}
