using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class ConfigMenu : MonoBehaviour
{
    [Header("Main")]
    public ConfigManager configManager;

    [Header("Music & Sounds")]
    [SerializeField] AudioMixer audioMixer;
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider soundSlider;

    void Start()
    {
        Load();
    }

    public void Load()
    {
        configManager.CarregarConfiguracoes();

        masterSlider.value = configManager.configuracoes.masterVolume;
        musicSlider.value = configManager.configuracoes.musicVolume;
        soundSlider.value = configManager.configuracoes.soundVolume;

        ApplyValues();
    }

    public void AplicarConfiguracoes()
    {
        configManager.configuracoes.masterVolume = masterSlider.value;
        configManager.configuracoes.musicVolume = musicSlider.value;
        configManager.configuracoes.soundVolume = soundSlider.value;

        configManager.SalvarConfiguracoes();
        ApplyValues();
    }

    public void ApplyValues()
    {
        audioMixer.SetFloat("masterVolume", Mathf.Log10(masterSlider.value) * 20f);
        audioMixer.SetFloat("musicVolume", Mathf.Log10(musicSlider.value) * 20f);
        audioMixer.SetFloat("soundVolume", Mathf.Log10(soundSlider.value) * 20f);
    }

    void OnDisable()
    {
        configManager.SalvarConfiguracoes();
    }
}