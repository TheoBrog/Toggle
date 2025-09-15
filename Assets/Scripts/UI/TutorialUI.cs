using TMPro;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void ShowCard(Sprite img)
    {
        anim.Play("FadeIn");
        spriteRenderer.sprite = img;
    }

    public void HideCard()
    {
        anim.Play("FadeOut");
    }
}
