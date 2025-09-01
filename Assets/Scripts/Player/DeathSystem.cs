using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathSystem : MonoBehaviour
{
    public Transform currentCheckpoint;
    public int checkpointSide;

    PlayerMovement playerMovement;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    public void Die()
    {
        StartCoroutine(DieCoroutine());
    }

    IEnumerator DieCoroutine()
    {
        playerMovement.health = playerMovement.maxHealth;
        GetComponent<ToggleScript>().FindRightSideForCheckpoints();
        yield return null;
        if (currentCheckpoint != null)
            transform.position = currentCheckpoint.position;
        else
            SceneManager.LoadScene(1);
        GameManager.instance.ResetGame();
    }

    public void NewCheckpoint(Transform touched)
    {
        if (currentCheckpoint == null || touched.position.x > currentCheckpoint.position.x)
        {
            // Debug.Log("Checkpoint reached");
            currentCheckpoint = touched;
            checkpointSide = currentCheckpoint.GetComponent<CheckpointTrigger>().side;
        }
    }
}
