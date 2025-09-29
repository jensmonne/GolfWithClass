using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GolfNetworkManager : NetworkManager
{
    public static GolfNetworkManager Instance;
    
    [HideInInspector] public string currentRoomCode;
    
    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }
    
    public void CreateLobby() {
        StartHost();
        currentRoomCode = GenerateRoomCode();
        Debug.Log("Created Lobby with Code: " + currentRoomCode);
        ServerChangeScene("Lobby");
    }
    
    public void JoinLobby(string roomCode) {
        // For now, map any roomCode -> localhost
        // Later you can hook this up to a real room resolver
        networkAddress = "localhost";
        StartClient();
        Debug.Log("Joining Lobby with Code: " + roomCode);
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

    private string GenerateRoomCode() {
        const string chars = "123456789";
        string code = "";
        for (int i = 0; i < 6; i++) {
            code += chars[Random.Range(0, chars.Length)];
        }
        return code;
    }
}
