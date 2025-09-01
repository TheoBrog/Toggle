using UnityEngine;
using System.IO;
using UnityEngine.InputSystem;

public class RebindSaveLoad : MonoBehaviour
{
    public InputActionAsset actions;

    private static string pastaPreferencias => Path.Combine(Directory.GetParent(Application.dataPath).FullName, "Preferences");
    private static string caminhoConfig => Path.Combine(pastaPreferencias, "Binds.txt");

    public void SalvarConfiguracoes() {
        var rebinds = actions.SaveBindingOverridesAsJson();
        
        if (!Directory.Exists(pastaPreferencias))
        {
            Directory.CreateDirectory(pastaPreferencias);
        }

        File.WriteAllText(caminhoConfig, rebinds);
        //Debug.Log("Saved in : " + caminhoConfig);
    }

    public void CarregarConfiguracoes() {
        if (File.Exists(caminhoConfig))
        {
            string json = File.ReadAllText(caminhoConfig);
            actions.LoadBindingOverridesFromJson(json);
            //Debug.Log("Loaded");
        }
        else
        {
            //Debug.Log("No file found");
        }
    }

    public void OnEnable()
    {
        /*
        var rebinds = PlayerPrefs.GetString("rebinds");
        if (!string.IsNullOrEmpty(rebinds))
            actions.LoadBindingOverridesFromJson(rebinds);*/
        CarregarConfiguracoes();
    }

    public void OnDisable()
    {
        /*
        var rebinds = actions.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString("rebinds", rebinds);*/
        SalvarConfiguracoes();
    }
}
