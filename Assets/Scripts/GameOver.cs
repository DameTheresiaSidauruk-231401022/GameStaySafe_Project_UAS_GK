using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public GameObject gameOverPanel;
    private bool isGameOver = false;

    public void TriggerGameOver()
    {
        if (isGameOver) return; // cegah double muncul
        isGameOver = true;

        gameOverPanel.SetActive(true);

        // Hentikan waktu (optional, bisa aktifkan jika mau freeze game)
        // Time.timeScale = 0f;
    }

    public void Restart()
    {
        // Time.timeScale = 1f; // reset waktu
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
