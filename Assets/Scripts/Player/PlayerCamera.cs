using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public float cameraOffset;
    public float cameraSmoothSpeed;

    PlayerMovement playerMovement;
    Transform playerTransform;

    float cameraY;
    float cameraZ;

    bool cameraStatic;
    Vector3 positionStatic;

    float velocityX;

    void Start()
    {
        cameraY = transform.position.y;
        cameraZ = transform.position.z;

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        playerMovement = playerTransform.gameObject.GetComponent<PlayerMovement>();
    }

    void Update()
    {
        CameraControl();
    }

    void CameraControl()
    {
        if (!cameraStatic)
        {
            float targetX = (cameraOffset * playerMovement.playerDirection) + playerTransform.position.x;

            float smoothX = Mathf.SmoothDamp(
                transform.position.x,
                targetX,
                ref velocityX,
                1f / cameraSmoothSpeed // menor valor = resposta mais rápida
            );

            transform.position = new Vector3(smoothX, cameraY, cameraZ);
        }
        else
        {
            
        }
    }

    public void StaticCamera(Transform position)
    {

    }

    public void UnStaticCamera()
    {
        
    }
}
