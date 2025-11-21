using UnityEngine;

public class GameTimer : MonoBehaviour
{
    public static float timeElapsed;
    public static int deathCount;

    void Update()
    {
        timeElapsed += Time.deltaTime;
        // Debug.Log(Mathf.Floor(timeElapsed));
    }
}