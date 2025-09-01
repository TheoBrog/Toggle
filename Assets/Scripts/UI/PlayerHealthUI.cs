using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    GameObject player;
    PlayerMovement playerMovement;
    ToggleScript toggleScript;

    int playerHealth;
    int lastPlayerHealth;

    public List<GameObject> hearts = new();

    public GameObject heartObject;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = player.GetComponent<PlayerMovement>();
        toggleScript = player.GetComponent<ToggleScript>();

        playerHealth = playerMovement.health;
        //lastPlayerHealth = playerHealth;

        HealthUpdate();

        StartCoroutine(Loop());
    }

    IEnumerator Loop()
    {
        while (true)
        {
            playerHealth = playerMovement.health;
            if (playerHealth != lastPlayerHealth)
                HealthUpdate();
            lastPlayerHealth = playerHealth;
            yield return new WaitForSeconds(0.1f);
        }
    }

    void HealthUpdate()
    {
        ClearHearts();

        int i = 0;
        for (i = 0; i < playerHealth; i++)
        {
            CreateHeart(i, i + 1 > lastPlayerHealth, "HealthSpawn", true, false);
        }
        if (lastPlayerHealth > playerHealth)
            CreateHeart(i, true, "HealthDestroy", false, true);

        UpdateHeartColors();
    }

    void CreateHeart(int i, bool playAnim, string animString, bool add, bool delete)
    {
        GameObject obj = Instantiate(heartObject);
        obj.transform.SetParent(transform);

        RectTransform rt = obj.transform.GetComponent<RectTransform>();
        rt.transform.localPosition = Vector3.zero;
        rt.transform.localPosition += new Vector3(i * 50, 0, 0);
        rt.transform.localScale = new Vector3(1, 1, 1);

        Animator anim = obj.GetComponent<Animator>();
        if (anim != null && playAnim)
            anim.Play(animString);

        // Debug.Log("i: " + i + " // lpr: " + lastPlayerHealth);

        if (add)
        {
            hearts.Add(obj);
            UpdateHeartColors();
        }
        if (delete)
        {
            Destroy(obj, .1f);
            if (obj != null)
            {
                obj.GetComponent<Image>().color = toggleScript.sideColors[toggleScript.currentSide];
            }
        }

    }

    void ClearHearts()
    {
        foreach (GameObject obj in hearts)
        {
            Destroy(obj);
        }
    }

    void OnEnable()
    {
        GameManager.onToggle += UpdateHeartColors;
        GameManager.onDeath += UpdateHeartColors;
    }

    void UpdateHeartColors()
    {
        foreach (GameObject obj in hearts)
        {
            if (obj != null)
            {
                Image objImage = obj.GetComponent<Image>();
                objImage.color = toggleScript.sideColors[toggleScript.currentSide];
            }
        }
    }
}
