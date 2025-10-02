using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utp;

public class GolfNetworkManager : NetworkManager
{
    UtpTransport utp;
    public static GolfNetworkManager Instance;
    private List<Transform> spawnPoints = new List<Transform>();
    private int nextIndex = 0;

    public override void Awake() {
        base.Awake();
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
        
        utp = GetComponent<UtpTransport>();
        if (utp == null) Debug.LogError("UtpTransport is missing.");
    }
    
    public void CreateLobby(int maxPlayers) {
        Debug.Log("Creating lobby...");
        LoadingUI.Instance.Show();
        StartRelayHost(maxPlayers, () => {
            LoadingUI.Instance.AnimationSceneSwitchIn();
        });
    }

    public void OnZoomInComplete()
    {
        Debug.Log("Zoomin complete");
        ServerChangeScene("Lobby");
        int holes = PlayerPrefs.GetInt("Holes", 1);
        LevelManager.Instance.SetupLevelList(holes);
    }

    public void JoinLobby(string joinCode) {
        JoinRelayServer(joinCode);
        Debug.Log($"Joining lobby with code {joinCode}");
    }

    public void LeaveLobby() {
        if (NetworkServer.active && NetworkClient.isConnected) {
            StopHost();
        } else if (NetworkClient.isConnected) {
            StopClient();
        }

        SceneManager.LoadScene("MainMenu");
    }

    public void StartGame() {
        if (NetworkServer.active)
        {
            LevelManager.Instance.LoadNextLevel();
        }
    }
    
    private void StartRelayHost(int maxPlayers, Action onSuccess, string regionId = null)
    {
        utp.useRelay = true;
        utp.AllocateRelayServer(maxPlayers, regionId,
            (string joinCode) =>
            {
                Debug.Log($"Relay JoinCode: {joinCode}");
                PlayerPrefs.SetString("RoomCode", joinCode);
                StartHost();
                onSuccess?.Invoke();
            },
            () =>
            {
                UtpLog.Error("Failed to start a Relay host.");
            });
    }
    
    private void JoinRelayServer(string joinCode)
    {
        utp.useRelay = true;
        utp.ConfigureClientWithJoinCode(joinCode,
        () =>
        {
            StartClient();
        },
        () =>
        {
            UtpLog.Error("Failed to join Relay server.");
        });
    }
    
    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        if (conn.identity != null)
        {
            Debug.LogWarning($"Player already exists for connection {conn.connectionId}, skipping spawn.");
            return;
        }

        base.OnServerAddPlayer(conn);
    }
    
    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();
        Debug.Log("Disconnected from server. Returning to main menu...");
        SceneManager.LoadScene("MainMenu");
    }
    
    public override void OnServerSceneChanged(string sceneName)
    {
        base.OnServerSceneChanged(sceneName);

        spawnPoints.Clear();

        foreach (var sp in FindObjectsByType<SpawnPoint>(FindObjectsSortMode.None))
        {   
            spawnPoints.Add(sp.transform);
        }

        nextIndex = 0;
    }
    
    public override Transform GetStartPosition()
    {
        if (spawnPoints.Count == 0)
            return base.GetStartPosition();

        Transform spawn = spawnPoints[nextIndex % spawnPoints.Count];
        nextIndex++;
        return spawn;
    }
}
