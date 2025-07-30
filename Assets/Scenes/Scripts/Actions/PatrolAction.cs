using UnityEngine;

[CreateAssetMenu(fileName = "NewPatrolState", menuName = "AI/State/Patrol")]
public class PatrolAction : AiState
{
    public override void Execute(AiController controller)
    {
        if (!controller.IsGrounded)
        {
            controller.Rb.linearVelocity = new Vector2(controller.Rb.linearVelocity.x, controller.Rb.linearVelocity.y);
            return;
        }

        float Speed = controller.Stats.CurrentSpeed;
        controller.Rb.linearVelocity = new Vector2(controller.FacingDirection * Speed, controller.Rb.linearVelocity.y);
    }
}
