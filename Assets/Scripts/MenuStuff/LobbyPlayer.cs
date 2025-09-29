using Mirror;
using UnityEngine;

public class LobbyPlayer : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnNameChanged))] 
    public string playerName;

    [SyncVar(hook = nameof(OnHostChanged))] 
    public bool isHost;

    private LobbyUI lobbyUI;

    public override void OnStartClient()
    {
        base.OnStartClient();
        lobbyUI = FindFirstObjectByType<LobbyUI>();
        lobbyUI.AddPlayer(this);
    }

    public override void OnStopClient()
    {
        base.OnStopClient();
        if (lobbyUI != null)
            lobbyUI.RemovePlayer(this);
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
    }

    [Command]
    private void CmdSetPlayerName(string newName)
    {
        playerName = newName;
    }

    [Command]
    private void CmdSetHost(bool value)
    {
        isHost = value;
    }

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