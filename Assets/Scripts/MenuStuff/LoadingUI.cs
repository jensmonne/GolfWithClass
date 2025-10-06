using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingUI : MonoBehaviour
{
    [SerializeField] private Animator golfball;
    [SerializeField] private Animator loadingCircle;
    [SerializeField] private GameObject loadingImage;

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

    private void Start()
    {
        //Hide();
        Debug.Log($"Instance = {Instance}, Golfball = {golfball}, LoadingCircle = {loadingCircle}");
    }

    //Start loading animation
    public void Show()
    {
        loadingImage.SetActive(true);
        golfball.SetTrigger("Loading");
        loadingCircle.SetTrigger("Loading");
    }

    //stop loading animation
    public void Hide()
    {
        loadingImage.SetActive(false);
    }

    //start zoom in animation
    public void AnimationSceneSwitchIn()
    {
        golfball.SetTrigger("Zoomin");
        loadingCircle.gameObject.SetActive(false);
    }

    //start zoom out animation
    public void AnimationSceneSwitchOut()
    {
        golfball.SetTrigger("Zoomout");
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
