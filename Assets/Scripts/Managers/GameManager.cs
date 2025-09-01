using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    // GAME MANAGER
    public static GameManager instance;

    public List<GameObject> enemiesInScene = new();

    // ACTIONS
    public static Action onToggle;
    public static Action onDeath;

    public static Action<GameObject> enemyDeath;

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void ResetGame()
    {
        onDeath?.Invoke();
    }
}