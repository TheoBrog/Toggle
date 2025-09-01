using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class BulletScript : MonoBehaviour
{
    public float speed = 1;
    public bool destroyOnTouch = true;
    public float deleteTimer;
    public bool parryable;
    public bool deleteOnTouch;

    [HideInInspector] public bool parriedBullet;

    BoxCollider boxCollider;
    DamageBlock damageBlock;
    EnemyDamage enemyDamage;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        boxCollider.isTrigger = true;

        Invoke(nameof(BulletDestroy), deleteTimer);

        damageBlock = GetComponent<DamageBlock>();
        enemyDamage = GetComponent<EnemyDamage>();
    }

    void Update()
    {
        if (!parriedBullet)
            transform.position += transform.forward * speed * Time.deltaTime;
        else
            transform.position += transform.forward * -speed * 1.5f * Time.deltaTime;
        //transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0);
    }

    void OnTriggerEnter(Collider col)
    {
        // layer ground
        if (col.gameObject.layer == 3 && destroyOnTouch)
        {
            Destroy(gameObject);
        }

        if (deleteOnTouch)
            Destroy(gameObject);
    }

    public void Parry()
    {
        if (parryable)
        {
            parriedBullet = true;
            damageBlock.damage = 0;
            damageBlock.knockback = 0;
            if (enemyDamage != null)
                enemyDamage.damage = 1;
        }
    }

    public void BulletDestroy()
    {
        Destroy(gameObject);
    }
}