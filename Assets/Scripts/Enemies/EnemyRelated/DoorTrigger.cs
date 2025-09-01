using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public EnemyDoor enemyDoor;

    void Start()
    {
        if (enemyDoor == null)
            enemyDoor = transform.parent.GetComponent<EnemyDoor>();
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            enemyDoor.Spawn(gameObject);
        }
    }

    void Reset()
    {
        GetComponent<BoxCollider>().isTrigger = true;
    }
}
