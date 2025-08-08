using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerController))]
public class PlayerMovement : MonoBehaviour
{
    //Referencias de componentes
    private Rigidbody2D rb; 
    private PlayerController playerController;

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
    private bool jumpPressed;
    private bool jumpHeld;

    private bool isFacingRight = true;
    private float coyoteTimeCounter;
    private float jumpBufferTimeCounter;
    private bool isAttacking = false;
    private PlayerController controller;

    //Propriedades públicas 
    public bool IsGrounded {  get; private set; }
    public Vector2 RawInputDirection { get; private set; }
    public Vector2 FacingDirection => isFacingRight ? Vector2.right : Vector2.left;

    public void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        controller = GetComponent<PlayerController>();
    }
    public void HandleInputReading()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        RawInputDirection = new Vector2(horizontalInput, Input.GetAxisRaw("Vertical")).normalized;

        // Buffer de pulo (pressionou pulo)
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space))
        {
            jumpBufferTimeCounter = jumpBufferTime;
        }
    }
    private void Update()
    {
        HandleTimers();
        CheckIfGrounded();
        FlipCharacter();
    }
    private void FixedUpdate()
    {
        HandleMovementInput();
        ExecuteJump();
        HandleGravity();
    }

    public void ExecuteJump()
    {
        if ((Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.Space)) && rb.linearVelocity.y > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * JumpCutMultiplier);
        }
    }
    public void HandleJumpInput()
    {
        if (coyoteTimeCounter > 0f && jumpBufferTimeCounter > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, JumpForce);
            jumpBufferTimeCounter = 0f;
            coyoteTimeCounter = 0f;
            controller.ChangeState(EPlayerState.Jumping);
        }
    }

    private void HandleTimers()
    {
        coyoteTimeCounter = IsGrounded ? coyoteTime : coyoteTimeCounter - Time.deltaTime;
        jumpBufferTimeCounter -= Time.deltaTime;
    }

    private void CheckIfGrounded()
    {
        bool WasGrounded = IsGrounded;
        RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down, rayDistance, groundLayer);
        IsGrounded = hit.collider != null;

        if (!WasGrounded && IsGrounded)
        {
            controller.OnGrounded();
        }
    }

    public void HandleMovementInput()
    {
        float currentSpeed = isAttacking ? attackingMoveSpeed : moveSpeed;
        rb.linearVelocity = new Vector2(horizontalInput * currentSpeed, rb.linearVelocity.y);

        if(controller.CurrentState != EPlayerState.Jumping && controller.CurrentState != EPlayerState.Falling && IsGrounded)
        {
            if (Mathf.Abs(horizontalInput) > 0.1f)
            {   
                controller.ChangeState(EPlayerState.Moving);
            }
            else
            {
                controller.ChangeState(EPlayerState.Idle);
            }
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

    public void SetAttackingMovement(bool attacking)
    {
        isAttacking = attacking;
    }
}