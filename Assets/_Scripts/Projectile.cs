using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Collider2D coll;
    protected Rigidbody2D rb;

    private float weaponPower;

    protected void Start()
    {
        coll = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, 10);
    }

    public void SetupProjectile(float weaponPower)
    {
        this.weaponPower = weaponPower;
    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Vector2 moveDir = rb.velocity;
            moveDir.Normalize();
            collision.gameObject.GetComponent<Player>().TakeDamage(moveDir, weaponPower);
        }
        Destroy(gameObject);
    }
}
