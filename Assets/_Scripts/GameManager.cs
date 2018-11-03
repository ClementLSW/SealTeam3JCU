﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private bool powerupCollected = true;

    [System.Serializable]
    private class Area
    {
        public Transform spn1;
        public Transform spn2;

        public List<Transform> powerupSpawnPts = new List<Transform>();

        public Transform GetRandPlayerSpn()
        {
            if (Random.Range(0, 1) == 0)
                return spn1;
            else
                return spn2;
        }

        public Transform GetRandPowerupSpn()
        {
            return powerupSpawnPts[Random.Range(0, powerupSpawnPts.Count - 1)];
        }
    }
    [SerializeField] private List<Area> area = new List<Area>();
    private Area currArea;

    [Header("Player Prefabs")]
    [SerializeField] private GameObject playerPrefab;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI player1KnockbackTxt;
    [SerializeField] private TextMeshProUGUI player2KnockbackTxt;
    [SerializeField] private TextMeshProUGUI player1LivesTxt;
    [SerializeField] private TextMeshProUGUI player2LivesTxt;
    [SerializeField] private TextMeshProUGUI gameOverlayText;
    private Timer gameCDTimer = new Timer();

    [Space(10)]

    [SerializeField] private List<ControlMap> controlMaps = new List<ControlMap>();

    [Header("Powerup")]
    [SerializeField] private GameObject powerup_Prefab;
    [SerializeField] private float powerupSpawnFreq;
    private Timer powerupSpnTimer = new Timer();

    private Player player1;
    private Player player2;
    private bool setupDone = false;

    [Header("Total Health")]
    [SerializeField] private int player1Lives = 5;
    [SerializeField] private int player2Lives = 5;

    private void Start()
    {
        if (!instance)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Update()
    {
        if (!setupDone)
        {
            return;
        }

        player1KnockbackTxt.text = "Knockback:" + player1.currKnockbackForce.ToString();
        player2KnockbackTxt.text = "Knockback:" + player2.currKnockbackForce.ToString();

        player1LivesTxt.text = "Lives:" + player1Lives.ToString();
        player2LivesTxt.text = "Lives:" + player2Lives.ToString();

        SpawnPowerup();
    }

    public void Setup()
    {
        SpawnPlayers();
        player1.controlsUnlockTime.SetTimer(3);
        player2.controlsUnlockTime.SetTimer(3);
        gameCDTimer.SetTimer(3);
        StartCoroutine(CountDownGameStart());
        setupDone = true;
    }

    private IEnumerator CountDownGameStart()
    {
        while(!gameCDTimer.TimeIsUp)
        {
            gameOverlayText.text = ((int)gameCDTimer.TimeLeft + 1).ToString();
            yield return null;
        }

        gameOverlayText.text = "BEGIN";
        yield return new WaitForSeconds(1);
        gameOverlayText.text = "";
        // Enable controls
    }

    private void SetRandArea()
    {
        if (currArea != null)
            currArea = area[(area.IndexOf(currArea) + 1) % area.Count];
        else
            currArea = area[0];
    }

    private void SpawnPowerup()
    {
        if (powerupSpnTimer.TimeIsUp && powerupCollected)
        {
            powerupSpnTimer.SetTimer(powerupSpawnFreq);
            Instantiate(powerup_Prefab, currArea.GetRandPowerupSpn().position, Quaternion.identity);
            powerupCollected = false;
        }
    }

    public void PowerupCollected(Powerup.PowerupType powerup)
    {
        switch (powerup)
        {
            case Powerup.PowerupType.GLOBAL:
                switch(Random.Range(0, 3))
                {
                    case 0:
                        player1.TempKnockbackModification();
                        player2.TempKnockbackModification();
                        break;
                    case 1:
                        player1.TempSpeedModification();
                        player2.TempSpeedModification();
                        break;
                    case 2:
                        player1.TempKnockbackModification();
                        player2.TempKnockbackModification();
                        break;
                }
                break;
            case Powerup.PowerupType.TELEPORT:
                StartCoroutine(ChangeArea());
                ChangeArea();
                break;
            default:
                break;
        }

        powerupCollected = true;
    }

    private IEnumerator ChangeArea()
    {
        player1.gameObject.SetActive(false);
        player2.gameObject.SetActive(false);
        SetRandArea();
        // Play lightning animation
        yield return new WaitForSeconds(2);
        player1.gameObject.transform.position = currArea.spn1.position;
        player2.gameObject.transform.position = currArea.spn2.position;
        player1.gameObject.SetActive(true);
        player2.gameObject.SetActive(true);

        foreach (Powerup powerup in FindObjectsOfType<Powerup>())
        {
            Destroy(powerup.gameObject);
            powerupCollected = false;
        }
    }

    private void SpawnPlayers()
    {
        SetRandArea();

        // Spawn player1
        player1 = Instantiate(playerPrefab, currArea.spn1.position, currArea.spn1.rotation).GetComponent<Player>();
        player1.ConfigurePlayer(controlMaps[0], Color.red, "Player1");
        player1.name = "Player1";

         // Spawn player2
        player2 = Instantiate(playerPrefab, currArea.spn2.position, currArea.spn2.rotation).GetComponent<Player>();
        player2.ConfigurePlayer(controlMaps[1], Color.blue, "Player2");
        player2.GetComponent<Player>().SetFaceDir(Player.FaceDir.LEFT);
        player2.name = "Player2";

        CameraController.instance.SetPlayersToTrack(player1.gameObject, player2.gameObject);
    }

    public void RegisterBorderCollision(Player sourcePlayer)
    {
        sourcePlayer.ResetCurrentKnockback();
        StartCoroutine(SendPlayerToSpawn(sourcePlayer));

        if (sourcePlayer == player1)
            player1Lives -= 1;
        else
            player2Lives -= 1;
    }

    private IEnumerator SendPlayerToSpawn(Player sourcePlayer)
    {
        sourcePlayer.gameObject.SetActive(false);
        yield return new WaitForSeconds(3);
        sourcePlayer.gameObject.transform.position = currArea.GetRandPlayerSpn().position;
        sourcePlayer.gameObject.SetActive(true);
        yield return null;
    }

    public bool EnemyInRange(float dist)
    {
        return Vector2.Distance(player1.gameObject.transform.position, player2.gameObject.transform.position) <= dist;
    }

    public Player GetEnemy(Player sourcePlayer)
    {
        if (sourcePlayer == player1)
            return player2;
        else
            return player1;
    }
}

[System.Serializable]
public class ControlMap
{
    public KeyCode left = KeyCode.A;
    public KeyCode right = KeyCode.D;
    public KeyCode jump = KeyCode.Space;

    public KeyCode basicAtt = KeyCode.N;
}
