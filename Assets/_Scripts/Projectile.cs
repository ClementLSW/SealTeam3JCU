using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Collider2D coll;
    
    [SerializeField] private float pushForce = 0.2f;

    private void Start()
    {
        coll = GetComponent<Collider2D>();
        Destroy(gameObject, 10);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Vector2 moveDir = GetComponent<Rigidbody2D>().velocity;
            moveDir.Normalize();
            collision.gameObject.GetComponent<Player>().TakeDamage(moveDir);
        }

        Destroy(gameObject);
    }
}
