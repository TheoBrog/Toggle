using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDoor : MonoBehaviour
{
    public EnemyWave[] enemyList;
    List<GameObject> enemies = new();
    List<GameObject> deadEnemies = new();

    public DoorClass[] doors;
    bool spawned = false;
    bool ended = false;

    public int currentWave;
    int maxWave;

    GameObject fromTrigger;
    bool isReseting = false;

    void Start()
    {
        Door(false);
        foreach (DoorClass dc in doors)
        {
            dc.door.transform.SetParent(dc.side.transform);
            dc.door.GetComponentInChildren<SpriteRenderer>().color = dc.side.GetComponent<SideSettings>().color;
        }
    }

    #region Sequence
    public void Spawn(GameObject from)
    {
        fromTrigger = from;
        if (!spawned)
        {
            // Debug.Log("Spawn Waves");
            CalculateMaxWave();

            spawned = false;
            from.SetActive(false);

            currentWave = 0;

            Door(true);

            SpawnEnemy();

            StartCoroutine(LoopCheck(.5f));
        }
    }

    IEnumerator LoopCheck(float delay)
    {
        yield return new WaitForSeconds(0.25f);

        int loopCount = 0;

        while (true)
        {
            while (enemies.Count > 0 || isReseting)
            {
                yield return new WaitForSeconds(delay);
            }

            // sobe a wave
            currentWave++;

            // se passou do máximo
            if (currentWave > maxWave)
            {
                loopCount++;
                currentWave = 1;

                if (loopCount >= 1)
                {
                    EndSequence();
                    yield break;
                }
            }

            SpawnEnemy();

            yield return new WaitForSeconds(delay);
        }
    }

    void EndSequence()
    {
        if (isReseting) return;
        Debug.Log("End Waves");
        Door(false);
        ended = true;
        GameManager.onDeath -= ResetDoors;
        // Invoke(nameof(DeleteDoors), .25f);
    }

    void Door(bool state)
    {
        foreach (DoorClass dc in doors)
        {
            GameObject d = dc.door;

            if (d.GetComponent<Animator>())
            {
                if (state)
                    d.GetComponent<Animator>().Play("Close");
                else
                    d.GetComponent<Animator>().Play("Open");
            }
            else
                d.SetActive(state);
        }
    }

    void DeleteDoors()
    {
        foreach (DoorClass dc in doors)
        {
            GameObject d = dc.door;
            Destroy(d);
        }
    }

    #endregion

    #region Waves
    void SpawnEnemy()
    {
        foreach (EnemyWave enemy in enemyList)
        {
            if (enemy.spawnWave == currentWave)
            {
                GameObject obj = Instantiate(enemy.enemyPrefab);
                obj.transform.SetParent(enemy.side);
                obj.transform.position = enemy.spawnPosition.position;
                Vector3 pos = obj.transform.localPosition;
                obj.transform.localPosition = new Vector3(pos.x, pos.y, 0);
                enemies.Add(obj);
            }
        }
    }

    void CalculateMaxWave()
    {
        int max = 0;
        maxWave = 0;
        foreach (EnemyWave enemy in enemyList)
        {
            if (enemy.spawnWave > max)
            {
                max = enemy.spawnWave;
            }
        }
        maxWave = max;
    }

    void EnemyDied(GameObject enemy)
    {
        if (enemies.Contains(enemy))
        {
            enemies.Remove(enemy);
            deadEnemies.Add(enemy);
        }
    }
    #endregion

    #region Reseting
    void ResetDoors()
    {
        // Debug.Log("reset door fun");
        // Evitar bugs
        if (ended)
            return;

        isReseting = true;
        StopAllCoroutines();

        currentWave = 9999;
        // Deletar os inimigos que existem
        foreach (GameObject obj in enemies)
        {
            if (obj != null)
                obj.GetComponent<EnemyBase>().DeleteEnemy();
        }
        foreach (GameObject obj in deadEnemies)
            Destroy(obj);
        enemies.Clear();
        deadEnemies.Clear();

        // Fechar as portas e ligar triggers
        Door(false);
        if (fromTrigger != null)
            fromTrigger.SetActive(true);
        spawned = false;

        isReseting = false;
    }

    void OnEnable()
    {
        // Debug.Log("enable debug");
        GameManager.onDeath += ResetDoors;
        GameManager.enemyDeath += EnemyDied;
    }

    void OnDisable()
    {
        // Debug.Log("disable debug");
        GameManager.onDeath -= ResetDoors;
        GameManager.enemyDeath -= EnemyDied;
    }

    #endregion

    [System.Serializable]
    public class EnemyWave
    {
        public string name;

        public GameObject enemyPrefab;
        public Transform spawnPosition;

        public Transform side;
        public int spawnWave;
    }

    [System.Serializable]
    public class DoorClass
    {
        public GameObject door;
        public GameObject side;
    }
}