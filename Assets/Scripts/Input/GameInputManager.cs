using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class GameInputManager : MonoBehaviour
{
    public static GameInputManager Instance { get; private set; }

    public Vector2 MoveInput { get; private set; }
    public Vector3 RelativeTilt { get; private set; }


    private Vector3 rawTiltInput;
    private Vector3 neutralTiltOffset;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // Enable sensor here.
#if !UNITY_EDITOR && (UNITY_ANDROID) || (UNITY_IOS)
            if (GravitySensor.current != null)
            {
                InputSystem.EnableDevice(GravitySensor.current);
            }
#endif
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
        if (state == GameManager.GameState.Playing)
        {

            // Callibrate before new game.
            CalibrateTiltControls();
        }
    }

    public void CalibrateTiltControls()
    {
        neutralTiltOffset = rawTiltInput;
        Debug.Log("Calibrated tilt: " + neutralTiltOffset);
    }

    public void OnMove(InputAction.CallbackContext callbackContext)
    {
        MoveInput = callbackContext.ReadValue<Vector2>();
    }

    public void OnTilt(InputAction.CallbackContext callbackContext)
    {
        rawTiltInput = callbackContext.ReadValue<Vector3>();
    }

    // Update is called once per frame
    void Update()
    {
        RelativeTilt = rawTiltInput - neutralTiltOffset;
    }
}
