using UnityEngine;

[CreateAssetMenu(fileName = "New Dash Ability", menuName = "Abilities/Dash")]
public class DashAbility : PlayerAbility
{
    public float DashForce;

    public override void Activate(PlayerController owner)
    {
        owner.ChangeState(EPlayerState.Dashing);
    }
}
