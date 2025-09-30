using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GolfShotController : NetworkBehaviour
{
    [Header("Shot")]
    [SerializeField] Rigidbody ballRigidbody;
    [SerializeField] float minPower = 5f;
    [SerializeField] float maxPower = 30f;
    [SerializeField] float powerMultiplier = 0.1f;

    [Header("UI")]
    [SerializeField] Image powerBar; // fill image

    // runtime
    private PlayerInput playerInput;
    private InputAction clickAction;
    private InputAction pointAction;

    private Vector2 dragStartPos;
    private float shotPower;
    private bool isDragging;
    private Camera mainCam;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        if (playerInput == null) Debug.LogError("PlayerInput missing on prefab.");

        // Find actions by name (these names must match the actions in your asset)
        clickAction  = playerInput?.actions?.FindAction("Click");
        pointAction  = playerInput?.actions?.FindAction("Point");

        if (clickAction == null) Debug.LogWarning("Click action not found. Make sure action name is 'Click'.");
        if (pointAction == null) Debug.LogWarning("Point action not found. Make sure action name is 'Point'.");
    }

    void OnEnable()
    {
        if (clickAction != null)
        {
            clickAction.started += OnClickStarted;
            clickAction.canceled += OnClickReleased;
        }
    }

    void OnDisable()
    {
        if (clickAction != null)
        {
            clickAction.started -= OnClickStarted;
            clickAction.canceled -= OnClickReleased;
        }
    }

    public override void OnStartLocalPlayer()
    {
        mainCam = Camera.main;
        if (powerBar != null) powerBar.fillAmount = 0f;

        // Optional: ensure the PlayerInput's action map is enabled for this player
        // playerInput?.ActivateInput();
    }

    void Update()
    {
        if (!isLocalPlayer || !isDragging) return;
        if (pointAction == null) return;

        Vector2 currentPos = pointAction.ReadValue<Vector2>();
        float dragDistance = (currentPos.y - dragStartPos.y) * -1f; // down = positive
        shotPower = Mathf.Clamp(dragDistance * powerMultiplier, minPower, maxPower);

        float normalizedPower = (shotPower - minPower) / (maxPower - minPower);
        if (powerBar != null) powerBar.fillAmount = normalizedPower;
    }

    private void OnClickStarted(InputAction.CallbackContext ctx)
    {
        if (!isLocalPlayer || pointAction == null) return;
        dragStartPos = pointAction.ReadValue<Vector2>();
        isDragging = true;
    }

    private void OnClickReleased(InputAction.CallbackContext ctx)
    {
        if (!isLocalPlayer || !isDragging) return;
        isDragging = false;
        if (powerBar != null) powerBar.fillAmount = 0f;

        // direction choice: camera forward for now (flatten if you want)
        Vector3 dir = mainCam.transform.forward;
        CmdShoot(shotPower, dir);
        shotPower = 0f;
    }

    [Command]
    private void CmdShoot(float power, Vector3 direction)
    {
        if (ballRigidbody == null) return;
        ballRigidbody.linearVelocity = Vector3.zero;
        ballRigidbody.angularVelocity = Vector3.zero;
        ballRigidbody.AddForce(direction.normalized * power, ForceMode.Impulse);
    }
}
