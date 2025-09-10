using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ToggleScript : MonoBehaviour
{
    [Header("Sides")]
    public Transform[] sides;
    public List<Color> sideColors = new();
    float distance;

    public int currentSide;
    public LayerMask groundMask;

    [Header("Time")]
    public float transitionTime;
    [HideInInspector] public float toggleDelay;
    public float maxToggleDelay;

    bool inTransition = false;
    bool canToggle = true;

    void Awake()
    {
        // Se houver menos que dois lados não da pra trocar
        if (sides.Length < 2)
        {
            canToggle = false;
            return;
        }

        currentSide = 0;
        distance = sides[1].position.z;

        int index = 0;
        foreach (Transform obj in sides)
        {
            // Arrumar todas as posições no primeiro frame
            obj.position = new Vector3(obj.position.x, obj.position.y, distance * index);

            // Atribuir valores em cada SideSetting
            SideSettings sideSettings = obj.GetComponent<SideSettings>();
            sideSettings.sideIndex = index;
            sideColors.Add(sideSettings.color);

            index++;
        }
    }

    void Update()
    {
        // Não detectar Inputs quando pausado
        if (PauseMenu.isPaused)
            return;

        // Pode trocar se apertar o botão certo e poder trocar
        if (UserInput.instance.TogglePress && canToggle)
        {
            Toggle();
        }

        if (toggleDelay < maxToggleDelay && !inTransition)
        {
            toggleDelay += Time.deltaTime;
        }
    }

    void Toggle()
    {
        // Detectar uma parede atrás do player
        if (Physics.Raycast(transform.position, Vector3.forward, distance + 1, groundMask))
            return;

        // Executar Toggle se não havia parede
        StartCoroutine(ToggleAnimation());
    }

    IEnumerator ToggleAnimation()
    {
        // Parar o tempo
        Time.timeScale = 0;

        // Mudando estados
        canToggle = false;
        inTransition = true;
        toggleDelay = 0;

        // Principal animação dos lados
        int index = 0;
        foreach (Transform side in sides)
        {
            if (index != currentSide)
            {
                Vector3 vector3 = side.transform.position + new Vector3(0, 0, -distance);
                side.transform.DOMove(vector3, transitionTime).SetUpdate(true);
            }
            else
            {
                Vector3 vector3 = new Vector3(side.transform.position.x, side.transform.position.y, distance * sides.Length - distance);
                side.transform.DOMove(vector3, transitionTime).SetUpdate(true);
            }

            index++;
        }
        inTransition = false;

        // Muda o lado atual
        currentSide++;
        if (currentSide > sides.Length - 1)
            currentSide = 0;

        // Chamar Action de onToggle
        GameManager.onToggle?.Invoke();

        // Já que o Tweening não para o código isso aqui para
        yield return new WaitForSecondsRealtime(transitionTime);

        // Se o jogo for pausado no meio da animação ele não continua
        if (!PauseMenu.isPaused)
            Time.timeScale = 1;

        // Delay de troca
        yield return new WaitForSecondsRealtime(maxToggleDelay);

        canToggle = true;
    }

    public void FindRightSideForCheckpoints()
    {
        DeathSystem deathSystem = GetComponent<DeathSystem>();
        if (deathSystem.checkpointSide != currentSide)
        {
            while (deathSystem.checkpointSide != currentSide)
            {
                ToggleInstant();
            }
        }
    }

    void ToggleInstant()
    {
        // Principal animação dos lados
        int index = 0;
        foreach (Transform side in sides)
        {
            if (index != currentSide)
            {
                Vector3 vector3 = side.transform.position + new Vector3(0, 0, -distance);
                side.transform.DOMove(vector3, 0).SetUpdate(true);
            }
            else
            {
                Vector3 vector3 = new Vector3(side.transform.position.x, side.transform.position.y, distance * sides.Length - distance);
                side.transform.DOMove(vector3, 0).SetUpdate(true);
            }

            index++;
        }
        inTransition = false;

        // Muda o lado atual
        currentSide++;
        if (currentSide > sides.Length - 1)
            currentSide = 0;
    }
}
