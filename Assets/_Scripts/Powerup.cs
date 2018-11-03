using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Powerup : MonoBehaviour
{
    private SpriteRenderer img;

    [Header("Sprite Images")]
    [SerializeField] private Sprite global;
    [SerializeField] private Sprite personal;
    [SerializeField] private Sprite teleport;

    public enum PowerupType { NULL, GLOBAL, PERSONAL, TELEPORT };
    public PowerupType powerupType = PowerupType.NULL;
    public enum GlobalPowerupType { SPEED, KNOCKBACK, FIRERATE }

    private void Start()
    {
        img = GetComponent<SpriteRenderer>();
        Setup();
    }

    private void Setup()
    {
        switch (Random.Range(0,2))
        {
            case 0:
                img.sprite = global;
                powerupType = PowerupType.GLOBAL;
                break;
            case 1:
                img.sprite = personal;
                powerupType = PowerupType.PERSONAL;
                break;
            case 2:
                img.sprite = teleport;
                powerupType = PowerupType.TELEPORT;
                break;
        }
    }

    public void Pickuped()
    {
        // TODO: Play animation
        GameManager.instance.PowerupCollected(powerupType);
        Destroy(gameObject);
    }
}