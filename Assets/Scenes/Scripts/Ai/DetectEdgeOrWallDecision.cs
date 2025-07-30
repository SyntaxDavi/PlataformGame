using UnityEngine;

[CreateAssetMenu(fileName = "NewDetectEdgeOrWallDecision", menuName = "AI/Decision/Detect Edge or Wall")]
public class DetectEdgeOrWallDecision : AiDecision
{
    public LayerMask GroundLayer;

    [Header("Detecção de Borda e Parede (Para Virar)")]
    public float WallCheckDistance = 0.4f;

    [Header("Ajuste de Posição do Raycast")]
    [Tooltip("Offset para o raio que detecta a beirada e aciona o giro.")]

    public Vector2 RaycastOffset;

    [Header("Verificação de Segurança para Pulo")]
    [Tooltip("Habilita a verificação que impede o pulo se a plataforma for curta.")]

    public bool EnableJumpSafeCheck = true;

    [Tooltip("Distância à frente para verificar se a plataforma é longa o suficiente para um pulo seguro.")]
    public float JumpSafetyDistance = 1.5f;

    public abstract class AiDecision : ScriptableObject
    {
        public abstract bool Decide(AiController controller);
    }
    public override bool Decide(AiController controller)
    {
        if (!controller.IsGrounded)
        {
            return false;
        }

        bool wallDetected = CheckForWall(controller);
        bool edgeDetected = CheckForEdge(controller);

        return wallDetected || edgeDetected;
    }

    public bool IsPlataformAheadSafe(AiController controller)
    {
        if (!EnableJumpSafeCheck)
        {
            return true;
        }

        Vector2 Origin = (Vector2)controller.transform.position;
        Origin.x += (controller.BoxCollider.size.x / 2 + JumpSafetyDistance) * controller.FacingDirection;

        //Dispara o raio para baixo para ver se ainda há chão na distancia de segurança.

        RaycastHit2D Hit = Physics2D.Raycast(Origin,Vector2.down, 2f, GroundLayer);
        Debug.DrawRay(Origin, Vector2.down * 2f, (Hit.collider != null) ? Color.blueViolet : Color.darkRed);
        
        return Hit.collider != null;
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
        RaycastHit2D Hit = Physics2D.Raycast(Origin, Vector2.down, 1f, GroundLayer);
        return Hit.collider == null;
    }

}
