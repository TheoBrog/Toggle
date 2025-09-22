using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ScreenFade : MonoBehaviour
{
    public static ScreenFade instance;

    public bool startFade;
    Animator anim;

    void Awake()
    {
        instance = this;
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        if (startFade)
            FadeIn();
    }

    public void FadeIn()
    {
        anim.Play("FadeIn", -1, 0);
    }

    public void FadeOut()
    {
        anim.Play("FadeOut", -1, 0);
    }

    void EndLevel()
    {
        FadeOut();
    }

    void OnEnable()
    {
        GameManager.endLevel += EndLevel;
    }

    void OnDisable()
    {
        GameManager.endLevel -= EndLevel;        
    }
}