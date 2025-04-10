using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    Playing,
    Paused,
    Lost
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public float CrawledHeight { get; private set; }

    public GameState CurrentState { get; private set; } = GameState.Playing;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void UpdateCrawledHeight(float newHeight)
    {
        CrawledHeight = newHeight;
    }

    public void LeaveGame(){
        SceneManager.LoadScene("MainMenu");
    }

    public int GetCurrentObstacleIntensity()
    {
        if(CrawledHeight < 5f) return 0;
        return Mathf.Clamp(Mathf.FloorToInt(CrawledHeight / 50f) + 1, 1, 10);
    }

    public float GetCurrentSpeed()
    {
        if(CurrentState == GameState.Paused || CurrentState == GameState.Lost) return 0f;   
        if(GetCurrentObstacleIntensity() < 1) return 5f;
        return Mathf.Clamp(5f + (float) CrawledHeight / 100f, 5, 15);
    }

    public void SetGameState(GameState newState)
    {
        CurrentState = newState;

        switch (newState)
        {
            case GameState.Lost:
                PlayerManager.Instance.KillPlayer();
                UIManager.Instance.ShowGameOverScreen();
                break;

            case GameState.Paused:
                break;

            case GameState.Playing:
                break;
        }
    }

}
