using UnityEngine;

public class LeverScript : ObjectInteraction
{
    [Header("Lever")]
    public SpriteRenderer stateRenderer;
    public Sprite[] stateSprites;

    public bool currentState;
    bool debounce = true;

    Animator anim;

    [Header("Door")]
    public Animator doorAnim;

    protected override void Start()
    {
        base.Start();

        anim = GetComponent<Animator>();
    }

    protected override void OnAttack()
    {
        base.OnAttack();

        if (!debounce)
            return;

        // Debug.Log("activate lever");

        debounce = false;
        if (!currentState)
        {
            currentState = true;
            anim.Play("on");
            stateRenderer.sprite = stateSprites[1];
            doorAnim.Play("Open");
            // doorAnim.gameObject.transform.GetChild(2).GetComponent<SpriteRenderer>().sprite = ;
        }
        else
        {
            currentState = false;
            anim.Play("off");
            stateRenderer.sprite = stateSprites[0];
            doorAnim.Play("Close");
        }
    }

    public void ToggleDebounce()
    {
        debounce = true;
    }
}
