using TMPro;
using UnityEngine;

public class MenuUI : MonoBehaviour
{
    [SerializeField] private GameObject joinMenu;
    [SerializeField] private GameObject createMenu;
    [SerializeField] private TMP_InputField maxPlayersInput;
    [SerializeField] private TMP_InputField holesInput;
    [SerializeField] private TMP_InputField roomCodeInput;
    [SerializeField] private TMP_InputField playerNameInput;

    private string randomName;

    private void Start()
    {
        randomName = $"Player{Random.Range(1000,9999)}";
        playerNameInput.text = PlayerPrefs.GetString("PlayerName", randomName);
        playerNameInput.placeholder.GetComponent<TextMeshProUGUI>().text = randomName;
        playerNameInput.onValueChanged.AddListener(OnValueChanged);
    }

    private void OnValueChanged(string text)
    {
        PlayerPrefs.SetString("PlayerName", text.Length > 0 ? text.Trim() : randomName);
    }
    
    public void OnJoinButton()
    {
        joinMenu.SetActive(true);
        createMenu.SetActive(false);
    }

    public void OnCreateButton()
    {
        joinMenu.SetActive(false);
        createMenu.SetActive(true);
    }

    public void OnQuitButton()
    {
        Application.Quit();
        Debug.Log("Player has quit the game!");
    }

    public void OnBackButton()
    {
        joinMenu.SetActive(false);
        createMenu.SetActive(false);
    }
    
    public void OnJoinConfirm()
    {
        string code = roomCodeInput.text.Trim().ToUpper();
        GolfNetworkManager.Instance.JoinLobby(code);
    }
    
    public void OnCreateConfirm()
    {
        int holes = int.Parse(holesInput.text.Trim());
        PlayerPrefs.SetInt("holes", holes);
        int maxPlayers = int.Parse(maxPlayersInput.text.Trim());
        GolfNetworkManager.Instance.CreateLobby(maxPlayers);
    }
}
