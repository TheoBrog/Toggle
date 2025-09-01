using UnityEngine;

public class DamageBlock : MonoBehaviour
{
    public int damage;
    public bool dashInvincibility;
    public bool disableInv;
    public float knockback;

    PlayerMovement playerMovement;

    void Start()
    {
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
    }

    void OnTriggerEnter(Collider col)
    {
        // se não tocar no player
        if (!col.gameObject.CompareTag("Player"))
            return;

        // se o player está no dash com dashInvincibility
        if (dashInvincibility && playerMovement.state == PlayerState.dashing)
            return;

        if (damage > 0)
        {
            // função de dano
            playerMovement.Damage(damage, !disableInv, knockback);
            //StartCoroutine(playerMovement.Knockback(knockback));
        }
    }
}