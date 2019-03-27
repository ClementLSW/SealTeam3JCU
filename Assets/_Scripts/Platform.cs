using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class Platform : MonoBehaviour
{

    private int hitPoints;
    [SerializeField] private ParticleSystem explode;
    [SerializeField] private SpriteShape damagedSprite;

    private void Start()
    {
        hitPoints = 10;
    }

    public void registerDamage(int dmg)
    {
        hitPoints -= dmg;
        checkHP();
    }

    public void checkHP()
    {
        if (hitPoints <= 0)
        {
            //ParticleSystem fx = Instantiate(explode, this.gameObject.transform, false);
            //fx.transform.parent = null;
            //fx.transform.localScale = new Vector3(1, 1, 1);
            gameObject.AddComponent<Rigidbody2D>();
            gameObject.GetComponent<Rigidbody2D>().gravityScale = 5;
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 10);
            Destroy(gameObject.GetComponent<EdgeCollider2D>());
            Destroy(gameObject, 3.0f);
        }
        else if (hitPoints <= 5)
        {
            gameObject.GetComponent<SpriteShapeController>().spriteShape = damagedSprite;
        }
    }

    [ContextMenu("Explode")]
    public void Explode()
    {
        hitPoints = 0;
        checkHP();
    }

    //Debug Method

    //public void Update()
    //{
    //    if (Input.GetKeyDown("space"))
    //    {
    //        this.registerDamage(1);
    //    }
    //}
}