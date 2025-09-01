using UnityEngine;

public class AttackDamage : MonoBehaviour
{
    public int damage;
    PlayerMovement playerMovement;

    void Start()
    {
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
    }

    void OnTriggerEnter(Collider collider)
    {
        GameObject obj = collider.gameObject;
        if (obj.GetComponent<EnemyBase>() && damage != 0)
        {
            obj.GetComponent<EnemyBase>().TakeDamage(damage);
            playerMovement.StartCoroutine(playerMovement.AttackKnockback());
        }
    }
}
