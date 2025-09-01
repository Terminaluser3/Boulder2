using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 800f;
    [SerializeField][Range(0.1f, 5f)] private float tiltSensitivity = 1.5f;
    [SerializeField][Range(0f, 0.5f)] private float deadZone = 0.1f;

    [Header("References")]
    [Tooltip("Assign main camera from scene.")]
    [SerializeField] private Camera mainCamera;

    [Header("Reset Settings")]
    [SerializeField] private float resetDuration = 1.5f;

    private Rigidbody rb;
    private Vector3 startPosition;
    private Quaternion startRotation;

    private void OnEnable()
    {
        GameManager.OnStateChanged += HandleStateChage;
    }

    private void OnDisable()
    {
        GameManager.OnStateChanged -= HandleStateChage;
    }

    private void HandleStateChage(GameManager.GameState state)
    {
        // switch statement to stay uniform across classes handlers.
        switch (state)
        {
            case GameManager.GameState.GameOver:
                StartCoroutine(SmoothResetRoutine());
                break;
            default:
                return;
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    public IEnumerator SmoothResetRoutine()
    {
        Debug.Log("SmothResetRoutine");

        rb.isKinematic = true;
        float elapsedTime = 0f;
        Vector3 currentPos = transform.position;
        Quaternion currentRot = transform.rotation;

        while (elapsedTime < resetDuration)
        {
            transform.position = Vector3.Lerp(currentPos, startPosition, elapsedTime / resetDuration);
            transform.rotation = Quaternion.Slerp(currentRot, startRotation, elapsedTime / resetDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = startPosition;
        transform.rotation = startRotation;
        rb.isKinematic = false;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance == null || GameManager.Instance.CurrentState != GameManager.GameState.Playing)
        {
            return;
        }

        Vector2 finalMoveInput = Vector2.zero;
        Vector2 keyboardInput = GameInputManager.Instance.MoveInput;
        Vector3 relativeTilt = GameInputManager.Instance.RelativeTilt;

        if (keyboardInput.sqrMagnitude > deadZone * deadZone)
        {
            finalMoveInput = keyboardInput;
        }
        else
        {
            if (relativeTilt.sqrMagnitude > deadZone * deadZone)
            {
                finalMoveInput = new Vector2(relativeTilt.x, relativeTilt.y) * tiltSensitivity;
            }
        }

        finalMoveInput = Vector2.ClampMagnitude(finalMoveInput, 1f);

        Vector3 camForward = mainCamera.transform.forward;
        Vector3 camRight = mainCamera.transform.right;

        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDirection = (camForward * finalMoveInput.y + camRight * finalMoveInput.x).normalized;

        rb.AddForce(moveDirection * moveSpeed * Time.fixedDeltaTime);

    }
}

// @Author F.B.