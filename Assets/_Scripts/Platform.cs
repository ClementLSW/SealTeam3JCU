using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class Platform : MonoBehaviour {
    
    [SerializeField] private int hitPoints = 4;
    [SerializeField] private ParticleSystem explode;
    [SerializeField] private SpriteShape damagedSprite;

    public void registerDamage(int dmg)
    {
        hitPoints -= dmg;
        checkHP();
    }

    public void checkHP()
    {
        if (hitPoints <= 0)
        {
            explode.Play();
            GameObject.Destroy(gameObject, 3.0f);
        }
        else if(hitPoints <= 2)
        {
            gameObject.GetComponent<SpriteShapeController>().spriteShape = damagedSprite;
        }
    }
}
