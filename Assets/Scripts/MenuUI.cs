using UnityEngine;

public class MenuUI : MonoBehaviour
{
    [SerializeField] private GameObject joinMenu;
    [SerializeField] private GameObject createMenu;
    
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
}
