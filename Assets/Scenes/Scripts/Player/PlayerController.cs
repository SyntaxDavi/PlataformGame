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

                if (!Movement.IsGrounded && Movement.Rigidbody.linearVelocity.y < -0.1f)
                {
                    ChangeState(EPlayerState.Falling);
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

    public void ChangeState(EPlayerState newState)
    {
        if(CurrentState == newState) { return; }
        Debug.Log($"Mudando de estado: {CurrentState} -> {newState}");

        CurrentState = newState;

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
                lockedStateTimer = 0.5f;
                // Health.applyKnockback
                break;

            case EPlayerState.Dead:
                break;
        }
    }

    public void OnGrounded()
    {
        if (CurrentState == EPlayerState.Jumping || CurrentState == EPlayerState.Falling)
        {
            ChangeState(EPlayerState.Idle);
        }
    }

}
