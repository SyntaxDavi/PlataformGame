using UnityEngine;

[CreateAssetMenu(fileName = "NewJumpState", menuName = "AI/State/Jump")]
public class JumpingAction : AiState
{
    public float JumpForce = 10f;
    public float FallGravityMultiplier = 7f;

    private float OriginalGravityScale;

    public override void OnEnter(AiController controller)
    {
        if (!controller.IsGrounded || !controller.CanJump)
        {
            return;
        }

        controller.CanJump = false;

        OriginalGravityScale = controller.Rb.gravityScale;
        float HorizontalSpeed = controller.Rb.linearVelocity.x;
        controller.Rb.linearVelocity = new Vector2(HorizontalSpeed,JumpForce);
    }
    public override void Execute(AiController controller)
    {

        if (controller.Rb.linearVelocity.y < 0)
        {
            
            controller.Rb.gravityScale = OriginalGravityScale * FallGravityMultiplier;
        }

        CheckTransistions(controller);
    }

    public override void OnExit(AiController controller)
    {
        controller.Rb.gravityScale = OriginalGravityScale;
        controller.CanJump = true;
       
    }
}
