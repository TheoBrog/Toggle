using Unity.VisualScripting;
using UnityEngine;

public class EnemyLeftAndRight : EnemyBase
{
    [Header("Enemy Movement")]
    public float walkspeed = 1; // Enemy walk speed
    public float runSpeed = 1; // Enemy run speed
    float speed;
    int direction = 1;

    public float height;

    public float wallDetectionRaycast = 1.5f;

    public bool avoidFalling = true;
    public Transform floorDetectionObject;

    SpriteRenderer spriteRenderer;

    protected override void Start()
    {
        base.Start();

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    protected void Patrol()
    {
        // Left and Right Movement
        rb.linearVelocity = new Vector3(direction * speed, rb.linearVelocity.y, 0f);
        speed = walkspeed;

        CheckWalls();
        CheckFalls();
    }

    #region CHECKING THINGS
    void CheckWalls()
    {
        if (Physics.Raycast(transform.position, Vector3.right * direction, wallDetectionRaycast))
            ChangeDirection();
    }

    void CheckFalls()
    {
        if (!avoidFalling && floorDetectionObject != null) return;

        if (!Physics.Raycast(floorDetectionObject.position, Vector3.down, 0.25f) && Physics.Raycast(transform.position, Vector3.down, height))
            ChangeDirection();
    }

    void ChangeDirection()
    {
        direction *= -1;
        Vector3 fdo = floorDetectionObject.transform.localPosition;
        fdo.x *= -1;
        floorDetectionObject.transform.localPosition = fdo;
        if (spriteRenderer != null)
            spriteRenderer.flipX = direction < 0;
    }
    #endregion

    protected void Chase()
    {
        float dir = Mathf.Sign(player.transform.position.x - transform.position.x);
        rb.linearVelocity = new Vector3(dir * speed, rb.linearVelocity.y, 0f);
        speed = runSpeed;
        LookAtPlayer();
    }

    protected void LookAtPlayer()
    {
        if (spriteRenderer == null) return;
        spriteRenderer.flipX = transform.position.x > player.transform.position.x;
    }

    void Reset()
    {
        GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        obj.name = "Floor Detection";
        obj.transform.parent = transform;
        obj.transform.localPosition = Vector3.zero;
        DestroyImmediate(obj.GetComponent<BoxCollider>());
        DestroyImmediate(obj.GetComponent<MeshFilter>());
        DestroyImmediate(obj.GetComponent<MeshRenderer>());
        floorDetectionObject = obj.transform;

        transform.AddComponent<DamageFlash>();
    }
}