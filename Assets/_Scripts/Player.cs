using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    protected ControlMap controlMap = new ControlMap();
    private Rigidbody2D rb2D;

    [Header("Base Config")]
    [SerializeField] private float moveSpd = 10f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float groundRaycastLen = 0.7f;
    private enum WeaponType { MELEE, GUN, ROCKET };
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
    [SerializeField] private Transform gunFiringPt;
    [SerializeField] private float gunPower = 0.1f;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpd = 10f;
    [SerializeField] private float gunCD = 0.5f;
    [SerializeField] private float gunPowerupDuration = 10f;
    private Timer gunNextCD = new Timer();

    [Header("Hom Rocket Properties")]
    [SerializeField] private Transform rocketFiringPt;
    [SerializeField] private float rocketPower = 1.5f;
    [SerializeField] private GameObject rocketPrefab;
    [SerializeField] private float rocketSpd = 10f;
    [SerializeField] private float rocketCD = 2f;
    [SerializeField] private float rocketSteerSpd = 10f;
    [SerializeField] private float rocketPowerupDuration = 10f;
    private Timer rocketNextCD = new Timer();

    public Timer controlsUnlockTime = new Timer();
    private float controlsLockDuration = 1f;

    public enum FaceDir {NULL, LEFT, RIGHT };
    protected FaceDir currFaceDir = FaceDir.RIGHT;

    [Space(10)]

    [SerializeField] private Image arrowImg;
    [SerializeField] private TextMeshProUGUI playerNameTxt;

    [Space(10)]
    [SerializeField] private GameObject globalPU;

    protected void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
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
                case WeaponType.ROCKET:
                    RocketShoot();
                    break;
            }
        }
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
        CameraController.instance.Shake(0.1f, 0.1f);
    }

    private void GunShoot()
    {
        if (!gunNextCD.TimeIsUp)
            return;

        gunNextCD.SetTimer(gunCD);
        GameObject bullet = Instantiate(bulletPrefab, gunFiringPt.position, gunFiringPt.rotation);

        Vector2 dir = Vector2.right;
        if (currFaceDir == FaceDir.LEFT)
            dir = -dir;

        bullet.GetComponent<Projectile>().SetupProjectile(gunPower);
        bullet.GetComponent<Rigidbody2D>().AddForce(dir * bulletSpd, ForceMode2D.Impulse);
    }

    private void RocketShoot()
    {
        if (!rocketNextCD.TimeIsUp)
            return;

        rocketNextCD.SetTimer(rocketCD);
        GameObject rocket = Instantiate(rocketPrefab, rocketFiringPt.position, rocketFiringPt.rotation);

        Vector2 dir = Vector2.right;
        if (currFaceDir == FaceDir.LEFT)
            dir = -dir;

        rocket.GetComponent<HormingRocket>().SetupProjectile(rocketPower);
        rocket.GetComponent<HormingRocket>().SetupHormingRocket(
            GameManager.instance.GetEnemy(this).gameObject.transform,
            rocketSpd,
            rocketSteerSpd
            );
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

    public void ConfigurePlayer(ControlMap controlMap, Color arrowColor, string playerName)
    {
        this.controlMap = controlMap;
        arrowImg.color = arrowColor;
        playerNameTxt.text = playerName;

        Destroy(playerNameTxt, 3);
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
            CameraController.instance.Shake(1f, 0.2f);
            currKnockbackForce = 0;
        }

        if(collision.gameObject.tag == "Powerup")
        {
            Powerup powerup = collision.GetComponent<Powerup>();
            switch (powerup.powerupType)
            {
                case Powerup.PowerupType.GLOBAL:
                    Debug.Log("Get GLOBAL");
                    GameManager.instance.PowerupCollected(Powerup.PowerupType.GLOBAL);
                    break;
                case Powerup.PowerupType.PERSONAL:
                    Debug.Log("Get PERSONAL");
                    StartCoroutine(SwitchWeaponTemp());
                    GameManager.instance.PowerupCollected(Powerup.PowerupType.PERSONAL);
                    break;
                case Powerup.PowerupType.TELEPORT:
                    Debug.Log("Get TELEPORT");
                    GameManager.instance.PowerupCollected(Powerup.PowerupType.TELEPORT);
                    break;
                default:
                    break;
            }
            powerup.Pickuped();
        }
    }

    public IEnumerator TempFiringRateModification()
    {
        SetGlobalPowerupAnim(true);
        float origMeleeCD = meleeCD;
        float origGunCD = gunCD;
        float origRocketCD = rocketCD;

        meleeCD *= 2;
        gunCD *= 2;
        rocketCD *= 2;
        yield return new WaitForSeconds(10);
        SetGlobalPowerupAnim(false);

        meleeCD = origMeleeCD;
        gunCD = origGunCD;
        rocketCD = origRocketCD;
    }

    public IEnumerator TempKnockbackModification()
    {
        SetGlobalPowerupAnim(true);
        float origKnockbackMultiplyer = knockbackMultiplyer;
        origKnockbackMultiplyer *= 2;
        yield return new WaitForSeconds(10);
        SetGlobalPowerupAnim(false);
        knockbackMultiplyer = origKnockbackMultiplyer;
    }

    public IEnumerator TempSpeedModification()
    {
        SetGlobalPowerupAnim(true);
        float origSpd = moveSpd;
        moveSpd *= 2;
        yield return new WaitForSeconds(10);
        SetGlobalPowerupAnim(false);
        moveSpd = origSpd;
    }

    public IEnumerator SwitchWeaponTemp()
    {
        int weaponType = Random.Range(0, 2);
        float weaponDuration = 0;
        if (weaponType == 0)
        {
            currWeapon = WeaponType.GUN;
            weaponDuration = gunPowerupDuration;
        }
        else if (weaponType == 1)
        {
            currWeapon = WeaponType.ROCKET;
            weaponDuration = rocketPowerupDuration;
        }
        Debug.Log("Set Weapon CD to " + weaponDuration);
        yield return new WaitForSeconds(weaponDuration);
        Debug.Log("CD Fin");
        currWeapon = WeaponType.MELEE;
    }

    private void SetGlobalPowerupAnim(bool state)
    {
        Debug.Log("globalPU SetActive " + state);
        globalPU.gameObject.SetActive(state);
    }

    public void ResetCurrentKnockback()
    {
        currKnockbackForce = 0;
    }
}
