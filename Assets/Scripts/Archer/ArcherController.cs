using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherController : MonoBehaviour
{
    private float movementInputDirection;
    private float jumpTimer;
    private float turnTimer;
    [SerializeField]

    private int amountOfJumpsLeft;
    private int facingDirection = 1;

    private bool isFacingRight = true;
    private bool isWalking;
   [SerializeField] private bool isGrounded;
    private bool isTouchingWall;
    private bool canNormalJump;
    private bool isAttemptingToJump;
    private bool checkJumpMultiplier;
    private bool canMove;
    private bool canFlip;
    public float offset;
    public GameObject projectile;
    public Transform shotPoint;

    private Animator anim;
    private float timeBetweenAttacks = 0.5f; // Time between each attack
    private float timeSinceLastAttack = 0.0f;
    private Rigidbody2D rb;

    [SerializeField]
    public int amountOfJumps = 1;

    public float movementSpeed = 10.0f;
    public float jumpForce;
    public float groundCheckRadius;
    public float movementForceInAir;
    public float airDragMultiplier = 0.95f;
    public float variableJumpHeightMultiplier = 0.5f;
    public float jumpTimerSet = 0.15f;
    public float turnTimerSet = 0.1f;

    public float maxHealth;
    [SerializeField]
    private GameObject
        deathChunkParticle,
        deathBloodParticle;

    public float currentHealth;
    private GameManager GM;
    public Transform groundCheck;
    private ArcherController AC;
    private PlayerStats PS;
    public LayerMask whatIsGround;



    private AttackDetails attackDetails;



     private void Damage(AttackDetails attackDetails)
    {
        if (PS == null)
        {
            PS = GetComponent<PlayerStats>();
        }

        PS.DecreaseHealth(10);

    }
    public int GetFacingDirection()
    {
        return facingDirection;
    }
    private void CheckInput()
    {
        movementInputDirection = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded || (amountOfJumpsLeft > 0 && !isTouchingWall))
            {
                NormalJump();
            }
            else
            {
                jumpTimer = jumpTimerSet;
                isAttemptingToJump = true;
            }
        }

        if (Input.GetButtonDown("Horizontal") && isTouchingWall)
        {
            if (!isGrounded && movementInputDirection != facingDirection)
            {
                canMove = false;
                canFlip = false;

                turnTimer = turnTimerSet;
            }
        }

        if (turnTimer >= 0)
        {
            turnTimer -= Time.deltaTime;

            if (turnTimer <= 0)
            {
                canMove = true;
                canFlip = true;
            }
        }

        if (checkJumpMultiplier && !Input.GetButton("Jump"))
        {
            checkJumpMultiplier = false;
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * variableJumpHeightMultiplier);
        }

    }
    private void CheckMovementDirection()
    {
        if (isFacingRight && movementInputDirection < 0)
        {
            Flip();
        }
        else if (!isFacingRight && movementInputDirection > 0)
        {
            Flip();
        }

        if (Mathf.Abs(rb.velocity.x) >= 0.01f)
        {
            isWalking = true;
        }
        else
        {
            isWalking = false;
        }
    }
    private void CheckIfCanJump()
    {
        if (isGrounded && rb.velocity.y <= 0.01f)
        {
            amountOfJumpsLeft = amountOfJumps;
        }

        if (amountOfJumpsLeft <= 0)
        {
            canNormalJump = false;
        }
        else
        {
            canNormalJump = true;
        }

    }
    private void CheckSurroundings()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
    }
    private void NormalJump()
    {
        if (canNormalJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            amountOfJumpsLeft--;
            jumpTimer = 0;
            isAttemptingToJump = false;
            checkJumpMultiplier = true;
        }
    }
    private void CheckJump()
    {
        if (jumpTimer > 0)
        {
            if (isGrounded)
            {
                NormalJump();
                amountOfJumpsLeft = amountOfJumps;
            }
        }

        if (isAttemptingToJump)
        {
            jumpTimer -= Time.deltaTime;

        }
    }
    private void ApplyMovement()
    {

        if (!isGrounded && movementInputDirection == 0)
        {
            rb.velocity = new Vector2(rb.velocity.x * airDragMultiplier, rb.velocity.y);
        }
        else if (canMove)
        {
            rb.velocity = new Vector2(movementSpeed * movementInputDirection, rb.velocity.y);
        }
    }
    public void DisableFlip()
    {
        canFlip = false;
    }

    public void EnableFlip()
    {
        canFlip = true;
    }
    private void Flip()
    {
        if (canFlip)
        {
            facingDirection *= -1;
            isFacingRight = !isFacingRight;
            transform.Rotate(0.0f, 180.0f, 0.0f);
        }
    }
    private IEnumerator SpawnProjectileAfterDelay()
    {
        yield return new WaitForSeconds(0.5f); // Delay before spawning the projectile

        Instantiate(projectile, shotPoint.position, transform.rotation);
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        amountOfJumpsLeft = amountOfJumps;
        AC = GetComponent<ArcherController>();
        PS = GetComponent<PlayerStats>();
        currentHealth = maxHealth;
        //GM = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    private void UpdateAnimations()
    {
        anim.SetBool("isRunning", isWalking);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("yVelocity", rb.velocity.y);
    }
    private void Update()
    {
        CheckInput();
        CheckMovementDirection();
        UpdateAnimations();
        CheckIfCanJump();
        CheckJump();
        if (Input.GetMouseButton(0) && !isWalking && isGrounded && timeSinceLastAttack >= timeBetweenAttacks)
        {
            timeSinceLastAttack = 0.0f;
            anim.SetBool("isAttacking", true);
            StartCoroutine(SpawnProjectileAfterDelay());
        }
        else
        {
            timeSinceLastAttack += Time.deltaTime;
            anim.SetBool("isAttacking", false);
        }

    }

    private void FixedUpdate()
    {
        ApplyMovement();
        CheckSurroundings();
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
