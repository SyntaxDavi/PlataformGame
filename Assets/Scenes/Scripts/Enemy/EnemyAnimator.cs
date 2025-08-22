using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyAnimator : MonoBehaviour
{
   private Animator animator;

    // Usamos StringToHash para otimizar. Isso converte a string do parâmetro
    // em um ID inteiro, o que é muito mais rápido para o Animator processar.
    // Faça isso para TODOS os seus parâmetros.
    private static readonly int IsWalkingHash = Animator.StringToHash("IsWalking");
    private static readonly int IsGroundedHash = Animator.StringToHash("IsGrounded");
    private static readonly int YVelocityHash = Animator.StringToHash("YVelocity");
    private static readonly int JumpTriggerHash = Animator.StringToHash("JumpTrigger");
    private static readonly int HurtTriggerHash = Animator.StringToHash("HurtTrigger");
    private static readonly int DeathTriggerHash = Animator.StringToHash("DeathTrigger");
    private static readonly int AttackTriggerHash = Animator.StringToHash("AttackTrigger");

    private void Awake()
    {
       animator = GetComponent<Animator>();
    }

    // --- API PÚBLICA ---
    // Estes são os métodos que seus outros scripts (como as Ações da IA) vão chamar.
    // Eles são simples e descrevem uma intenção.

    public void SetWalk(bool isWalking)
    {
        animator.SetBool(IsWalkingHash, isWalking);
    }

    public void SetGrounded(bool isGrounded)
    {
        animator.SetBool(IsGroundedHash, isGrounded);
    }

    public void UpdateYVelocity(float velocity)
    {
        // Útil para transições de pulo para queda.
        animator.SetFloat(YVelocityHash, velocity);
    }

    public void TriggerJump()
    {
        animator.SetTrigger(JumpTriggerHash);
    }

    public void TriggerAttack()
    {
        // Exemplo para um futuro estado de ataque
        animator.SetTrigger(AttackTriggerHash);
    }

    public void TriggerHurt()
    {
        animator.SetTrigger(HurtTriggerHash);
    }

    public void TriggerDeath()
    {
        animator.SetTrigger(DeathTriggerHash);
    }
}
