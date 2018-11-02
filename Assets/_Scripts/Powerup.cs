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

    public enum PowerupType { GLOBAL, PERSONAL, TELEPORT };
    public PowerupType powerupType = PowerupType.GLOBAL;

    private void Start()
    {
        img = GetComponent<SpriteRenderer>();
        Setup();
    }

    private void Setup()
    {
        switch (powerupType)
        {
            case PowerupType.GLOBAL:
                img.sprite = global;
                break;
            case PowerupType.PERSONAL:
                img.sprite = personal;
                break;
            case PowerupType.TELEPORT:
                img.sprite = teleport;
                break;
        }
    }

    public void Pickuped()
    {
        // TODO: Play animation
        GameManager.instance.PowerupCollected();
        Destroy(gameObject);
    }
}
