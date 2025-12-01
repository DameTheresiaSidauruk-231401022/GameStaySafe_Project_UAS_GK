using UnityEngine;

public class Player : MonoBehaviour
{
    public float playerSpeed = 5f;
    private Rigidbody2D rb;

    public Transform firePoint;
    public GameObject projectilePrefab;

    public int shardCount = 6;
    public float verticalSpread = 0.4f;
    public float shootCooldown = 0.15f;
    private float lastShootTime = 0f;

    private Vector2 playerDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.constraints = RigidbodyConstraints2D.FreezePositionX |
                         RigidbodyConstraints2D.FreezeRotation;
    }

    void Update()
    {
        float directionY = Input.GetAxisRaw("Vertical");
        playerDirection = new Vector2(0, directionY).normalized;

        if (Input.GetKey(KeyCode.Space) && Time.time > lastShootTime + shootCooldown)
        {
            ShootShards();
            lastShootTime = Time.time;
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(0, playerDirection.y * playerSpeed);
    }

    void ShootShards()
    {
        for (int i = 0; i < shardCount; i++)
        {
            float offsetY = Random.Range(-verticalSpread, verticalSpread);
            Vector3 spawnPos = firePoint.position + new Vector3(0, offsetY, 0);

            GameObject bullet = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);

            bullet.GetComponent<Projectile>().SetDirection(Vector2.right);
        }
    }

}
