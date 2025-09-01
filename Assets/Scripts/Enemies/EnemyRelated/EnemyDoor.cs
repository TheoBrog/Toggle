using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDoor : MonoBehaviour
{
    public EnemyWave[] enemyList;
    List<GameObject> enemies = new();

    public GameObject[] doors;
    bool spawned = false;
    bool ended = false;

    public int currentWave;
    int maxWave;

    GameObject fromTrigger;

    void Start()
    {
        Door(false);
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
            while (enemies.Count > 0)
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
        // Debug.Log("End Waves");
        Door(false);
        ended = true;
    }

    void Door(bool state)
    {
        foreach (GameObject d in doors)
        {
            d.SetActive(state);
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
                obj.transform.parent = enemy.side;
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
        }
    }
    #endregion

    #region Reseting
    void ResetDoors()
    {
        if (ended)
            return;
        currentWave = 9999;
        foreach (GameObject obj in enemies)
        {
            if (obj != null)
                obj.GetComponent<EnemyBase>().DeleteEnemy();
        }
        enemies.Clear();
        Door(false);
        if (fromTrigger != null)
            fromTrigger.SetActive(true);
        spawned = false;
        
        GameManager.onDeath -= ResetDoors;
    }    

    void OnEnable()
    {
        // Inscreve a função AbrirPorta para ser chamada quando OnDeath for disparado
        GameManager.onDeath += ResetDoors;
        GameManager.enemyDeath += EnemyDied;
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
}