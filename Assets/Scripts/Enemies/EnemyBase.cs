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
    public float maxHealth = 100f;
    protected float health;

    public string enemyName = "Basic Enemy";

    public EnemyType enemyType;
    protected EnemyState currentState = EnemyState.Patroling;

    protected Rigidbody rb;
    protected GameObject player;

    DamageFlash damageFlash;

    public AudioClip damageClip;

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
        // Debug.Log("Killed " + enemyName);
        GameManager.instance.enemiesInScene.Remove(transform.gameObject);
        Destroy(transform.gameObject);
        GameManager.enemyDeath?.Invoke(transform.gameObject);
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