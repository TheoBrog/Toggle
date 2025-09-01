using System.IO;
using UnityEngine;

[System.Serializable]
public class Configuracoes {
    public float masterVolume = 1f;
    public float musicVolume = 1f;
    public float soundVolume = 1f;
}

public class ConfigManager : MonoBehaviour {
    private static string pastaPreferencias => Path.Combine(Directory.GetParent(Application.dataPath).FullName, "Preferences");
    private static string caminhoConfig => Path.Combine(pastaPreferencias, "Configs.txt");

    public Configuracoes configuracoes = new Configuracoes();

    public void SalvarConfiguracoes() {
        if (!Directory.Exists(pastaPreferencias)) {
            Directory.CreateDirectory(pastaPreferencias);
        }

        string json = JsonUtility.ToJson(configuracoes, true);
        File.WriteAllText(caminhoConfig, json);
        //Debug.Log("Saved in : " + caminhoConfig);
    }

    public void CarregarConfiguracoes() {
        if (File.Exists(caminhoConfig)) {
            string json = File.ReadAllText(caminhoConfig);
            configuracoes = JsonUtility.FromJson<Configuracoes>(json);
            //Debug.Log("Loaded from : " + caminhoConfig);
        } else {
            //Debug.Log("No file found");
        }
    }
}