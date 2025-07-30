using UnityEngine;

[CreateAssetMenu(fileName = "NewTurnState", menuName = "AI/State/Turn")]
public class TurnState : AiState
{
    public override void OnEnter(AiController controller)
    {
        controller.Rb.linearVelocity = Vector2.zero;
        controller.Flip();

        CheckTransistions(controller);
    }

    public override void Execute(AiController controller)
    {
            
    }
}
