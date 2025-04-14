using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] TMP_Text heightText;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject gameplayUI;
    [SerializeField] private TMP_Text finalScoreText;
    [SerializeField] private TMP_Text coinsText;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Update()
    {
        float crawledHeight = GameManager.Instance.CrawledHeight;

        heightText.text = Mathf.RoundToInt(crawledHeight).ToString();
    }

    public void ShowGameOverScreen()
    {
        gameplayUI.SetActive(false);
        gameOverScreen.SetActive(true);
        finalScoreText.text = Mathf.RoundToInt(GameManager.Instance.CrawledHeight).ToString();
    }

    public void OnTapReplay()
    {
        GameManager.Instance.LeaveGame();
    }

    public void UpdateCoins(int coins)
    {
        coinsText.text = coins.ToString();
    }
}