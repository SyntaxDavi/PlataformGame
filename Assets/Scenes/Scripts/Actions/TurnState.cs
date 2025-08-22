using UnityEngine;

[CreateAssetMenu(fileName = "NewTurnState", menuName = "AI/State/Turn")]
public class TurnState : AiState
{
    public override void OnEnter(AiController controller)
    {
        controller.Rb.linearVelocity = Vector2.zero;
        controller.Flip();
    }

    public override void Execute(AiController controller)
    {

        CheckTransistions(controller);
    }
}
