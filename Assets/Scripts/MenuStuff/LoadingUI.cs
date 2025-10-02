using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingUI : MonoBehaviour
{
    [SerializeField] private Animator Golfball;
    [SerializeField] private Animator LoadingCircle;

    public static LoadingUI Instance;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Start()
    {
        Hide();
        Debug.Log($"Instance = {Instance}, Golfball = {Golfball}, LoadingCircle = {LoadingCircle}");
    }

    //Start loading animation
    public void Show()
    {
        gameObject.SetActive(true);
        Golfball.SetTrigger("Loading");
        LoadingCircle.SetTrigger("Loading");
    }

    //stop loading animation
    public void Hide()
    {
        gameObject.SetActive(false);
    }

    //start zoom in animation
    public void AnimationSceneSwitchIn()
    {
        Golfball.SetTrigger("Zoomin");
        LoadingCircle.enabled = false;
    }

    //start zoom out animation
    public void AnimationSceneSwitchOut()
    {
        Golfball.SetTrigger("Zoomout");
    }

    //called by animation event "Zoonin" (In CompleteAnim.cs)
    public void OnZoomInComplete()
    {
        GolfNetworkManager.Instance.OnZoomInComplete();
    }

    //called by animation event "Zoomout" (In CompleteAnim.cs)
    public void OnZoomOutComplete()
    {
        Hide();
    }

    //called when scene is loaded (in GolfNetworkManager.cs)
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AnimationSceneSwitchOut();
    }

}
