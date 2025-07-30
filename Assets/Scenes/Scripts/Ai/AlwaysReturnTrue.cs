using UnityEngine;

[CreateAssetMenu(fileName = "AlwaysReturnTrue", menuName = "AI/Decision/AlwaysReturnTrue")]
public class AlwaysReturnTrue : AiDecision
{
    public override bool Decide(AiController controller)
    {
        return true;
    }
    
}
