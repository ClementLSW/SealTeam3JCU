using System.Collections;
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

    [Space(10)]

    [SerializeField] private List<ControlMap> controlMaps = new List<ControlMap>();

    [Header("Powerup")]
    [SerializeField] private GameObject powerup_Prefab;
    [SerializeField] private float powerupSpawnFreq;
    private Timer powerupSpnTimer = new Timer();

    private Player player1;
    private Player player2;

    [Header("Total Health")]
    [SerializeField] private int player1Lives = 5;
    [SerializeField] private int player2Lives = 5;

    private void Start()
    {
        if (!instance)
            instance = this;
        else
            Destroy(gameObject);

        SpawnPlayers();
        SpawnPowerup();
    }

    private void Update()
    {
        player1KnockbackTxt.text = "Knockback:" + player1.currKnockbackForce.ToString();
        player2KnockbackTxt.text = "Knockback:" + player2.currKnockbackForce.ToString();

        player1LivesTxt.text = "Lives:" + player1Lives.ToString();
        player2LivesTxt.text = "Lives:" + player2Lives.ToString();
    }

    private void SetRandArea()
    {
        currArea = area[Random.Range(0, area.Count - 1)];
    }

    private void SpawnPowerup()
    {
        if(powerupSpnTimer.TimeIsUp && powerupCollected)
        {
            powerupSpnTimer.SetTimer(powerupSpawnFreq);
            Instantiate(powerup_Prefab, currArea.GetRandPowerupSpn().position, Quaternion.identity);
            powerupCollected = false;
        }
    }

    public void PowerupCollected()
    {
        Debug.Log("Powerup Collecte");
        powerupCollected = true;
    }

    private void SpawnPlayers()
    {
        SetRandArea();

        // Spawn player1
        player1 = Instantiate(playerPrefab, currArea.spn1.position, currArea.spn1.rotation).GetComponent<Player>();
        player1.ConfigurePlayer(controlMaps[0]);
        player1.name = "Player1";

         // Spawn player2
        player2 = Instantiate(playerPrefab, currArea.spn2.position, currArea.spn2.rotation).GetComponent<Player>();
        player2.ConfigurePlayer(controlMaps[1]);
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
