using Mirror;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GolfPlayer : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnNameChanged))] 
    public string playerName;

    [SyncVar(hook = nameof(OnHostChanged))] 
    public bool isHost;
    
    [SyncVar] public int score;

    private CinemachineCamera vcam;
    private LobbyUI lobbyUI;

    public override void OnStartClient()
    {
        base.OnStartClient();
        SceneManager.sceneLoaded += OnSceneLoaded;

        HandleScene(SceneManager.GetActiveScene().name);
    }
    
    public override void OnStopClient()
    {
        base.OnStopClient();
        if (lobbyUI != null)
            lobbyUI.RemovePlayer(this);
        if (vcam != null)
            vcam.enabled = false;
        var listener = GetComponentInChildren<AudioListener>();
        if (listener != null)
            listener.enabled = false;
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        
        if (isServer && isLocalPlayer)
        {
            CmdSetHost(true);
        }

        string defaultName = PlayerPrefs.GetString("PlayerName", $"Player{Random.Range(1000,9999)}");
        CmdSetPlayerName(defaultName);
        
        vcam = GetComponentInChildren<CinemachineCamera>(true);
        if (vcam != null)
        {
            vcam.Priority = 10;
            vcam.enabled = true;
        }
        
        var listener = GetComponentInChildren<AudioListener>(true);
        if (listener != null)
            listener.enabled = true;
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        HandleScene(scene.name);
    }
    
    private void HandleScene(string sceneName)
    {
        if (sceneName == "Lobby")
        {
            lobbyUI = FindFirstObjectByType<LobbyUI>();
            if (lobbyUI != null)
                lobbyUI.AddPlayer(this);
        }
        else
        {
            Debug.Log($"{playerName} is ready to play minigolf!");
        }
    }

    [Command]
    private void CmdSetPlayerName(string newName) => playerName = newName;

    [Command]
    private void CmdSetHost(bool value) => isHost = value;
    
    [Command]
    public void CmdAddScore(int amount) => score += amount;

    private void OnNameChanged(string _, string newName)
    {
        if (lobbyUI != null)
            lobbyUI.UpdatePlayer(this);
    }

    private void OnHostChanged(bool _, bool newValue)
    {
        if (lobbyUI != null)
            lobbyUI.UpdatePlayer(this);
    }
}