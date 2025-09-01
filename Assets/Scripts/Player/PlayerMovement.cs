using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using FirstGearGames.SmoothCameraShaker;

public enum PlayerState
{
    idle,
    attacking,
    dashing
}

public class PlayerMovement :    MonoBehaviour
{
    [Header("Player")]
    public int maxHealth = 100;
    public int health = 100;
    float invincibilityTimer;

    Animator anim;
    string currentAnimation;
    bool pauseAnim;

    public PlayerSounds playerSounds;

    public PlayerState state;
    public bool canMove = true;
    public bool canFall = true;


    [Header("Player UI")]
    public Image toggleDelayUI;
    public Image dashDelayUI;


    [Header("Player Input")]
    Vector2 moveInput;


    [Header("Player Camera")]
    public GameObject mainCamera;

    public float cameraOffset;
    public float cameraSmoothSpeed;
    public float cameraY;


    [Header("Player Components")]
    Rigidbody rig;
    ToggleScript toggleScript;

    SpriteRenderer spriteRenderer;
    BoxCollider boxCollider;
    DamageFlash damageFlash;


    [Header("Horizontal Movement")]
    public float horizontalSpeed;
    float horizontalAxis;
    float playerDirection = 1;

    public float dashPower;
    public float dashLength;
    float stamina;


    [Header("Vertical Movement")]
    public float jumpPower;
    public float maxJump;
    float jumpTimer;

    public float maxCoyoteTimer;
    float coyoteTimer;

    public float gravity;
    float spdY;


    [Header("Head Bump")]
    public Transform footPos;
    public LayerMask groundMask;
    public Vector3 boxShape = new Vector3(1, .1f, 1f);

    public Transform headPosition;
    public float maxHeadDistance;


    [Header("Attacking")]
    public GameObject attackObject;
    float attackAngle;


    [Header("Parry")]
    public ShakeData parryShakeData;
    public GameObject parryFlash;

    void Start()
    {
        rig = GetComponent<Rigidbody>();
        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider>();
        anim = GetComponent<Animator>();
        damageFlash = GetComponent<DamageFlash>();

        toggleScript = GetComponent<ToggleScript>();

        health = maxHealth;
        stamina = 1;

        mainCamera = Camera.main.gameObject;
        // fazer camera ser parent
        mainCamera.transform.parent = transform;

        //QualitySettings.vSyncCount = 0;
        //Application.targetFrameRate = 10;
    }

