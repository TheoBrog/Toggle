using UnityEngine;

public enum EnemyType
{
    Basic,
    Respawn,
    Boss
}

public enum EnemyState
{
    Idle,
    Patroling,
    Chasing,
    Attacking,
    Dead
}

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class EnemyBase : MonoBehaviour
{
    [Header("Enemy Basics")]
    // Basics
    public float maxHealth = 100f;
    protected float health;

    public string enemyName = "Basic Enemy";

    // States
    public EnemyType enemyType;
    protected EnemyState currentState = EnemyState.Patroling;

    // Components
    protected Rigidbody rb;
    protected GameObject player;

    // Quality of Life
    public AudioClip damageClip;
    public GameObject flash;

    DamageFlash damageFlash;
    public GameObject particleSpawn;
    
    public GameObject[] deadObj;
    protected bool isAlive = true;

    protected virtual void Start()
    {
        health = maxHealth;

        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");
        damageFlash = GetComponent<DamageFlash>();

        GameManager.instance.enemiesInScene.Add(transform.gameObject);

        GameObject p = Instantiate(particleSpawn);
        p.transform.SetParent(null);
        p.transform.position = transform.position;
        p.transform.localEulerAngles = Vector3.zero;
        Destroy(p, 1f);

        ApplyColorFromSideSettings();
    }

    public virtual void TakeDamage(float dmg)
    {
        health -= dmg;
        damageFlash.CallDamageFlash();
        if (damageClip != null)
            SoundManager.instance.Play(damageClip);
        if (health <= 0)
            EnemyDie();
    }

    protected virtual void EnemyDie()
    {
        isAlive = false;

        GameManager.instance.enemiesInScene.Remove(transform.gameObject);
        GameManager.enemyDeath?.Invoke(transform.gameObject);        

        if (deadObj.Length > 0)
        {
            foreach (Transform child in gameObject.transform)
            {
                child.gameObject.SetActive(false);
            }
            foreach (GameObject d in deadObj)
            {
                d.SetActive(true);
                SpriteRenderer sr = d.GetComponent<SpriteRenderer>();
                float darkenAmount = 0.5f;

                Color original = sr.color;
                sr.color = new Color(
                    original.r * darkenAmount,
                    original.g * darkenAmount,
                    original.b * darkenAmount,
                    original.a // mantém a transparência
                );
            }
        }
        else
            Destroy(gameObject);
                    
        foreach (var comp in GetComponents<MonoBehaviour>())
        {
            Destroy(comp);
        }    
    }

    protected virtual void PlayFlash()
    {
        GameObject pf = Instantiate(flash);
        pf.transform.SetParent(null);
        pf.transform.position = transform.position;
        pf.transform.localEulerAngles = Vector3.zero;
    }

    public void DeleteEnemy()
    {
        // Debug.Log("Deleted " + enemyName);
        GameManager.instance.enemiesInScene.Remove(transform.gameObject);
        Destroy(transform.gameObject);
    }

    protected virtual void ApplyColorFromSideSettings()
    {
        // tenta achar o SideSettings em até 5 níveis
        SideSettings settings = GetSideSettings();
        if (settings == null) return;

        Color targetColor = settings.color;

        // pega todos os SpriteRenderers nos filhos
        SpriteRenderer[] renderers = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sr in renderers)
        {
            if (sr.gameObject != this.gameObject) // não muda o do objeto atual
            {
                sr.color = targetColor;
            }
        }
    }

    SideSettings GetSideSettings()
    {
        Transform current = transform;
        int limit = 5;

        for (int i = 0; i < limit; i++)
        {
            if (current == null) break;

            SideSettings settings = current.GetComponent<SideSettings>();
            if (settings != null)
            {
                return settings;
            }

            current = current.parent;
        }

        return null;
    }

    void Reset()
    {
        gameObject.layer = LayerMask.NameToLayer("Enemy");
        gameObject.GetComponent<Rigidbody>().freezeRotation = true;
    }
}