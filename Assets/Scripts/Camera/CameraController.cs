using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [Header("Targets")]
    [SerializeField] private Transform playerTarget;
    [SerializeField] private Transform menuPosition;
    [Tooltip("Transition to menu in seconds.")]
    [SerializeField] private float transitionDuration = 1.5f;

    [Header("Settings")]
    [Tooltip("Default distance and height from player.")]
    [SerializeField] private Vector3 gameplayOffset = new Vector3(0, 8, -10);
    [Tooltip("Speed of camera orbit.")]
    [SerializeField] private float rotationSpeed = 120f;
    [Tooltip("Speed of camera movement to target.")]
    [SerializeField] private float moveSpeed = 10f;

    [Header("Tilt controls")]
    [SerializeField][Range(0.1f, 5f)] private float tiltSensitivity = 1.0f;
    [SerializeField][Range(0f, 0.5f)] private float deadZone = 0.1f;

    private Coroutine _transitionCorourtine;

    private void Start()
    {
        // Snap to menu position on start.
        transform.position = menuPosition.position;
        transform.rotation = menuPosition.rotation;
    }

    private void OnEnable()
    {
        GameManager.OnStateChanged += HandleGameStateChanged;
    }

    private void OnDisable()
    {
        GameManager.OnStateChanged -= HandleGameStateChanged;
    }

    private void HandleGameStateChanged(GameManager.GameState state)
    {
        // Snap to meu when not playing.
        if (state != GameManager.GameState.Playing)
        {

            // Stop current transitions.
            if (_transitionCorourtine != null)
            {
                StopCoroutine(_transitionCorourtine);
            }

            _transitionCorourtine = StartCoroutine(TransitionTo(menuPosition, transitionDuration));
        }
    }

    private IEnumerator TransitionTo(Transform target, float duration)
    {
        float elapsedTime = 0f;
        Vector3 startingPos = transform.position;
        Quaternion startingRot = transform.rotation;

        while (elapsedTime < duration)
        {

            float t = Mathf.SmoothStep(0f, 1f, elapsedTime / duration);

            transform.position = Vector3.Lerp(startingPos, target.position, t);
            transform.rotation = Quaternion.Slerp(startingRot, target.rotation, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = target.position;
        transform.rotation = target.rotation;

        _transitionCorourtine = null;
    }

    private void LateUpdate()
    {
        if (GameInputManager.Instance == null || GameManager.Instance.CurrentState != GameManager.GameState.Playing)
        {
            return;
        }

        float finalHorizontalInput = 0f;

        Vector2 keyboardInput = GameInputManager.Instance.MoveInput;
        Vector3 relativeTilt = GameInputManager.Instance.RelativeTilt;

        if (Mathf.Abs(keyboardInput.x) > deadZone)
        {
            finalHorizontalInput = keyboardInput.x;
        }
        else if (Mathf.Abs(relativeTilt.x) > deadZone)
        {
            finalHorizontalInput = relativeTilt.x * tiltSensitivity;
        }

        finalHorizontalInput = Mathf.Clamp(finalHorizontalInput, -1f, 1f);

        // Rotation
        // Quaternion based on gorizopntal input.
        Quaternion turnAngle = Quaternion.AngleAxis(finalHorizontalInput * rotationSpeed * Time.deltaTime, Vector3.up);
        // Apply to camera's offset vector.
        gameplayOffset = turnAngle * gameplayOffset;

        // Position
        // Desired position.
        Vector3 desiredPosition = playerTarget.position + gameplayOffset;
        // Smoothly move camera to desired position.
        transform.position = Vector3.Slerp(transform.position, desiredPosition, moveSpeed * Time.deltaTime);

        // Look at player GO.
        transform.LookAt(playerTarget);
    }
}

// @Author F.B.