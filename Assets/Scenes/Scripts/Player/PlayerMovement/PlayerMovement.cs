using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance { get; private set; }
    public static PlayerAttack AttackScript; 

    public float BaseSpeed = 3f;
    public float JumpGravityScale = 1f;
    public float FallGravityScale = 2.5f;
    public float JumpForce = 5f;
    public float GroundCheckRadius = 0.1f;

    public Transform GroundCheck;
    public LayerMask GroundLayer;
    public static Vector2 FacingDirection = Vector2.right;

    private float AttackBaseSpeed = 2.0f;
    private float MoveX = 0f;
    private bool IsGrounded = true;
    Rigidbody2D rb;

    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        rb = GetComponent<Rigidbody2D>();
        if (rb == null) { Debug.LogError("RB nao encontrado"); }
    }
    void Start()
    {
        AttackScript = GetComponent<PlayerAttack>();
        Debug.Log("Game Started");
    }
    private void Update()
    {
        HandleInputMovement();
        CheckGravity();
    }
    private void FixedUpdate()
    {
        IsGrounded = Physics2D.OverlapCircle(GroundCheck.position, GroundCheckRadius, GroundLayer); 
    }

    private void CheckGravity()
    {
        if (rb.linearVelocity.y < 0)
        {
            rb.gravityScale = FallGravityScale;
        }
        else
        {
            rb.gravityScale = JumpGravityScale;
        }
    }
    private void HandleInputMovement()
    {
        float MoveHorizontal = Input.GetAxisRaw("Horizontal");
        float MoveVertical = Input.GetAxisRaw("Vertical");

        Vector2 AimInput = new Vector2(MoveHorizontal, MoveVertical);

        if(AimInput != Vector2.zero)
        {
            FacingDirection = AimInput.normalized;
        }

        float PlayerSpeed = AttackScript.IsPlayerAttacking ? AttackBaseSpeed : BaseSpeed;

        MoveX = MoveHorizontal
            ;
        if (IsGrounded && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, JumpForce);
        }

        rb.linearVelocity = new Vector2(MoveX * PlayerSpeed, rb.linearVelocity.y);
    }
}