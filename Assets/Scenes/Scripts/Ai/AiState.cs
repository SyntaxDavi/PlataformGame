using UnityEngine;

public abstract class AiState : ScriptableObject
{
    // O que fazer enquanto estiver neste estado
    public abstract void Execute(AiController controller);
    // O que fazer ao entrar neste estado pela primeira vez
    public virtual void OnEnter(AiController Controller) { }
    // Sair do estado
    public virtual void OnExit(AiController Controller) { }

    [SerializeField]
    private AiTransition[] Transitions;

    public void CheckTransistions(AiController controller)
    {
        for(int i = 0; i < Transitions.Length; i++)
        {
            bool DecisionSucceeded = Transitions[i].Decision.Decide(controller);

            if (DecisionSucceeded)
            {
                controller.TransitionToState(Transitions[i].TrueState);
                return;
            }
        }
    }
}

[System.Serializable]
public class AiTransition
{
    public AiDecision Decision;
    public AiState TrueState;
}
