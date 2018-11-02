using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    protected ControlMap controlMap = new ControlMap();
    private Rigidbody2D rb2D;

    [Header("Base Config")]
    [SerializeField] private float moveSpd = 10f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float groundRaycastLen = 0.7f;
    private enum WeaponType { MELEE, GUN };
    [SerializeField] private WeaponType currWeapon = WeaponType.MELEE;

    [Header("Knockback")]
    [SerializeField] private float knockbackMultiplyer = 1;
    [SerializeField] private float knockbackIncrement = 5;
    [SerializeField] private float baseKnockback = 10;
    public float currKnockbackForce = 0;

    [Header("Melee Properties")]
    [SerializeField] private float meleePower = 1;
    [SerializeField] private float meleeRange = 2;
    [SerializeField] private float meleeCD = 0.2f;
    private Timer meleeNextCD = new Timer();

    [Header("Gun Properties")]
    [SerializeField] private float gunPower = 0.1f;
    [SerializeField] private Transform firingPt;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpd = 10f;
    [SerializeField] private float gunCD = 0.5f;
    private Timer gunNextCD = new Timer();

    private Timer controlsUnlockTime = new Timer();
    private float controlsLockDuration = 1f;

    public enum FaceDir {NULL, LEFT, RIGHT };
    protected FaceDir currFaceDir = FaceDir.RIGHT;

    protected void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    private void MeleeAtt()
    {
        if (!GameManager.instance.EnemyInRange(meleeRange))
            return;

        if (!gunNextCD.TimeIsUp)
            return;

        meleeNextCD.SetTimer(meleeCD);
        Vector2 moveDir;

        if (currFaceDir == FaceDir.LEFT)
            moveDir = Vector2.left;
        else
            moveDir = Vector2.right;

        GameManager.instance.GetEnemy(this).TakeDamage(moveDir, meleePower);
    }

    private void GunShoot()
    {
        if (!gunNextCD.TimeIsUp)
            return;

        gunNextCD.SetTimer(gunCD);
        GameObject bullet = Instantiate(bulletPrefab, firingPt.position, firingPt.rotation);

        Vector2 dir = Vector2.right;
        if (currFaceDir == FaceDir.LEFT)
            dir = -dir;

        bullet.GetComponent<Projectile>().SetPushForce(gunPower);
        bullet.GetComponent<Rigidbody2D>().AddForce(dir * bulletSpd, ForceMode2D.Impulse);
    }

    private void Update()
    {
        MovePlayer();
        UpdatePlayerFaceDir();

        if (Input.GetKeyDown(controlMap.basicAtt))
        {
            switch (currWeapon)
            {
                case WeaponType.MELEE:
                    MeleeAtt();
                    break;
                case WeaponType.GUN:
                    GunShoot();
                    break;
            }
        }
    }

    private void UpdatePlayerFaceDir(FaceDir forcedFaceDir = FaceDir.NULL)
    {
        float localYScalePositive = Mathf.Abs(transform.localScale.x);

        if (forcedFaceDir == FaceDir.LEFT || rb2D.velocity.x < 0)
        {
            currFaceDir = FaceDir.LEFT;
            transform.localScale = new Vector2(-localYScalePositive, localYScalePositive);
        }
        else if (forcedFaceDir == FaceDir.RIGHT || rb2D.velocity.x > 0)
        {
            currFaceDir = FaceDir.RIGHT;
            transform.localScale = new Vector2(localYScalePositive, localYScalePositive);
        }
    }

    private void MovePlayer()
    {
        if (!controlsUnlockTime.TimeIsUp)
            return;

        if (Input.GetKey(controlMap.left))
        {
            rb2D.velocity = -(Vector2)Vector3.right * moveSpd + new Vector2(0, rb2D.velocity.y);
        }
        else if (Input.GetKey(controlMap.right))
        {
            rb2D.velocity = (Vector2)Vector3.right * moveSpd + new Vector2(0, rb2D.velocity.y);
        }

        if (!Input.GetKey(controlMap.left) && !Input.GetKey(controlMap.right))
        {
            rb2D.velocity = new Vector2(0, rb2D.velocity.y);
        }

        if (IsOnGround() && Input.GetKeyDown(controlMap.jump))
        {
            rb2D.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    protected bool IsOnGround()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundRaycastLen, 1 << LayerMask.NameToLayer("Ground"));
        return hit.collider;
    }

    public void ConfigurePlayer(ControlMap controlMap)
    {
        this.controlMap = controlMap;
    }

    public void TakeDamage(Vector2 pushbackDir, float weaponPower)
    {
        currKnockbackForce += knockbackIncrement;
        rb2D.velocity = Vector2.zero;

        pushbackDir.y = 1;
        pushbackDir.Normalize();

        rb2D.AddForce(pushbackDir * (baseKnockback + currKnockbackForce) * knockbackMultiplyer * weaponPower, ForceMode2D.Impulse);
        controlsUnlockTime.SetTimer(controlsLockDuration);
    }

    public void SetFaceDir(FaceDir dir)
    {
        UpdatePlayerFaceDir(dir);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Border"))
        {
            GameManager.instance.RegisterBorderCollision(this);
        }
    }

    public void ResetCurrentKnockback()
    {
        currKnockbackForce = 0;
    }
}
