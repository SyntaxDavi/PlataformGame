using UnityEngine;

[CreateAssetMenu(fileName = "NewPeriodicDecision", menuName = "AI/Decision/Periodic")]
public class PeriodicDecision : AiDecision
{
    public float Interval = 3f;

    public override bool Decide(AiController controller)
    {
        if (controller.PlayerTransform == null)
        {
            return false;
        }
        else
        {
            if (!controller.DecisionTimers.ContainsKey(this))
            {
                controller.DecisionTimers[this] = 0;
            }

            controller.DecisionTimers[this] += Time.deltaTime;

            if (controller.DecisionTimers[this] >= Interval)
            {
                controller.DecisionTimers[this] = 0;
                return true;
            }
        }

        return false;
    }
}
