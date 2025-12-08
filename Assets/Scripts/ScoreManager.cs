using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public Text scoreText;
    private int score = 0;

    private GameManager gm;
    private bool sudahMenang = false;

    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        scoreText.text = score.ToString();
    }

    public void AddScore(int amount)
    {
        if (sudahMenang) return; // ⛔ Stop tambah score setelah menang

        score += amount;

        if (score >= 100)
        {
            score = 100;
            sudahMenang = true;

            scoreText.text = score.ToString();

            if (gm != null)
                gm.Win();   // ✅ PANGGIL YOU WIN

            return;
        }

        scoreText.text = score.ToString();
    }
}