    #region LOOPS
    void Update()
    {
        if (PauseMenu.isPaused)
            return;

        moveInput = UserInput.instance.MoveInput;
        HorizontalMovement();
        VerticalMovement();

        AttackInput();
        Dash();
        CameraControl();

        PlayerUI();
        AnimationFunction();

        if (invincibilityTimer > 0)
            invincibilityTimer -= Time.deltaTime;

        // teste resetar posição
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(1);
        }
    }

    void FixedUpdate()
    {
        if (PauseMenu.isPaused)
            return;

        // velocidade horizontal
        if (canMove)
        {
            Vector3 velocity = new Vector3(horizontalAxis * horizontalSpeed, rig.linearVelocity.y, rig.linearVelocity.z);
            rig.linearVelocity = velocity;
        }

        // velocidade vertical
        if (canFall)
        {
            spdY -= gravity;
            rig.linearVelocity = new Vector3(rig.linearVelocity.x, spdY, rig.linearVelocity.z);
        }
    }
    #endregion

    #region MOVEMENT
    void HorizontalMovement()
    {
        if (!canMove)
            return;

        horizontalAxis = Mathf.Round(moveInput.x);

        // filtro para evitar valores pequenos do analógico
        if (Mathf.Abs(horizontalAxis) > 0.2f)
        {
            playerDirection = Mathf.Sign(horizontalAxis);
            spriteRenderer.flipX = playerDirection < 0;
        }
        else
        {
            horizontalAxis = 0f; // zera input para evitar deslize mínimo
        }
    }

    void VerticalMovement()
    {
        // executa no chão
        if (isGrounded())
        {
            spdY = 0f;
            coyoteTimer = 0f;
        }
        else
            coyoteTimer += Time.deltaTime;

        Jump();
    }

    void Jump()
    {
        if (!canFall)
            return;

        // se apertar espaço + coyote time
        if (UserInput.instance.JumpPress && coyoteTimer < maxCoyoteTimer)
        {
            if (spdY > 0)
                return;
            jumpTimer = maxJump;
            coyoteTimer = maxCoyoteTimer;
        }
        // executar enquanto espaço estiver pressionado
        if (UserInput.instance.JumpHold && jumpTimer > 0f)
        {
            spdY = jumpPower;
            jumpTimer -= Time.deltaTime;
        }
        // quando soltar espaço
        if (UserInput.instance.JumpRelease)
        {
            // se estiver caindo não mudar velocidade vertical
            if (spdY > 0f)
            {
                spdY = 0f;
            }
            jumpTimer = 0;
        }

        // detectar se bateu a cabeça
        if (Physics.Raycast(headPosition.position, Vector3.up, maxHeadDistance, groundMask) && spdY > 0)
        {
            spdY = 0;
            jumpTimer = 0;
        }
    }
    #endregion

    #region ATTACK
    void AttackInput()
    {
        if (UserInput.instance.AttackPress && state != PlayerState.attacking)
        {
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack()
    {
        state = PlayerState.attacking;

        SoundManager.instance.Play(playerSounds.swingAttack);

        //get direction
        float horizontal = moveInput.x;
        float vertical = moveInput.y;

        Vector2 dir = new Vector2(horizontal, vertical);
        if (dir != Vector2.zero)
            attackAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        else
        {
            attackAngle = playerDirection * 90;
            attackAngle -= 90;
        }

        attackObject.transform.rotation = Quaternion.Euler(0f, 0f, attackAngle);

        attackObject.transform.localPosition = Vector3.zero;
        attackObject.transform.localPosition += attackObject.transform.right * 1.25f;

        attackObject.SetActive(true);
        attackObject.GetComponent<Animator>().Play("SlashAnim");
        yield return new WaitForSeconds(.1f);

        attackObject.SetActive(false);
        yield return new WaitForSeconds(.1f);

        state = PlayerState.idle;
    }
    #endregion

    #region DASH
    void Dash()
    {
        if (UserInput.instance.DashPress && stamina >= 1)
        {
            StartCoroutine(DashEnumerator());
        }

        if (stamina < 1)
            stamina += Time.deltaTime;
    }

    IEnumerator DashEnumerator()
    {
        stamina -= 1;
        jumpTimer = 0;

        canMove = false;
        canFall = false;
        state = PlayerState.dashing;

        pauseAnim = true;
        ChangeAnimation("DashAnim");
        SoundManager.instance.Play(playerSounds.dashSound, 1, Random.Range(.8f, 1.2f));

        spdY = 0;

        boxCollider.isTrigger = true;

        bool parried = false;

        // misericórdia
        for (int i = 0; i < dashLength; i++)
        {
            if (Physics.Raycast(transform.position, Vector3.right * playerDirection, 1f, groundMask))
                break;

            Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1f);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.gameObject.GetComponent<BulletScript>())
                {
                    GameObject obj = hitCollider.gameObject;

                    BulletScript b = obj.GetComponent<BulletScript>();
                    b.Parry();
                    if (b.parryable)
                    {
                        spdY = 15;
                        parried = true;
                        stamina = 1;
    
                        GameObject pf = Instantiate(parryFlash);
                        pf.transform.SetParent(null);
                        pf.transform.localPosition = transform.localPosition;
                        pf.transform.localEulerAngles = Vector3.zero;

                        SoundManager.instance.Play(playerSounds.parrySound, .5f, Random.Range(.9f, 1.1f));

                        CameraShakerHandler.Shake(parryShakeData);

                        break;
                    }
                }
            }

            Vector3 velocity = new Vector3(playerDirection * dashPower, 0, rig.linearVelocity.z);
            rig.linearVelocity = velocity;

            if (!parried)
                yield return new WaitForFixedUpdate();
        }

        boxCollider.isTrigger = false;

        canMove = true;
        canFall = true;
        pauseAnim = false;

        if (parried)
            yield return new WaitForSeconds(0.25f);
        state = PlayerState.idle;
    }
    #endregion

    #region AESTHETICS
    void PlayerUI()
    {
        toggleDelayUI.fillAmount = toggleScript.toggleDelay / toggleScript.maxToggleDelay;
        dashDelayUI.fillAmount = stamina;
    }

    void PlayerUIColorUpdate()
    {
        toggleDelayUI.color = toggleScript.sideColors[toggleScript.currentSide];
    }

    void ChangeAnimation(string animation)
    {
        if (currentAnimation != animation)
        {
            currentAnimation = animation;
            anim.Play(animation);
        }
    }

    void AnimationFunction()
    {
        if (pauseAnim == true)
            return;

        if (spdY > 0)
        {
            ChangeAnimation("JumpAnim");
            return;
        }
        else if (spdY < 0)
        {
            ChangeAnimation("FallAnim");
            return;
        }

        if (spdY != 0 || coyoteTimer > 0)
            return;

        if (horizontalAxis != 0)
            ChangeAnimation("WalkAnim");
        else
            ChangeAnimation("IdleAnim");
    }

    void CameraControl()
    {
        Vector3 target = new Vector3(cameraOffset * playerDirection, cameraY, -14);
        mainCamera.transform.localPosition = Vector3.Lerp(mainCamera.transform.localPosition, target, Time.deltaTime * cameraSmoothSpeed);
        // set camera Y
        mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, cameraY, -14);
    }
    #endregion

    #region DAMAGE & KNOCKBACK
    public void Damage(int damage, bool itm, float kbDir)
    {
        if (invincibilityTimer > 0 || state == PlayerState.dashing)
            return;

        health -= damage;
        StartCoroutine(Knockback(kbDir));
        if (itm)
            invincibilityTimer = 1f;
        else
            invincibilityTimer = 0.1f;
        damageFlash.CallDamageFlash();

        if (health <= 0)
        {
            DeathSystem deathSystem = GetComponent<DeathSystem>();
            deathSystem.Die();
        }
    }

    public IEnumerator Knockback(float knDir)
    {
        if (knDir != 0 && invincibilityTimer <= 0.01f)
        {
            StopCoroutine(DashEnumerator());
            canMove = false;
            invincibilityTimer = 1f;

            spdY = jumpPower * 1.1f;
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + 0.5f, transform.localPosition.z);
            rig.linearVelocity = new Vector3(horizontalSpeed * knDir, spdY, rig.linearVelocity.z);
            jumpTimer = 0;

            yield return new WaitForSeconds(0.25f);
            canMove = true;
        }
    }

    public IEnumerator AttackKnockback()
    {
        Vector2 direcao = new Vector2(Mathf.Cos(attackAngle * Mathf.Deg2Rad), Mathf.Sin(attackAngle * Mathf.Deg2Rad));
        StopCoroutine(DashEnumerator());
        canMove = false;
        canFall = false;

        // if (Mathf.Round(direcao.y) != 0)
        // {
            rig.linearVelocity = direcao * -7.5f;
            jumpTimer = 0;
            spdY = 0;
        // }

        yield return new WaitForSeconds(0.1f);
        canMove = true;
        canFall = true;
    }

    bool isGrounded()
    {
        //return Physics.Raycast(footPos.position, Vector3.down, 1f);
        return Physics.CheckBox(footPos.position, boxShape / 2, Quaternion.identity, groundMask);
    }
    #endregion

    void OnEnable()
    {
        GameManager.onToggle += PlayerUIColorUpdate;
        GameManager.onDeath += PlayerUIColorUpdate;
    }

    [System.Serializable]
    public class PlayerSounds
    {
        public AudioClip swingAttack;
        public AudioClip dashSound;
        public AudioClip parrySound;
    }
}