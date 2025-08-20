using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D),typeof(CharacterStats), typeof(BoxCollider2D))]
public class AiController : MonoBehaviour
{
    [Header("Configuração de Estados")]
    [Tooltip("O estado inicial da IA quando ela é ativada")]
    public AiState InitialState;
    public AiState CurrentState {  get; private set; }

    [Header("Configuração de detecção")]
    public LayerMask GroundLayer;
    [Header("Configuração de Decisão e Segurança")]
    [Tooltip("Arraste aqui o asset de decisão que verifica bordas e segurança de pulo")]
    public DetectEdgeOrWallDecision SafetyDecision;
    public bool CanJump { get; set; } = true;

    [Header("Configuração de Impacto")]
    [Tooltip("A força inicial do empurrão de knockback")]
    public float knockbackInitialForce = 15f;
    [Tooltip("Duração em segundos no knockback")]
    public float knockbackDuration = 0.45f;
    [Tooltip("A cor que o inimigo pisca ao tomar dano")]
    public Color HitFlashColor = Color.white;
    [Tooltip("A duração do congelamento do inimigo:")]
    public float HitStopDuration = 0.05f;

    private bool isBeingKnockBack = false;
    private SpriteRenderer hitRenderer;
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
        hitRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void OnEnable()
    {
        if(GameEvents.PlayerTransform != null)
        {
            PlayerTransform = GameEvents.PlayerTransform;
        }

        GameEvents.OnPlayerSpawned += HandlePlayerSpawned;
        GameEvents.OnPlayerDeath += HandlePlayerDeath;
        Stats.OnDamaged.AddListener(HandleDamageTaken);

        isBeingKnockBack = false;
        TransitionToState(InitialState);
    }

    private void OnDisable()
    {
        // SEMPRE se desinscreve dos eventos para evitar memory leaks
        GameEvents.OnPlayerSpawned -= HandlePlayerSpawned;
        GameEvents.OnPlayerDeath -= HandlePlayerDeath;
        Stats.OnDamaged.RemoveListener(HandleDamageTaken);
    }

    private void HandlePlayerSpawned(Transform playerTransform)
    {
        PlayerTransform = playerTransform;
    }

    private void HandlePlayerDeath()
    {
        PlayerTransform = null;
    }

    private void FixedUpdate()
    {
        if( PlayerTransform == null || isBeingKnockBack) { return; }

        if(CurrentState != null)
        {
            UpdateJumpPermission();
            CurrentState.Execute(this);
            CurrentState.CheckTransistions(this);
        }
    }

    private void HandleDamageTaken()
    {
        if(PlayerTransform == null) { return; }

        StartCoroutine(HitStopCoroutine());

        if(hitRenderer != null)
        {
            StartCoroutine(DamageFlashCoroutine());
        }

        Vector2 knockbackDirection = (transform.position - PlayerTransform.position).normalized;
        ApplyKnockback(knockbackDirection);
    }

    public void ApplyKnockback(Vector2 direction)
    {
        if (isBeingKnockBack) { return ; }

        StartCoroutine(knockbackCoroutine(direction));
    }

    private IEnumerator knockbackCoroutine(Vector2 direction)
    {
        isBeingKnockBack = true;
        float timer = 0;
        Vector2 initialVelocity = direction * knockbackInitialForce;

        while(timer < knockbackDuration)
        {
            float t = 1 - (timer / knockbackDuration);
            Rb.linearVelocity = Vector2.Lerp(Vector2.zero,initialVelocity, t);
            timer += Time.fixedDeltaTime;

            yield return new WaitForFixedUpdate();
        }

        Rb.linearVelocity = Vector2.zero;
        isBeingKnockBack = false;
    }
    private IEnumerator DamageFlashCoroutine()
    {
        Color originalColor = hitRenderer.color;
        hitRenderer.color = HitFlashColor;
        yield return new WaitForSeconds(0.08f);
        hitRenderer.color = originalColor;
    }

    private IEnumerator HitStopCoroutine()
    {
        if(HitStopDuration <= 0) { yield break; }
        float originalTimeScale = Time.timeScale;
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(HitStopDuration);
        Time.timeScale = originalTimeScale;
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

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (BoxCollider == null) return;
        Gizmos.color = Color.yellow;
        Vector2 boxSize = BoxCollider.size;
        Vector2 origin = (Vector2)transform.position + BoxCollider.offset;

        // Gizmo para verificação de chão
        Gizmos.DrawWireCube(origin + Vector2.down * (boxSize.y / 2 + 0.05f), new Vector2(boxSize.x * 0.9f, 0.1f));

        // Gizmo para verificação de parede
        Gizmos.color = Color.red;
        Gizmos.DrawLine(origin, origin + new Vector2(FacingDirection * (boxSize.x / 2 + 0.1f), 0));

        Gizmos.color = Color.green;
        float extraHeight = 0.1f;
        Gizmos.DrawWireCube(origin + Vector2.down * (boxSize.y / 2 + extraHeight / 2), new Vector2(boxSize.x, extraHeight));

    }  //Debug
#endif
}

