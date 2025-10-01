using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ShotController : NetworkBehaviour
{
    [Header("Shot")]
    [SerializeField] Rigidbody ballRigidbody;
    [SerializeField] float minPower = 5f;
    [SerializeField] float maxPower = 30f;
    [SerializeField] float powerMultiplier = 0.05f;

    [Header("UI")]
    [SerializeField] Image powerBar; // fill image

    [SerializeField] private Camera camera;

    // runtime
    private PlayerInput playerInput;
    private InputAction clickAction;
    private InputAction pointAction;

    private Vector2 dragStartPos;
    private float shotPower;
    private bool isDragging;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        clickAction  = playerInput?.actions?.FindAction("Click");
        pointAction  = playerInput?.actions?.FindAction("Point");

        if (clickAction == null) Debug.LogWarning("Click action not found. Make sure action name is 'Click'.");
        if (pointAction == null) Debug.LogWarning("Point action not found. Make sure action name is 'Point'.");
    }

    private void OnEnable()
    {
        if (clickAction != null)
        {
            clickAction.started += OnClickStarted;
            clickAction.canceled += OnClickReleased;
        }
    }

    private void OnDisable()
    {
        if (clickAction != null)
        {
            clickAction.started -= OnClickStarted;
            clickAction.canceled -= OnClickReleased;
        }
    }

    public override void OnStartLocalPlayer()
    {
        if (!camera) Debug.LogError("Main camera missing on prefab.");
        if (powerBar != null) powerBar.fillAmount = 0f;
    }

    private void Update()
    {
        if (!isLocalPlayer || !isDragging) return;
        if (pointAction == null) return;
        
        Vector3 camForward = camera.transform.forward;
        camForward.y = 0f;
        if (camForward.sqrMagnitude > 0.001f)
            transform.rotation = Quaternion.LookRotation(camForward);

        Vector2 currentPos = pointAction.ReadValue<Vector2>();
        float deltaY = currentPos.y - dragStartPos.y;

        if (deltaY > 0f)
        {
            shotPower = Mathf.Clamp(deltaY * powerMultiplier, minPower, maxPower);
        }
        else
        {
            shotPower = 0f;
        }

        float normalizedPower = (shotPower - minPower) / (maxPower - minPower);
        if (powerBar != null) powerBar.fillAmount = Mathf.Clamp01(normalizedPower);
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

        Vector3 dir = transform.forward; 
        ShootBall(shotPower, dir);
        shotPower = 0f;
    }

    private void ShootBall(float power, Vector3 direction)
    {
        Debug.Log("SHOT FIRED, GET DOWN GENERAL");
        if (ballRigidbody == null) return;
        ballRigidbody.linearVelocity = Vector3.zero;
        ballRigidbody.angularVelocity = Vector3.zero;
        ballRigidbody.AddForce(direction.normalized * power, ForceMode.Impulse);
    }
}
