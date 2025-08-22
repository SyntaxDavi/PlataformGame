    using UnityEngine;

[RequireComponent(typeof(PlayerHealth))]
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerAttack))]
public class PlayerController : MonoBehaviour
{
    [Header("Referências")]
    public PlayerHealth Health {  get; private set; }
    public PlayerMovement Movement { get; private set; }
    public PlayerAttack Attack { get; private set; }
    private Animator animator;

    public EPlayerState CurrentState { get; private set; }
    private float lockedStateTimer;

    [Header("Slots para habilidades")]
    public PlayerAbility DashAbility;
    public PlayerAbility PowerAbility;

    private void Awake()
    {
        Health = GetComponent<PlayerHealth>();
        Movement = GetComponent<PlayerMovement>();
        Attack = GetComponent<PlayerAttack>();
        animator = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        GameEvents.OnPlayerDeath += HandleRespawn;
    }

    private void OnDisable()
    {
        GameEvents.OnPlayerDeath -= HandleRespawn;
    }
    private void Start()
    {
        ChangeState(EPlayerState.Idle);
    }

    private void Update()
    {
        Movement.HandleInputReading();
        HandleStateUpdate();
    }
    private void HandleRespawn()
    {
        ChangeState(EPlayerState.Idle);
    }
    private void HandleStateUpdate()
    {
        if (lockedStateTimer > 0)
        {
            lockedStateTimer -= Time.deltaTime;
            if (lockedStateTimer <= 0)
            {
                ChangeState(Movement.IsGrounded ? EPlayerState.Idle : EPlayerState.Falling);
            }

            return;
        }

        if (Movement.HasJumpBuffer && Movement.CanUseCoyoteTime)
        {
            ChangeState(EPlayerState.Jumping);
            return;
        }

        if(!Movement.IsGrounded && Movement.Rigidbody.linearVelocity.y < -0.1f)
        {
            ChangeState(EPlayerState.Falling);
        }


        switch (CurrentState)
        {
            case EPlayerState.Idle:
            case EPlayerState.Moving:
               
                if(Mathf.Abs(Movement.RawInputDirection.x) > 0.1f)
                {
                    ChangeState(EPlayerState.Moving);
                }
                else
                {
                    ChangeState(EPlayerState.Idle);
                }

                Attack.HandleAttackInput();
                if (!Movement.IsGrounded) ChangeState(EPlayerState.Falling);
                break;

            case EPlayerState.Jumping:
            case EPlayerState.Falling:
                Attack.HandleAttackInput();
                break;

            case EPlayerState.Attacking:
            case EPlayerState.Hurt:
            case EPlayerState.Dead:
                break;

        }
    }

    public void ChangeState(EPlayerState newState, float stateLockDuration = 0f)
    {
        if(CurrentState == newState) { return; }
        Debug.Log($"Mudando de estado: {CurrentState} -> {newState}");

        CurrentState = newState;
        animator.SetInteger("State", (int)newState);

        Movement.SetAttackingMovement(false);

        switch (newState)
        {
            case EPlayerState.Idle:
            case EPlayerState.Moving:
                break;

            case EPlayerState.Jumping:
                Movement.ExecuteJump();
                break;

            case EPlayerState.Falling:
                break;

            case EPlayerState.Attacking:
                lockedStateTimer = Attack.CurrentCoolDown;
                Attack.ExecuteAttack();
                Movement.SetAttackingMovement(true);
                break;

            case EPlayerState.Hurt:
                lockedStateTimer = stateLockDuration;
                break;

            case EPlayerState.Dead:
                break;
        }
    }

    public void OnGrounded()
    {
        if(CurrentState == EPlayerState.Jumping || CurrentState == EPlayerState.Falling)
    {
            animator.SetTrigger("Land");

            ChangeState(EPlayerState.Idle);
        }
    }

}
