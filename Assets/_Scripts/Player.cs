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

    private float dmgRecieved = 0f;
    private float controlsUnlockTime;
    public enum FaceDir {NULL, LEFT, RIGHT };
    protected FaceDir currFaceDir = FaceDir.RIGHT;

    protected void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    protected void Update()
    {
        MovePlayer();
        UpdatePlayerFaceDir();
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
        if (ControlsLocked())
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

        if (hit.collider != null)
            return true;
        else
            return false;
    }

    private bool ControlsLocked()
    {
        return controlsUnlockTime > Time.time;
    }

    public void ConfigurePlayer(ControlMap controlMap)
    {
        this.controlMap = controlMap;
    }

    public void TakeDamage(float dmgAmt, Vector2 pushForce, float stunDuration)
    {
        dmgRecieved += dmgAmt;
        rb2D.velocity = Vector2.zero;
        rb2D.AddForce(pushForce, ForceMode2D.Impulse);
        controlsUnlockTime = Time.time + stunDuration;
    }

    public void SetFaceDir(FaceDir dir)
    {
        UpdatePlayerFaceDir(dir);
    }
}
