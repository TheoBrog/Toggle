using System.Collections;
using UnityEngine;

public class BasicMelee : EnemyLeftAndRight
{
    public float viewDistance; // Max distance the enemy can see the player
    public float maxMemory = 5f; // Max time to the enemy forget the player
    float enemyMemory = 0f;

    [Header("Attacking")]
    public float attackDistance = 1f;
    public GameObject attackObject;
    public float attackTime = 1f;

    [Header("Extra")]
    [SerializeField] TestEnemySounds enemySounds;

    protected override void Start()
    {
        base.Start();

        attackObject.SetActive(false);
        StartCoroutine(DelayedUpdate(0.1f));
    }

    IEnumerator DelayedUpdate(float delay)
    {
        yield return new WaitForSeconds(Random.Range(0f, 1f)); // Used for random execution time for multiple enemies

        while (true) // Loop
        {
            DetectState();

            if (currentState == EnemyState.Patroling)
                Patrol();
            else if (currentState == EnemyState.Chasing)
                Chase();

            enemyMemory -= delay;

            yield return new WaitForSeconds(delay); // Loop delay
        }
    }

    void DetectState()
    {
        if (currentState == EnemyState.Attacking) return;

        if (enemyMemory <= 0)
            currentState = EnemyState.Patroling;
        else
            currentState = EnemyState.Chasing;

        float playerDistance = Vector3.Distance(transform.position, player.transform.position);

        if (playerDistance < attackDistance)
            StartCoroutine(Attack());
        else if (playerDistance < viewDistance)
            enemyMemory = maxMemory;
    }

    IEnumerator Attack()
    {
        float sign = Mathf.Sign(player.transform.position.x - transform.position.x);

        currentState = EnemyState.Attacking;
        rb.linearVelocity = Vector3.zero;
        LookAtPlayer();
        yield return new WaitForSeconds(.5f);
        // Debug.Log("attack");
        attackObject.SetActive(true);
        Vector3 pos = attackObject.transform.localPosition;
        attackObject.transform.localPosition = new Vector3(Mathf.Abs(pos.x) * sign, 0, 0);
        attackObject.GetComponent<DamageBlock>().knockback = sign;
        SoundManager.instance.Play(enemySounds.swingAttack, 1, Random.Range(0.9f, 1.1f));
        yield return new WaitForSeconds(attackTime);
        attackObject.SetActive(false);
        yield return new WaitForSeconds(attackTime * 1.5f);
        currentState = EnemyState.Chasing;
    }

    [System.Serializable]
    class TestEnemySounds
    {
        public AudioClip swingAttack;
    }
}