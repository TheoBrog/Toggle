using System.Collections;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{
    [SerializeField] Color _flashColor = Color.white;
    [SerializeField] float _flashTime = 0.25f;

    SpriteRenderer[] _spriteRenderers;
    Material[] _materials;

    Coroutine _damageFlashCoroutine;

    void Awake()
    {
        _spriteRenderers = GetComponentsInChildren<SpriteRenderer>();

        Init();
    }

    void Init()
    {
        _materials = new Material[_spriteRenderers.Length];

        for (int i = 0; i < _spriteRenderers.Length; i++){
            _materials[i] = _spriteRenderers[i].material;
        }
    }

    public void CallDamageFlash()
    {
        _damageFlashCoroutine = StartCoroutine(DamageFlasher());
    }

    IEnumerator DamageFlasher() {
        SetFlashColor();

        float currentFlashAmount = 0f;
        float elapsedTime = 0f;
        while (elapsedTime < _flashTime)
        {
            elapsedTime += Time.deltaTime;

            currentFlashAmount = Mathf.Lerp(1f, 0f, elapsedTime / _flashTime);
            SetFlashAmount(currentFlashAmount);

            yield return null;
        }
    }

    void SetFlashColor() {
        for (int i = 0; i < _materials.Length; i++)
        {
            _materials[i].SetColor("_FlashColor", _flashColor);
        }
    }

    void SetFlashAmount(float amount) {
        for (int i = 0; i < _materials.Length; i++)
        {
            _materials[i].SetFloat("_FlashAmount", amount);
        }
    }
}