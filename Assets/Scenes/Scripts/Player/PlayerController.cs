using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerHealth Health {  get; private set; }
    public PlayerMovement Movement { get; private set; }
    public PlayerAttack Attack { get; private set; }

    private void Awake()
    {
        Health = GetComponent<PlayerHealth>();
        Movement = GetComponent<PlayerMovement>();
        Attack = GetComponent<PlayerAttack>();
    }

}
