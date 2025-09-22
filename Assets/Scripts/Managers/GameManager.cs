using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // GAME MANAGER
    public static GameManager instance;

    public List<GameObject> enemiesInScene = new();

    // ACTIONS
    public static Action endLevel;
    string nextScene;

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

    public void EndLevel(string name)
    {
        endLevel?.Invoke();
        
        nextScene = name;
        Invoke(nameof(GoToNextScene), 1f);
    }
    void GoToNextScene()
    {
        SceneManager.LoadSceneAsync(nextScene);
    }
}