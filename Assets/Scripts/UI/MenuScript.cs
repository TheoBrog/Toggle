using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public GameObject menuObject;
    public GameObject menuBack;

    public GameObject settingsObject;
    public GameObject settingsFirst;

    public GameObject loadingPanel;

    public void PlayButton(string scene)
    {
        loadingPanel.SetActive(true);
        SceneManager.LoadScene(scene);
        GameTimer.timeElapsed = 0;
    }

    public void SettingsButton()
    {
        menuObject.SetActive(false);
        settingsObject.SetActive(true);
        SetFirst(settingsFirst);
    }

    public void CloseSettings()
    {
        menuObject.SetActive(true);
        settingsObject.SetActive(false);
        SetFirst(menuBack);
    }

    public void QuitButton()
    {
        Application.Quit();
    }
    
    void SetFirst(GameObject pos)
    {
        EventSystem.current.SetSelectedGameObject(pos);
    }
}
