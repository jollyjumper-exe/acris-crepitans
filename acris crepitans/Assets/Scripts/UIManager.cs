using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TMP_Text heightText;

    void Update()
    {
        float crawledHeight = GameManager.Instance.CrawledHeight;

        heightText.text = Mathf.RoundToInt(crawledHeight).ToString();
    }
}