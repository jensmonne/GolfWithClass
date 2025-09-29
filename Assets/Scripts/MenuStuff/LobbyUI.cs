using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LobbyUI : MonoBehaviour
{
    [SerializeField] private TMP_Text codeText;
    [SerializeField] private Transform playerListParent;
    [SerializeField] private GameObject playerEntryPrefab;
    
    private readonly Dictionary<LobbyPlayer, GameObject> playerEntries = new();
    
    private void Start()
    {
        codeText.text = PlayerPrefs.GetString("RoomCode");
    }
    
    public void AddPlayer(LobbyPlayer player)
    {
        GameObject entry = Instantiate(playerEntryPrefab, playerListParent);
        playerEntries[player] = entry;
        UpdatePlayer(player);
    }

    public void RemovePlayer(LobbyPlayer player)
    {
        if (playerEntries.TryGetValue(player, out var entry))
        {
            Destroy(entry);
            playerEntries.Remove(player);
        }
    }

    public void UpdatePlayer(LobbyPlayer player)
    {
        if (playerEntries.TryGetValue(player, out var entry))
        {
            TMP_Text text = entry.GetComponentInChildren<TMP_Text>();
            text.text = player.isHost ? $"{player.playerName} 👑" : player.playerName;
        }
    }
    
    public void OnExitButton()
    {
        GolfNetworkManager.Instance.LeaveLobby();
    }
    
    public void OnInviteButton()
    {
        
    }

    public void OnStartButton()
    {
        GolfNetworkManager.Instance.StartGame();
    }
}