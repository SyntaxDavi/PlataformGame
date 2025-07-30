using UnityEngine;

[CreateAssetMenu(fileName = "NewPeriodicDecision", menuName = "AI/Decision/Periodic")]
public class PeriodicDecision : AiDecision
{
    public float Interval = 3f;
    private float Timer = 0;

    public override bool Decide(AiController controller)
    {
        Timer += Time.deltaTime;
        if(Timer >= Interval)
        {
            Timer = 0;
            return true;
        }
        return false;
    }
}
