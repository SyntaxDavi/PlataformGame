using UnityEngine;
using System.Collections;

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
    [SerializeField] private Vector2 groundCheckSize = new Vector2(0.8f, 0.1f);
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
    private bool canMove = true;
    private Coroutine knockbackCoroutine;

    //Propriedades públicas 
    public bool HasJumpBuffer => jumpBufferTimeCounter > 0f;
    public bool CanUseCoyoteTime => coyoteTimeCounter > 0f;
    public bool IsGrounded {  get; private set; }
    public Vector2 RawInputDirection { get; private set; }
    public Vector2 FacingDirection => isFacingRight ? Vector2.right : Vector2.left;
    public Rigidbody2D Rigidbody => rb;

    public void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerController = GetComponent<PlayerController>();
    }
    public void HandleInputReading()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        RawInputDirection = new Vector2(horizontalInput, Input.GetAxisRaw("Vertical")).normalized;

        if (Input.GetButtonDown("Jump"))
        {
            jumpPressed = true;
        }

        jumpHeld = Input.GetButton("Jump");
    }
    private void Update()
    {
        HandleTimers();
        CheckIfGrounded();
        FlipCharacter();

        if (jumpPressed)
        {
            jumpBufferTimeCounter = jumpBufferTime;
            jumpPressed = false;
        }
    }
    private void FixedUpdate()
    {
        if (!canMove) { return; }
        HandleMovementPhysics();
        HandleJumpCut();
        HandleGravity();
    }
    public void ApplyKnockback(Vector2 direction, float force, float duration)
    {
        if(knockbackCoroutine != null)
        {
            StopCoroutine(knockbackCoroutine);
        }
        knockbackCoroutine = StartCoroutine(KnockbackCoroutine(direction, force, duration));
    }
    private void HandleMovementPhysics()
    {
        float currentSpeed = isAttacking ? attackingMoveSpeed : moveSpeed;
        rb.linearVelocity = new Vector2(horizontalInput * currentSpeed, rb.linearVelocity.y);
    }

    private IEnumerator KnockbackCoroutine(Vector2 direction, float force, float duration)
    {
        canMove = false;

        rb.linearVelocity = Vector2.zero;
        rb.AddForce(direction * force, ForceMode2D.Impulse);
        rb.gravityScale = 1.5f;

        yield return new WaitForSeconds(duration);

        canMove = true;
        knockbackCoroutine = null;
    }

    public void ExecuteJump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, JumpForce);

        jumpBufferTimeCounter = 0f;
        coyoteTimeCounter = 0f;
    }
    public void HandleJumpCut()
    {
      if(!jumpHeld && rb.linearVelocity.y > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * JumpCutMultiplier);
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
        RaycastHit2D hit = Physics2D.BoxCast(groundCheck.position, groundCheckSize,0f,Vector2.down, rayDistance, groundLayer);
        IsGrounded = hit.collider != null;

        if (!WasGrounded && IsGrounded)
        {
            playerController.OnGrounded();
        }
    }

    private void HandleGravity()
    {
       rb.gravityScale = (rb.linearVelocity.y < 0) ? FallGravityMultiplier : 1f;
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
    private void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            // Centro da caixa "esticada" no cast
            Vector3 boxCenter = (Vector2)groundCheck.position + Vector2.down * rayDistance * 0.5f;
            // Tamanho real (inclui a descida do rayDistance)
            Vector3 boxSize = new Vector3(groundCheckSize.x, groundCheckSize.y + rayDistance, 0f);

            // Desenha o volume do BoxCast
            Gizmos.DrawWireCube(boxCenter, boxSize);

            // Se quiser desenhar também o ponto de origem em vermelho
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(groundCheck.position, groundCheckSize);

            // Se quiser visualizar a colisão em tempo real
            RaycastHit2D hit = Physics2D.BoxCast(
                groundCheck.position,
                groundCheckSize,
                0f,
                Vector2.down,
                rayDistance,
                groundLayer
            );
            if (hit.collider != null)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(hit.point, 0.1f); // marca exatamente onde encostou
            }
        }
    }

}