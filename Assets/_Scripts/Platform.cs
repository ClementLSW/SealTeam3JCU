using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class Platform : MonoBehaviour {
    
    [SerializeField] private int hitPoints;
    [SerializeField] private ParticleSystem explode;
    [SerializeField] private SpriteShape damagedSprite;

    private void Start()
    {
        hitPoints = 4;
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
            Instantiate(explode);
            Destroy(gameObject);
        }
        else if(hitPoints <= 2)
        {
            gameObject.GetComponent<SpriteShapeController>().spriteShape = damagedSprite;
        }
    }

    //Debug Method

    public void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            this.registerDamage(1);
        }
    }
}
