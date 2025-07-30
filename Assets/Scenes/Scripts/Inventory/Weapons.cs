using UnityEngine;

[CreateAssetMenu(fileName = "Weapons")]

public class Weapons : ScriptableObject
{
    public float WeaponDamage;
    public float WeaponCoolDown;    
    public float BulletSpeed;

    public Sprite WeaponIcon;

}
