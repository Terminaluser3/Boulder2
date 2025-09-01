using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject inGameHUD;

    private void OnEnable()
    {
        GameManager.OnStateChanged += HandleGameStateChanged;
        // gameOverPanel.SetActive(false);
        // inGameHUD.SetActive(false);
    }

    private void OnDisable()
    {
        GameManager.OnStateChanged -= HandleGameStateChanged;
    }

    private void HandleGameStateChanged(GameManager.GameState state)
    {
        Debug.Log("Handling state: " + state);

        // Deactivate all panels to clean up.
        mainMenuPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        inGameHUD.SetActive(false);

        switch (state)
        {
            case GameManager.GameState.MainMenu:
                mainMenuPanel.SetActive(true);
                break;
            case GameManager.GameState.Playing:
                inGameHUD.SetActive(true);
                break;
            case GameManager.GameState.GameOver:
                gameOverPanel.SetActive(true);
                break;
        }
    }
}

// @Author F.B.