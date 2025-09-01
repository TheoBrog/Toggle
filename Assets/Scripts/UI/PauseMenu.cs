using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    [Header("Pause Shenanigans")]
    public static bool isPaused = false;

    CanvasGroup canvasGroup;
    public static PauseMenu instance;

    [Header("UI Panels")]
    public GameObject pauseMenu;
    public GameObject settingsMenu;

    [Header("First Selected Options")]
    public GameObject mainMenuFirst;
    public GameObject settingsFirst;
    public GameObject backFromSettingsFirst;

    [Header("Low Cut Stuff")]
    public AudioMixer mixer;
    public string exposedParam = "Master"; // nome exposto no mixer
    public float transitionTime = 1.0f; // segundos

    private Coroutine currentTransition;

    void Start()
    {
        instance = this;

        canvasGroup = pauseMenu.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;

        pauseMenu.SetActive(false);
    }

    void Update()
    {
        if (UserInput.instance.MenuPress && !settingsMenu.activeSelf)
        {
            if (!isPaused)
                OpenPauseMenu();
            else
                ClosePauseMenu();
        }
    }

    #region PAUSE/RESUME
    void Pause()
    {
        isPaused = true;    

        StartCoroutine(FadeCanvasGroup(0f, 1f, transitionTime));
        FadeToAbafado();

        Time.timeScale = 0f;
    }

    void Resume()
    {
        isPaused = false;

        StartCoroutine(FadeOutAndDisable(transitionTime));
        StopCoroutine(FadeCanvasGroup(0, 0, 0));
        FadeToNormal();

        Time.timeScale = 1f;
    }
    #endregion

    #region MENU FUNCTIONS
    public void OpenPauseMenu()
    {
        if (!isPaused)
            Pause();
        
        pauseMenu.SetActive(true);
        settingsMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(mainMenuFirst);
    }

    public void ClosePauseMenu()
    {
        Resume();

        EventSystem.current.SetSelectedGameObject(null);
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(false);
    }

    public void SettingsButton()
    {
        EventSystem.current.SetSelectedGameObject(settingsFirst);
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(true);
        settingsMenu.GetComponent<SettingsMenu>().FirstEnter();
    }

    public void OpenFromSettings()
    {
        if (!isPaused)
            Pause();
        
        pauseMenu.SetActive(true);
        settingsMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(backFromSettingsFirst);
    }

    public void Menu()
    {
        Time.timeScale = 1f;
        isPaused = false;
        mixer.SetFloat(exposedParam, 22000f);
        SceneManager.LoadScene(0);
    }
    #endregion

    #region FADE EFFECTS
    void FadeToAbafado()
    {
        StartLowPassTransition(22000f, 500f); // de limpo para abafado
    }

    void FadeToNormal()
    {
        StartLowPassTransition(500f, 22000f); // de abafado para limpo
    }

    private void StartLowPassTransition(float from, float to)
    {
        if (currentTransition != null)
            StopCoroutine(currentTransition);

        currentTransition = StartCoroutine(LerpLowPass(from, to));
    }

    private IEnumerator LerpLowPass(float from, float to)
    {
        float elapsed = 0f;

        while (elapsed < transitionTime)
        {
            elapsed += Time.unscaledDeltaTime; // usar unscaled para continuar mesmo se o Time.timeScale for 0
            float t = elapsed / transitionTime;
            float value = Mathf.Lerp(from, to, t);
            mixer.SetFloat(exposedParam, value);
            yield return null;
        }

        mixer.SetFloat(exposedParam, to);
    }

    private IEnumerator FadeCanvasGroup(float from, float to, float duration)
    {
        float elapsed = 0f;
        canvasGroup.alpha = from;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            canvasGroup.alpha = Mathf.Lerp(from, to, elapsed / duration);
            yield return null;
        }

        canvasGroup.alpha = to;
    }

    private IEnumerator FadeOutAndDisable(float duration)
    {
        yield return FadeCanvasGroup(1f, 0f, duration);
        pauseMenu.SetActive(false);
    }
    #endregion
}
