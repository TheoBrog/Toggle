using System.Collections;
using UnityEngine;

public class ParryFlash : MonoBehaviour
{
    public float lifeTime = 1f;   // Tempo total até desaparecer
    public float maxScale = 2f;   // Escala máxima final
    public float rotationSpeed = 180f; // Velocidade de rotação (graus por segundo)

    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private Vector3 initialScale;

    public AudioClip audioClip;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        initialScale = transform.localScale;

        if (audioClip != null)
            SoundManager.instance.Play(audioClip);
        StartCoroutine(LifeTime());
    }

    IEnumerator LifeTime()
    {
        float elapsed = 0;

        while (elapsed < lifeTime)
        {
            float t = elapsed / lifeTime; // Normaliza o tempo (0 → 1)

            // Scale (cresce suavemente até maxScale)
            float scale = Mathf.Lerp(initialScale.x, maxScale, t);
            transform.localScale = new Vector3(scale, scale, initialScale.z);

            // Rotation
            transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);

            // Transparency (alpha de 1 → 0)
            float alpha = Mathf.Lerp(1f, 0f, t);
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

            elapsed += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}
