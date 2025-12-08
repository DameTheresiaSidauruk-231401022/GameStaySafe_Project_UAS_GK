using UnityEngine;
using System.Collections;

public class Virus : MonoBehaviour

    
{
    private EnemyVisuals visuals;
    public float speed = 3f;
    public float lifeTime = 6f;
    public float rotateSpeed = 80f;

    private bool sudahKena = false;

    void Start()
    {
        Destroy(gameObject, lifeTime);
        visuals = GetComponent<EnemyVisuals>();
    }

    void Update()
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime, Space.World);
        transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ✅ JIKA KENA PLAYER → GAME OVER
        if (collision.CompareTag("Player"))
        {
            GameOver go = FindObjectOfType<GameOver>();
            if (go != null) go.TriggerGameOver();

            Destroy(collision.gameObject);
            return;
        }

        // ✅ JIKA KENA PROJECTILE
        if (collision.CompareTag("Projectile") && !sudahKena)
        {
            sudahKena = true;

            // 🔥 FLASH SHADER
            if (visuals != null)
            {
                print("trigger flash");
                visuals.TriggerHitFlash();   // ✅ NAMA YANG BENAR
            }

            ScoreManager sm = FindObjectOfType<ScoreManager>();
            if (sm != null) sm.AddScore(10);

            Destroy(collision.gameObject);

            StartCoroutine(ShrinkThenDestroy());
        }
    }

    IEnumerator ShrinkThenDestroy()
    {
        Vector3 original = transform.localScale;
        Vector3 target = original * 0.1f;

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * 6f;
            transform.localScale = Vector3.Lerp(original, target, t);
            yield return null;
        }

        Destroy(gameObject);
    }
}
