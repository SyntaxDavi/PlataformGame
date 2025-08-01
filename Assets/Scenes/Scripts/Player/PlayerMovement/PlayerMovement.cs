using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    //Referencias de componentes
    private Rigidbody2D rb;

    //Configurações de Movimento
    [Header("Movimento")]
    [Tooltip("Velocidade base do jogador.")]
    [SerializeField] private float moveSpeed = 8f;
    [Tooltip("Velocidade do jogador enquanto ataca")]
    [SerializeField] private float attackingMoveSpeed = 4f;

    [Header("Pulo")]
    [Tooltip("Força inicial aplicada ao pular.")]
    [SerializeField] private float JumpForce = 10f;
    [Tooltip("Multiplicador da gravidade ao cair para uma queda mais rápida")]
    [SerializeField] private float FallGravityMultiplier = 2.5f;
    [Tooltip("Multiplicador aplicado à velocidade vertical ao cortar o pulo (soltando o botão).")]
    [SerializeField] private float JumpCutMultiplier = 0.5f;

    [Header("Verificação de Chão(Ground check)")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float rayDistance = 1f;

    //Game Feel
    [Header("Game Feel")]
    [Tooltip("Permite pular por um curto período após sair de uma plataforma (tempo de 'coiote').")]
    [SerializeField] private float coyoteTime = 0.2f;
    [Tooltip("Permite que o comando de pulo seja 'lembrado' se for pressionado um pouco antes de tocar o chão.")]
    [SerializeField] private float jumpBufferTime = 0.2f;

    //Variaves de estado (privadas)

    private float horizontalInput;
    private float verticalInput;
    private bool isFacingRight = true;
    private float coyoteTimeCounter;
    private float jumpBufferTimeCounter;
    private bool isAttacking = false;
    private bool hasJumped = false;

    //Propriedades públicas 
    public bool IsGrounded {  get; private set; }
    public Vector2 RawInputDirection { get; private set; }
    public Vector2 FacingDirection => isFacingRight ? Vector2.right : Vector2.left;

    public void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        HandleInput();
        HandleTimers();
        CheckIfGrounded();
        FlipCharacter();
    }
    private void FixedUpdate()
    {
        HandleGravity();
        HandleJump();
        HandleMovement();
    }
    private void HandleInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        RawInputDirection = new Vector2(horizontalInput, verticalInput).normalized;

        // Buffer de pulo (pressionou pulo)
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space))
        {
            jumpBufferTimeCounter = jumpBufferTime;
        }

        // Cortar pulo (soltou pulo)
        if ((Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.Space)) && rb.linearVelocity.y > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * JumpCutMultiplier);
        }
    }

    private void HandleTimers()
    {
        if (IsGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        jumpBufferTimeCounter -= Time.deltaTime;
    }

    private void CheckIfGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down, rayDistance, groundLayer);

        //bool isOnWall = Physics2D.Raycast(transform.position, Vector2.right * direction, wallCheckDistance, groundLayer);

        IsGrounded = hit.collider != null;

        if (IsGrounded)
        {
            hasJumped = false;
        }
    }

    private void HandleMovement()
    {
        float currentSpeed = isAttacking ? attackingMoveSpeed : moveSpeed;
        rb.linearVelocity = new Vector2(horizontalInput * currentSpeed, rb.linearVelocity.y);
    }
    private void HandleJump()
    {
        if(jumpBufferTimeCounter > 0f && coyoteTimeCounter > 0f && !hasJumped)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, JumpForce);
            jumpBufferTimeCounter = 0f;
            hasJumped = true;
        }
    }

    private void HandleGravity()
    {
        if(rb.linearVelocity.y < 0)
        {
            rb.gravityScale = FallGravityMultiplier;
        }
        else
        {
            rb.gravityScale = 1f;
        }
    }

    private void FlipCharacter()
    {
        if((isFacingRight && horizontalInput < 0) || (!isFacingRight && horizontalInput > 0))
        {
            isFacingRight = !isFacingRight;
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }
    }

    public void SetAttackingState(bool attacking)
    {
        isAttacking = attacking;
    }

    private void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(groundCheck.position, groundCheck.position + Vector3.down * 0.1f);
        }
    }
}