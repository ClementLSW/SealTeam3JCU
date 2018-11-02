using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Spawn Points")]
    [SerializeField] private Transform p1Spawn;
    [SerializeField] private Transform p2Spawn;

    [Header("Player Prefabs")]
    [SerializeField] private GameObject playerGunSlinger_Prefab;
    [SerializeField] private GameObject playerMinotaur_Prefab;

    private enum CharTypes { GUNSLINGER, MINOTAUR };

    [SerializeField] private List<ControlMap> controlMaps = new List<ControlMap>();

    private List<GameObject> spawnedPlayers = new List<GameObject>();

    private void Start()
    {
        if (!instance)
            instance = this;
        else
            Destroy(gameObject);

        SpawnPlayers(CharTypes.GUNSLINGER, CharTypes.GUNSLINGER);
    }

    private GameObject GetPlayerPrefab(CharTypes charType)
    {
        switch (charType)
        {
            case CharTypes.GUNSLINGER:
                return playerGunSlinger_Prefab;
            case CharTypes.MINOTAUR:
                return playerMinotaur_Prefab;
            default:
                return null;
        }
    }

    private void SpawnPlayers(CharTypes p1, CharTypes p2)
    {
        if(p1Spawn)
        {
            GameObject player = Instantiate(GetPlayerPrefab(p1), p1Spawn.position, p1Spawn.rotation);
            player.GetComponent<Player>().ConfigurePlayer(controlMaps[0]);
            player.name = "Player1";
            spawnedPlayers.Add(player);
        }

        if (p2Spawn)
        {
            GameObject player = Instantiate(GetPlayerPrefab(p2), p2Spawn.position, p2Spawn.rotation);
            player.GetComponent<Player>().ConfigurePlayer(controlMaps[1]);
            player.GetComponent<Player>().SetFaceDir(Player.FaceDir.LEFT);
            player.name = "Player2";
            spawnedPlayers.Add(player);
        }

        CameraController.instance.SetPlayersToTrack(spawnedPlayers);
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
