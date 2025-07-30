using UnityEngine;

// Toda a��o � um ScriptableObject, um asset no projeto
public abstract class AIAction : ScriptableObject
{
    //o "Contrato": toda a��o deve implementar um m�todo Execute.
    // Ela recebe o AIController como par�metro para poder acessaro o RB, stats, etc.

    public abstract void Execute(AiController controller);

    
}
