using UnityEngine;
using UnityEngine.Events;

public class TriggerScript : MonoBehaviour
{
    public UnityEvent[] onEnterEvents;
    public UnityEvent[] onExitEvents;

    public bool singleEnter;
    public bool singleExit;

    bool hasEntered;
    bool hasExited;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player") && !hasEntered)
        {
            foreach (UnityEvent e in onEnterEvents)
            {
                e.Invoke();
            }

            if (singleEnter)
                hasEntered = true;
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("Player") && !hasExited)
        {
            foreach (UnityEvent e in onExitEvents)
            {
                e.Invoke();
            }

            if (singleExit)
                hasExited = true;
        }
    }

    void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
    }
}
