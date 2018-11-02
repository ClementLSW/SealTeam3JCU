using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Collider2D coll;

    [SerializeField] private float dmgAmt = 0f;
    [SerializeField] private float pushForce = 1f;
    [SerializeField] private Vector3 pushBias;

    private void Start()
    {
        coll = GetComponent<Collider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            // TODO: Add proper push dir
            collision.gameObject.GetComponent<Player>().TakeDamage(dmgAmt, pushBias * pushForce, 0.5f);
        }

        Destroy(gameObject);
    }
}
