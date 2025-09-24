using UnityEngine;

public class GameTimer : MonoBehaviour
{
    public static float timeElapsed;

    void Update()
    {
        timeElapsed += Time.deltaTime;
        // Debug.Log(Mathf.Floor(timeElapsed));
    }
}