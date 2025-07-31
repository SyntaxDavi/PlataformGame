using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private float SpaceOfShot = 0.05f;
    private float AttackDuration = 0.2f;
    private float AttackTimer = 0f;

    public float ShotCoolDown = 2.5f;
    public float AttackDamage = 5f;

    private bool PlayerCanAttack = true;

    public float ShotBaseSpeed { get; private set; } = 10f;
    public bool IsPlayerAttacking { get; private set; }
    public Weapons CurrentWeapon { get; private set; }
    private float TimeElapsed = 0f;


    private void Update()
    {
        PlayerCanAttack = CanShot();

        if (Input.GetKeyDown(KeyCode.Mouse0) && PlayerCanAttack)
        {
            GameObject Shot = PoolSpawner.Instance.GetBullet(1);
            IsPlayerAttacking = true;
            AttackTimer = AttackDuration;

            if (Shot != null)
            {
                Shot.SetActive(true);

                Vector3 SpawnOffset = (Vector3)PlayerMovement.FacingDirection * SpaceOfShot;
                Shot.transform.position = transform.position + SpawnOffset;
                Shot.transform.rotation = Quaternion.identity;

                var BulletScript = Shot.GetComponent<PlayerBullet>();
                Shot.GetComponent<PlayerBullet>().SetDirection(PlayerMovement.FacingDirection);
                BulletScript.PlayerAttackInstance = this;

                ShotBaseSpeed = CurrentWeapon != null ? CurrentWeapon.BulletSpeed : ShotBaseSpeed;
                AttackDamage = CurrentWeapon != null ? CurrentWeapon.WeaponDamage : AttackDamage;
            }

            TimeElapsed = 0f;
            PlayerCanAttack = false;
        }

        if (IsPlayerAttacking)
        {
            AttackTimer -= Time.deltaTime;
            if (AttackTimer <= 0f)
            {
                IsPlayerAttacking = false;
            }
        }
    }
    private bool CanShot()
    {
        TimeElapsed += Time.deltaTime;
        return TimeElapsed >= ShotCoolDown;
    }

    public void ResetCoolDown()
    {
        TimeElapsed = 2.5f;
        PlayerCanAttack = true;
    }
    public void SetWeapon(Weapons weapon)
    {
        CurrentWeapon = weapon;
        AttackDamage = weapon.WeaponDamage;
        ShotCoolDown = weapon.WeaponCoolDown;
    }
}