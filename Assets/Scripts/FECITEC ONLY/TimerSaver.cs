using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using TMPro;

public class TimerSaver : MonoBehaviour
{
    string filePath;

    public TMP_Text text;

    void Start()
    {
        // Pega o caminho da área de trabalho do usuário
        string desktopPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
        filePath = Path.Combine(desktopPath, "TemposSalvos.txt");
        
        SaveTime();
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
        string line = $"Tempo: {formattedTime}";
        text.text = formattedTime;

        // Adiciona no arquivo (AppendText cria se não existir)
        using (StreamWriter sw = File.AppendText(filePath))
        {
            sw.WriteLine(line);
        }

        Debug.Log("Tempo salvo em: " + filePath + " -> " + formattedTime);
    }

    public void Voltar()
    {
        SceneManager.LoadScene("Menu");
    }
}