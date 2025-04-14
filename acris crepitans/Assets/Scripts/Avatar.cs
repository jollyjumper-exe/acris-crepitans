using UnityEngine;
using TMPro;

public class Avatar : MonoBehaviour
{
    [SerializeField] private TMP_Text hitPointsText;

    private void OnTriggerEnter(Collider other)
    {
        PlayerManager.Instance.ReportCollision(other);
    }

    public void UpdateHitPoints(int hitPoints)
    {
        hitPointsText.text = hitPoints.ToString();
    }
}
