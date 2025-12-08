using UnityEngine;

public class ProjectileGlowAnim : MonoBehaviour
{
    public Material mat;
    public float minIntensity = 0.5f;
    public float maxIntensity = 2f;
    public float speed = 4f;

    private float time;

    void Start()
    {
        if (mat == null)
            mat = GetComponent<SpriteRenderer>().material;
    }

    void Update()
    {
        time += Time.deltaTime * speed;
        float pulse = Mathf.Lerp(minIntensity, maxIntensity, (Mathf.Sin(time) + 1f) * 0.5f);
        mat.SetFloat("_GlowIntensity", pulse);
    }
}
