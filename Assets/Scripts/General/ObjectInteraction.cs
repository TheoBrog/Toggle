using Unity.VisualScripting;
using UnityEngine;

public class ObjectInteraction : MonoBehaviour
{
    public bool knockbackPlayer = true;
    DamageFlash damageFlash;

    protected virtual void Start()
    {
        if (GetComponent<DamageFlash>())
            damageFlash = GetComponent<DamageFlash>();
    }

    public void Activate()
    {
        OnAttack();
    }

    protected virtual void OnAttack()
    {
        if (damageFlash != null)
            damageFlash.CallDamageFlash();
    }

    void Reset()
    {
        transform.AddComponent<DamageFlash>();
        GetComponent<Collider>().isTrigger = true;
    }
}
