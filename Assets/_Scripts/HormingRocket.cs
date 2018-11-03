using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HormingRocket : Projectile
{
    private Transform target;
    private float flySpd;
    private float steerSpd;

    private new void Start()
    {
        base.Start();
    }

    public void SetupHormingRocket(Transform target, float flySpd, float steerSpd)
    {
        this.target = target;
        this.flySpd = flySpd;
        this.steerSpd = steerSpd;
    }

    private void Update()
    {
        Vector2 dir = target.position - transform.position;
        dir.Normalize();
        float rotateAmt = Vector3.Cross(dir, transform.up).z;
        rb.angularVelocity = -rotateAmt * steerSpd;
        rb.velocity = transform.up * flySpd;
    }

    protected new void OnCollisionEnter2D(Collision2D collider)
    {
        CameraController.instance.Shake(0.3f, 0.3f);

        if (collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            collider.gameObject.GetComponent<Platform>().registerDamage(1);
        }
        SFXManager.instance.PlaySound(SFXManager.Sound.EXPLOSION, false);
        base.OnCollisionEnter2D(collider);
    }
}
