using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public float speed = 3f;
    public float lifeTime = 6f;
    public float rotateSpeed = 80f;

    private bool sudahKena = false; // mencegah score dobel

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime, Space.World);
        transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ---- PLAYER KENA → GAME OVER ----
        if (collision.CompareTag("Player"))
        {
            GameOver go = FindObjectOfType<GameOver>();
            if (go != null) go.TriggerGameOver();

            Destroy(collision.gameObject); // hancurkan player
            return;
        }

        // ---- VIRUS KENA PELURU (NO DOUBLE SCORE) ----
        if (collision.CompareTag("Projectile") && !sudahKena)
        {
            sudahKena = true;

            ScoreManager sm = FindObjectOfType<ScoreManager>();
            if (sm != null) sm.AddScore(10);

            Destroy(gameObject); // hancurkan virus
        }
    }
}
