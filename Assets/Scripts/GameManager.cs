using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverPanel;
    public GameObject winPanel;   // ✅ PANEL YOU WIN

    private bool gameEnded = false;

    public void GameOver()
    {
        if (gameEnded) return;

        gameEnded = true;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        Time.timeScale = 0f; // ⛔ STOP GAME
    }

    public void Win()
    {
        if (gameEnded) return;

        gameEnded = true;

        if (winPanel != null)
            winPanel.SetActive(true);

        // Time.timeScale = 0f; // ⛔ STOP GAME
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // ▶️ LANJUTKAN WAKTU
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
