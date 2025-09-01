using System.Collections;
using UnityEngine;

public class BasicShooter : EnemyShooter
{
    [Header("Basic Shooter")]
    public float maxDistance;

    public float maxDelay;
    public float delay;

    SpriteRenderer spriteRenderer;

    protected override void Start()
    {
        base.Start();

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
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
                if (delay >= maxDelay)
                {
                    delay = 0;
                    PlayFlash();
                    yield return new WaitForSeconds(.5f);
                    Fire(player.transform);
                }

                spriteRenderer.flipX = player.transform.position.x < transform.position.x;
            }

            if (delay < maxDelay)
                delay += loopDelay;
        }
    }
}
