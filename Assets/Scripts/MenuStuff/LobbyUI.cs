using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LobbyUI : MonoBehaviour
{
    [SerializeField] private TMP_Text codeText;
    [SerializeField] private Transform playerListParent;
    [SerializeField] private GameObject playerEntryPrefab;
    
    private readonly Dictionary<GolfPlayer, GameObject> playerEntries = new();
    
    private void Start()
    {
        codeText.text = PlayerPrefs.GetString("RoomCode");
    }
    
    public void AddPlayer(GolfPlayer player)
    {
        GameObject entry = Instantiate(playerEntryPrefab, playerListParent);
        playerEntries[player] = entry;
        UpdatePlayer(player);
    }

    public void RemovePlayer(GolfPlayer player)
    {
        if (playerEntries.TryGetValue(player, out var entry))
        {
            Destroy(entry);
            playerEntries.Remove(player);
        }
    }

    public void UpdatePlayer(GolfPlayer player)
    {
        if (playerEntries.TryGetValue(player, out var entry))
        {
            TMP_Text text = entry.GetComponentInChildren<TMP_Text>();
            text.text = player.isHost ? $"{player.playerName} (Host)" : player.playerName;
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