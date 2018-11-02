using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    private List<GameObject> playersToTrack = new List<GameObject>();
    private Camera cam;

    [Header("Camera Bounds")]
    [SerializeField] private float maxX;
    [SerializeField] private float minX;
    [SerializeField] private float maxY;
    [SerializeField] private float minY;

    [Header("Camera Movement")]
    [SerializeField] private float camMoveSpd = 1;
    [SerializeField] private float camZoomMultiplyer = 1f;
    [SerializeField] private float minZoom = 5f;

    private void Start()
    {
        if (instance)
            Destroy(gameObject);
        else
            instance = this;

        cam = GetComponent<Camera>();
    }

    private void Update()
    {
        if (playersToTrack[0] != null && playersToTrack[1] != null)
            TrackPlayers();
    }

    public void SetPlayersToTrack(GameObject player1, GameObject player2)
    {
        List<GameObject> players = new List<GameObject>();
        players.Add(player1);
        players.Add(player2);
        playersToTrack = players;
    }

    private void TrackPlayers()
    {
        // Get center point of 2 players
        Vector3 playersCenterMass = (playersToTrack[0].transform.position + playersToTrack[1].transform.position) / 2;
        playersCenterMass -= Vector3.forward;

        // Dist of 2 players
        float dist = Vector3.Distance(playersToTrack[0].transform.position, playersToTrack[1].transform.position);

        // Keeping camera within bounds
        if (playersCenterMass.x > maxX)
            playersCenterMass.x = maxX;

        if (playersCenterMass.x < minX)
            playersCenterMass.x = minX;

        if (playersCenterMass.y > maxY)
            playersCenterMass.y = maxY;

        if (playersCenterMass.y < minY)
            playersCenterMass.y = minY;

        // Apply CamPos
        transform.position = Vector3.Lerp(transform.position, playersCenterMass, Time.deltaTime * camMoveSpd);

        // Apply CamZoom
        cam.orthographicSize = dist * camZoomMultiplyer;
        if (cam.orthographicSize < minZoom)
            cam.orthographicSize = minZoom;
    }
}
