using TMPro;
using UnityEngine;

public class LobbyUI : MonoBehaviour
{
    [SerializeField] private TMP_Text codeText;
    
    private void Start()
    {
        codeText.text = PlayerPrefs.GetString("RoomCode");
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