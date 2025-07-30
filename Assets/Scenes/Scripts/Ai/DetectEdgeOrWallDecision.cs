using UnityEngine;

[CreateAssetMenu(fileName = "NewDetectEdgeOrWallDecision", menuName = "AI/Decision/Detect Edge or Wall")]
public class DetectEdgeOrWallDecision : AiDecision
{
    public LayerMask GroundLayer;
    public float WallCheckDistance = 0.4f;

    [Header("Ajuste de Posição do Raycast")] 
    [Tooltip("Offset para ajustar a origem do Raycast.")]
    public Vector2 RaycastOffset;

    public override bool Decide(AiController controller)
    {
        if (!controller.IsGrounded)
        {
            return false;
        }

        bool WallDetected = CheckForWall(controller);
        bool EdgeDetected = CheckForEdge(controller);
        return WallDetected || EdgeDetected;
    }

    private bool CheckForWall(AiController controller)
    {
        Vector2 Origin = (Vector2)controller.transform.position + controller.BoxCollider.offset;
        Origin.x += RaycastOffset.x * controller.FacingDirection;
        Origin.y += RaycastOffset.y;

        float Distance = controller.BoxCollider.size.x / 2 + WallCheckDistance;
        RaycastHit2D Hit = Physics2D.Raycast(Origin, new Vector2(controller.FacingDirection, 0), Distance, GroundLayer);
        return Hit.collider != null;
    }

    private bool CheckForEdge(AiController controller)
    {
        Vector2 Origin = (Vector2)controller.transform.position + controller.BoxCollider.offset;

        Origin.x += (controller.BoxCollider.size.x / 2 + RaycastOffset.x) * controller.FacingDirection;
        RaycastHit2D Hit = Physics2D.Raycast(Origin , Vector2.down, 1f, GroundLayer);
        return Hit.collider == null;
    }
}
