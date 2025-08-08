using UnityEngine;

public abstract class PlayerAbility : ScriptableObject
{
    public string AbilityName;
    public float CoolDown;
    public Sprite AbilityIcon;

    public abstract void Activate(PlayerController owner);
}
