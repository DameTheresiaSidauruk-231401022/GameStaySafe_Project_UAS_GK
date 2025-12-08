using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public float playerSpeed = 5f;
    private Rigidbody2D rb;

    public Transform firePoint;
    public GameObject projectilePrefab;

    public int shardCount = 1;          // ✅ peluru 1 saja dulu
    public float verticalSpread = 0f;   // ✅ tanpa sebaran biar lurus
    public float shootCooldown = 0.2f;
    private float lastShootTime = 0f;

    private Vector2 playerDirection;
    private Vector3 normalScale;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.constraints = RigidbodyConstraints2D.FreezePositionX |
                         RigidbodyConstraints2D.FreezeRotation;

        normalScale = transform.localScale;

        // ✅ Auto cari FirePoint
        if (firePoint == null)
        {
            firePoint = transform.Find("FirePoint");
            if (firePoint == null)
                Debug.LogError("❌ FirePoint TIDAK DITEMUKAN!");
        }

        if (projectilePrefab == null)
            Debug.LogError("❌ Projectile Prefab BELUM DIISI!");
    }

    void Update()
    {
        float directionY = Input.GetAxisRaw("Vertical");
        playerDirection = new Vector2(0, directionY).normalized;

        if (Input.GetKey(KeyCode.Space) && Time.time > lastShootTime + shootCooldown)
        {
            Shoot();
            lastShootTime = Time.time;
            StartCoroutine(ShootScaleEffect());
        }
    }

    private void FixedUpdate()
    {
        // ✅ INI YANG BENAR (BUKAN linearVelocity)
        rb.velocity = new Vector2(0, playerDirection.y * playerSpeed);
    }

    void Shoot()
    {
        if (firePoint == null || projectilePrefab == null) return;

        Vector3 spawnPos = firePoint.position;

        GameObject bullet = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);

        Projectile p = bullet.GetComponent<Projectile>();
        if (p != null)
            p.SetDirection(Vector2.right); // ✅ TEMBAK KE KANAN
    }

    IEnumerator ShootScaleEffect()
    {
        Vector3 big = normalScale * 1.1f;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * 10f;
            transform.localScale = Vector3.Lerp(normalScale, big, t);
            yield return null;
        }

        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * 10f;
            transform.localScale = Vector3.Lerp(big, normalScale, t);
            yield return null;
        }
    }
}
