using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class SettingsMenu : MonoBehaviour
{
    [Header("UI")]
    public GameObject[] uiPanels;
    public GameObject[] firstPositions;
    public GameObject[] gridButtons;
    public GameObject settingsBase;

    [Header("Saving Stuff")]
    public ConfigMenu configMenu;
    public MenuScript menuScript;
    public PauseMenu pauseMenu;

    [Header("Inputs")]
    public InputActionReference cancelAction; // arrasta "UI/Cancel" aqui no inspetor

    private void OnEnable()
    {
        cancelAction.action.performed += OnCancel;
        cancelAction.action.Enable();
    }

    private void OnDisable()
    {
        cancelAction.action.performed -= OnCancel;
        cancelAction.action.Disable();
    }

    private void OnCancel(InputAction.CallbackContext ctx)
    {
        if (settingsBase.activeSelf)
        {
            CloseSettings();
        }
        else
        {
            ShowBase();
        }
    }


    public void GraphicsButton()
    {
        HideAllPanels();
        uiPanels[0].SetActive(true);
        SetFirst(firstPositions[0]);
    }

    public void AudioButton()
    {
        HideAllPanels();
        uiPanels[1].SetActive(true);
        SetFirst(firstPositions[1]);

        configMenu.Load();
    }

    public void KeyboardBindsButton()
    {
        HideAllPanels();
        uiPanels[2].SetActive(true);
        SetFirst(firstPositions[2]);
    }

    public void GamepadBindsButton()
    {
        HideAllPanels();
        uiPanels[3].SetActive(true);
        SetFirst(firstPositions[3]);
    }

    void HideAllPanels()
    {
        foreach (GameObject current in uiPanels)
        {
            current.SetActive(false);
        }
        settingsBase.SetActive(false);
    }

    public void ShowBase()
    {
        BaseGoBack();
        HideAllPanels();
        settingsBase.SetActive(true);
        configMenu.AplicarConfiguracoes();
    }

    void SetFirst(GameObject pos)
    {
        EventSystem.current.SetSelectedGameObject(pos);
    }

    void BaseGoBack()
    {
        int index = 0;
        foreach (GameObject c in uiPanels)
        {
            if (c.activeSelf)
            {
                SetFirst(gridButtons[index]);
                break;
            }
            index++;
        }
    }

    public void CloseSettings()
    {
        if (pauseMenu != null)
            pauseMenu.OpenFromSettings();

        if (menuScript != null)
            menuScript.CloseSettings();

        gameObject.SetActive(false);
    }

    public void FirstEnter()
    {
        foreach (GameObject current in uiPanels)
        {
            current.SetActive(false);
        }
    }
}
