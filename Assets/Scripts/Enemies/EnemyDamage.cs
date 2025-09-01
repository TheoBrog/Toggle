using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public int damage;

    void OnTriggerEnter(Collider collider)
    {
        GameObject obj = collider.gameObject;
        if (obj.GetComponent<EnemyBase>() && damage != 0)
        {
            obj.GetComponent<EnemyBase>().TakeDamage(damage);
        }
    }
}
