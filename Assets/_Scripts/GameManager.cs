using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Spawn Points")]
    [SerializeField] private Transform area1_p1_spawn;
    [SerializeField] private Transform area1_p2_spawn;

    [Header("Player Prefabs")]
    [SerializeField] private GameObject playerPrefab;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI player1KnockbackTxt;
    [SerializeField] private TextMeshProUGUI player2KnockbackTxt;
    [SerializeField] private TextMeshProUGUI player1LivesTxt;
    [SerializeField] private TextMeshProUGUI player2LivesTxt;

    [SerializeField] private List<ControlMap> controlMaps = new List<ControlMap>();

    private Player player1;
    private Player player2;

    [SerializeField] private int player1Lives = 5;
    [SerializeField] private int player2Lives = 5;

    private void Start()
    {
        if (!instance)
            instance = this;
        else
            Destroy(gameObject);

        SpawnPlayers();
    }

    private void Update()
    {
        player1KnockbackTxt.text = "Knockback:" + player1.currKnockbackForce.ToString();
        player2KnockbackTxt.text = "Knockback:" + player2.currKnockbackForce.ToString();

        player1LivesTxt.text = "Lives:" + player1Lives.ToString();
        player2LivesTxt.text = "Lives:" + player2Lives.ToString();
    }

    private void SpawnPlayers()
    {
        // Spawn player1
        player1 = Instantiate(playerPrefab, area1_p1_spawn.position, area1_p1_spawn.rotation).GetComponent<Player>();
        player1.ConfigurePlayer(controlMaps[0]);
        player1.name = "Player1";

         // Spawn player2
        player2 = Instantiate(playerPrefab, area1_p2_spawn.position, area1_p2_spawn.rotation).GetComponent<Player>();
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
        Debug.Log("HEY");
        sourcePlayer.gameObject.SetActive(false);
        yield return new WaitForSeconds(3);
        sourcePlayer.gameObject.transform.position = area1_p1_spawn.position;
        sourcePlayer.gameObject.SetActive(true);
        yield return null;
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
