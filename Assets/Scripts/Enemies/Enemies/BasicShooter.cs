using System.Collections;
using UnityEngine;

public class BasicShooter : EnemyShooter
{
    [Header("Basic Shooter")]
    public float maxDistance;

    public float maxDelay;
    public float delay;

    SpriteRenderer spriteRenderer;
    Animator anim;

    public Sprite[] enemySprites;
    public Transform gunPivot;
    public Light gunLight;
    public ParticleSystem particle;

    protected override void Start()
    {
        base.Start();

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponent<Animator>();
        StartCoroutine(EnemyLoop(.25f));
    }

    IEnumerator EnemyLoop(float loopDelay)
    {
        yield return new WaitForSeconds(Random.Range(0f, 2f));
        while (true)
        {
            yield return new WaitForSeconds(loopDelay);
            if (Vector3.Distance(transform.position, player.transform.position) < maxDistance)
            {
                spriteRenderer.sprite = enemySprites[1];
                if (delay >= maxDelay - 0.5f)
                {
                    delay = 0;
                    PlayFlash();
                    anim.Play("Prepare");
                    yield return new WaitForSeconds(.5f);
                    anim.Play("Fire");
                    StartCoroutine(GunEffect());
                    Fire(player.transform);
                }

                spriteRenderer.flipX = player.transform.position.x > transform.position.x;
                if (player.transform.position.x > transform.position.x)
                    gunPivot.localScale = new Vector3(-1, 1, 1);
                else
                    gunPivot.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                spriteRenderer.sprite = enemySprites[0];
            }

            if (delay < maxDelay)
                delay += loopDelay;
        }
    }

    IEnumerator GunEffect()
    {
        particle.Play();

        gunLight.enabled = true;
        yield return new WaitForSeconds(0.1f);
        gunLight.enabled = false;
    }
}
