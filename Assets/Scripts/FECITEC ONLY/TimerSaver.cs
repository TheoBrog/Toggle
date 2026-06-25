using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using TMPro;

public class TimerSaver : MonoBehaviour
{
    string filePath;

    public TMP_Text timerText;
    public TMP_Text deathText;

    void Start()
    {
        deathText.text = GameTimer.deathCount.ToString();
    }

    public void SaveTime()
    {
        // Pega o tempo atual do GameTimer
        float currentTime = GameTimer.timeElapsed;

        // Converte para minutos, segundos e milissegundos
        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);
        int milliseconds = Mathf.FloorToInt((currentTime * 1000f) % 1000f);

        // Formata no estilo mm:ss:ms
        string formattedTime = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);

        // Monta a linha
        string line = $"Tempo: {formattedTime} | Falhas: {GameTimer.deathCount}";
        timerText.text = formattedTime;
    }

    public void Voltar()
    {
        SceneManager.LoadScene("Menu");
    }
}