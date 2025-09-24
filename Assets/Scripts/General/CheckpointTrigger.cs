
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CheckpointTrigger : MonoBehaviour
{
    [HideInInspector] public int side;
    public ParticleSystem checkpointParticle;

    void Start()
    {
        GameObject probablySide = transform.parent.parent.gameObject;
        if (probablySide.GetComponent<SideSettings>())
            side = probablySide.GetComponent<SideSettings>().sideIndex;
        else
            Debug.LogWarning("No Side found! Please fix the parenting of " + transform.name);
    }

    void OnTriggerEnter(Collider col)
    {
        GameObject obj = col.gameObject;
        if (obj.CompareTag("Player"))
        {
            DeathSystem deathSystem = obj.GetComponent<DeathSystem>();
            deathSystem.NewCheckpoint(transform);
            if (GetComponent<Renderer>().enabled)
            {
                checkpointParticle.Play();
                GetComponent<Renderer>().enabled = false;
            }
        }
    }

    void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
    }
}