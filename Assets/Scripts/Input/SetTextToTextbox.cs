using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class SetTextToTextbox : MonoBehaviour
{
    [TextArea(2, 3)]
    [SerializeField] string message = "Press BUTTONPROMPT to interact.";

    [Header("Setup for Sprites")]
    [SerializeField] ListOfTmpAssets listOfTmpAssets;
    [SerializeField] DeviceType deviceType;

    [SerializeField] PlayerInput playerInput;
    TMP_Text textBox;

    void Awake()
    {
        // playerInput = new PlayerInput();
        textBox = GetComponent<TMP_Text>();
    }

    void Start()
    {
        SetText();
    }

    [ContextMenu("Set Text")]
    void SetText()
    {
        if ((int)deviceType > listOfTmpAssets.SpriteAssets.Count - 1)
        {
            Debug.Log($"Missing Sprite Asset for {deviceType}");
            return;
        }

        // Debug.Log(playerInput.actions["Jump"].bindings[0].ToString());

        textBox.text = CompleteTextWithButtonPromptSprite.ReadAndReplaceBinding(
            message,
            playerInput.actions["Jump"].bindings[(int)deviceType],
            listOfTmpAssets.SpriteAssets[(int)deviceType]
            // playerInput.actions.FindActionMap("Ingame").FindAction("Jump").bindings[(int)deviceType]
        );
    }

    enum DeviceType
    {
        Keyboard = 0,
        Gamepad = 1
    }
}
