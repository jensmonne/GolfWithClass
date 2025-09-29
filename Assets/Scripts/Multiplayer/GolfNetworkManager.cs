using System;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utp;

public class GolfNetworkManager : NetworkManager
{
    UtpTransport utp;
    private string relayJoinCode;
    public static GolfNetworkManager Instance;
    
    private void Awake() {
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
        StartRelayHost(maxPlayers, () => {
            Debug.Log("Lobby created.");
            ServerChangeScene("Lobby");
        });
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
        if (NetworkServer.active) {
            ServerChangeScene("SampleScene");
        }
    }
    
    private void StartRelayHost(int maxPlayers, Action onSuccess, string regionId = null)
    {
        utp.useRelay = true;
        utp.AllocateRelayServer(maxPlayers, regionId,
            (string joinCode) =>
            {
                relayJoinCode = joinCode;
                Debug.LogError($"Relay JoinCode: {joinCode}");
                PlayerPrefs.SetString("RoomCode", joinCode);
                StartHost();
                onSuccess?.Invoke();
            },
            () =>
            {
                UtpLog.Error($"Failed to start a Relay host.");
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
            UtpLog.Error($"Failed to join Relay server.");
        });
    }

    public override void OnStopServer()
    {
        relayJoinCode = null;
        base.OnStopServer();
    }
    
    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();
        Debug.Log("Disconnected from server. Returning to main menu...");
        SceneManager.LoadScene("MainMenu");
    }
}
