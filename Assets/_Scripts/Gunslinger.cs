using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gunslinger : Player
{
    [Header("Gun Properties")]
    [SerializeField] private Transform firingPt;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpd = 10f;

    private new void Update()
    {
        base.Update();

        if(Input.GetKeyDown(controlMap.basicAtt))
        {
            FireBullet();
        }
    }

    private void FireBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, firingPt.position, firingPt.rotation);

        Vector2 dir = Vector2.right;
        if (currFaceDir == FaceDir.LEFT)
            dir = -dir;

        bullet.GetComponent<Rigidbody2D>().AddForce(dir * bulletSpd, ForceMode2D.Impulse);
    }
}
