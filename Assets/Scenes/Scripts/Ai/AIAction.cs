using UnityEngine;

// Toda ação é um ScriptableObject, um asset no projeto
public abstract class AIAction : ScriptableObject
{
    //o "Contrato": toda ação deve implementar um método Execute.
    // Ela recebe o AIController como parâmetro para poder acessaro o RB, stats, etc.

    public abstract void Execute(AiController controller);

    
}
