using System.Collections;
using UnityEngine;

public class QuickTest : MonoBehaviour
{
    [SerializeField] private bool isDebugSession = false;
    [SerializeField] private int maxPlayers = 2;
    [SerializeField] private int holes = 2;

#if UNITY_EDITOR
    private void Start()
    {
        if (isDebugSession)
        {
            StartCoroutine(WaitForAuthThenLobby());
        }
    }
    private IEnumerator WaitForAuthThenLobby()
    {
        while (!UnityAuthInitializer.IsAuthenticated)
        {
            yield return null;
        }

        AutoLobby();
    }

    private void AutoLobby()
    {
        PlayerPrefs.SetInt("holes", holes);
        GolfNetworkManager.Instance.CreateLobby(maxPlayers, () =>
        {
            GolfNetworkManager.Instance.StartGame();
        });
    }
#endif
}
