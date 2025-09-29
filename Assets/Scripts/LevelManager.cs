using System;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class LevelManager : NetworkBehaviour
{
    public static LevelManager Instance;
    
    [SerializeField] private List<string> allLevels = new List<string>();

    [SyncVar] public int currentLevelIndex = -1;
    [SyncVar(hook = nameof(OnLevelListUpdated))]
    private string serializedLevelList;

    private List<string> levelList = new();

    private void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else Destroy(gameObject);
    }

    [Server]
    public void SetupLevelList(int amount)
    {
        List<string> shuffled = new(allLevels);
        Shuffle(shuffled);

        if (amount > shuffled.Count)
            amount = shuffled.Count;

        levelList = shuffled.GetRange(0, amount);
        serializedLevelList = string.Join(";", levelList);
    }

    private void OnLevelListUpdated(string _, string newList)
    {
        levelList = new List<string>(newList.Split(";"));
    }

    [Server]
    public void LoadNextLevel()
    {
        currentLevelIndex++;
        if (currentLevelIndex < levelList.Count)
        {
            string nextScene = levelList[currentLevelIndex];
            NetworkManager.singleton.ServerChangeScene(nextScene);
        }
        else
        {
            Debug.Log("Game finished!");
            // TODO: winner things
            NetworkManager.singleton.ServerChangeScene("Lobby");
        }
    }

    private static void Shuffle<T>(List<T> list)
    {
        System.Random rng = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            int k = rng.Next(n--);
            (list[n], list[k]) = (list[k], list[n]);
        }
    }
}