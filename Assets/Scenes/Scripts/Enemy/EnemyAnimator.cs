using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyAnimator : MonoBehaviour
{
   private Animator animator;

    // Usamos StringToHash para otimizar. Isso converte a string do par�metro
    // em um ID inteiro, o que � muito mais r�pido para o Animator processar.
    // Fa�a isso para TODOS os seus par�metros.
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

    // --- API P�BLICA ---
    // Estes s�o os m�todos que seus outros scripts (como as A��es da IA) v�o chamar.
    // Eles s�o simples e descrevem uma inten��o.

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
        // �til para transi��es de pulo para queda.
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
