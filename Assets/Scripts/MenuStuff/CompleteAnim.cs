using UnityEngine;

public class AnimationEventForwarder : MonoBehaviour
{
    public static AnimationEventForwarder Instance;
    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Called by animation event "Zoonin"
    public void OnZoomInComplete()
    {
        LoadingUI.Instance.OnZoomInComplete();
    }

    // Called by animation event "Zoomout"
    public void OnZoomOutComplete()
    {
        LoadingUI.Instance.OnZoomOutComplete();
    }
}
