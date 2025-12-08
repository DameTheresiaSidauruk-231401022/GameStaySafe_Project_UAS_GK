using UnityEngine;
using System.Collections;

public class EnemyVisuals : MonoBehaviour
{
    [Header("Settings")]
    public float flashDuration = 0.15f; // Berapa lama kedip putihnya

    private Material enemyMat;
    private Coroutine flashRoutine;

    void Start()
    {
        // Ambil material dari SpriteRenderer agar bisa diedit
        enemyMat = GetComponent<SpriteRenderer>().material;
    }

    public void TriggerHitFlash()
    {
        // Kalau sedang kedip, stop dulu biar bisa kedip ulang (reset)
        if (flashRoutine != null) StopCoroutine(flashRoutine);

        flashRoutine = StartCoroutine(FlashProcess());
    }

    IEnumerator FlashProcess()
    {
        // Ubah jadi Putih (Set _FlashAmount ke 1)
        enemyMat.SetFloat("_FlashAmount", 1f);

        // Tunggu sebentar
        yield return new WaitForSeconds(flashDuration);

        // Ubah jadi Normal lagi (Set _FlashAmount ke 0)
        enemyMat.SetFloat("_FlashAmount", 0f);

        flashRoutine = null;
    }
}