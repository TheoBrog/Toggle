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
    
    public GameObject[] deadObj;
    protected bool isAlive = true;

    protected virtual void Start()
    {
        health = maxHealth;

        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");
        damageFlash = GetComponent<DamageFlash>();

        GameManager.instance.enemiesInScene.Add(transform.gameObject);
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
                SpriteRenderer s = d.GetComponent<SpriteRenderer>();
                
                s.flipX = transform.GetChild(0).GetComponent<SpriteRenderer>().flipX;
                s.color = Color.gray;
            }
        }
        else
            Destroy(gameObject);
                    
        foreach (var comp in GetComponents<MonoBehaviour>())
        {
            Destroy(comp);
        }    
    
        // Debug.Log("Killed " + enemyName);
        // Destroy(transform.gameObject);
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

    void Reset()
    {
        gameObject.layer = LayerMask.NameToLayer("Enemy");
        gameObject.GetComponent<Rigidbody>().freezeRotation = true;
    }
}