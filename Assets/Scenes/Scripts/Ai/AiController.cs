using UnityEngine;

[RequireComponent(typeof(Rigidbody2D),typeof(CharacterStats), typeof(BoxCollider2D))]
public class AiController : MonoBehaviour
{
    [Header("Configura��o de Estados")]
    [Tooltip("O estado inicial da IA quando ela � ativada")]
    public AiState InitialState;
    public AiState CurrentState {  get; private set; }

    [Header("Configura��o de detec��o")]
    public LayerMask GroundLayer;
    [Header("Configura��o de Decis�o e Seguran�a")]
    [Tooltip("Arraste aqui o asset de decis�o que verifica bordas e seguran�a de pulo")]
    public DetectEdgeOrWallDecision SafetyDecision;
    public bool CanJump { get; set; } = true;

    public BoxCollider2D BoxCollider { get; private set; }  
    public Rigidbody2D Rb {  get; private set; }
    public CharacterStats Stats { get; private set; }
    public Transform PlayerTransform { get; private set; }

    [HideInInspector] public int FacingDirection = 1;
    private void Awake()
    {
        Rb = GetComponent<Rigidbody2D>();
        Stats = GetComponent<CharacterStats>();
        BoxCollider = GetComponent<BoxCollider2D>();

        GameObject Player = GameObject.FindGameObjectWithTag("Player");
        if(Player != null)
        {
            PlayerTransform = Player.transform;
        }       
    }

    private void OnEnable()
    {
        TransitionToState(InitialState);
    }

    private void FixedUpdate()
    {
        if(CurrentState != null)
        {
            UpdateJumpPermission();
            CurrentState.Execute(this);
            CurrentState.CheckTransistions(this);
        }
    }

    private void UpdateJumpPermission()
    {
        if (SafetyDecision != null)
        {
            CanJump = SafetyDecision.IsPlataformAheadSafe(this);
        }
        else
        {
            CanJump = true;
        }
    }

    public void TransitionToState(AiState NextState)
    {
        if (NextState == null) return;
        if (CurrentState != null)
        {
            CurrentState.OnExit(this);
        } 
        CurrentState = NextState;
        CurrentState.OnEnter(this);
    }

    public void Flip()
    {
        FacingDirection *= -1;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }

    public bool IsGrounded
    {
        get
        {
            float ExtraHeight = 0.1f;

            Vector2 Origin = (Vector2)transform.position + BoxCollider.offset;
            Vector2 BoxSize = BoxCollider.size;

            RaycastHit2D Hit = Physics2D.BoxCast(Origin, BoxSize, 0f, Vector2.down, ExtraHeight, GroundLayer);
            return Hit.collider != null;
        }
    }

    private void OnDrawGizmos()
    {
        if (BoxCollider == null) return;
        Gizmos.color = Color.yellow;
        Vector2 boxSize = BoxCollider.size;
        Vector2 origin = (Vector2)transform.position + BoxCollider.offset;

        // Gizmo para verifica��o de ch�o
        Gizmos.DrawWireCube(origin + Vector2.down * (boxSize.y / 2 + 0.05f), new Vector2(boxSize.x * 0.9f, 0.1f));

        // Gizmo para verifica��o de parede
        Gizmos.color = Color.red;
        Gizmos.DrawLine(origin, origin + new Vector2(FacingDirection * (boxSize.x / 2 + 0.1f), 0));

        Gizmos.color = Color.green;
        float extraHeight = 0.1f;
        Gizmos.DrawWireCube(origin + Vector2.down * (boxSize.y / 2 + extraHeight / 2), new Vector2(boxSize.x, extraHeight));

    }  //Debug
}

