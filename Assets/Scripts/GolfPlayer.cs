using Mirror;
using UnityEngine;

public class GolfPlayer : NetworkBehaviour
{
    [SyncVar] public string playerName;
    [SyncVar] public int score;
    
    public override void OnStartLocalPlayer()
    {
        string defaultName = PlayerPrefs.GetString("PlayerName", $"Player{Random.Range(1000, 9999)}");
        CmdSetPlayerName(defaultName);
    }
    
    [Command]
    private void CmdSetPlayerName(string newName)
    {
        playerName = newName;
    }

    [Command]
    public void CmdAddScore(int amount)
    {
        score += amount;
    }
}
