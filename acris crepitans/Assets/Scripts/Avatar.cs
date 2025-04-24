using UnityEngine;
using TMPro;

public class Avatar : MonoBehaviour
{
    [SerializeField] private TMP_Text hitPointsText;
    private Renderer rend;
     
    void Awake()
    {
        rend = GetComponent<Renderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerManager.Instance.ReportCollision(other);
    }

    public void UpdateHitPoints(int hitPoints)
    {
        hitPointsText.text = hitPoints.ToString();

        if (rend && rend.material.HasProperty("_Color"))
        {
            Color color;

            switch (hitPoints)
            {
                case 3:
                    color = Color.white;
                    break;
                case 2:
                    color = new Color(1f, 0.5f, 0.5f);
                    break;
                case 1:
                    color = Color.red;
                    break;
                default:
                    color = Color.black;
                    break;
            }

            rend.material.color = color;
        }
    }

}
