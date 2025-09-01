using UnityEngine;
using System;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public enum GameState { MainMenu, Playing, GameOver }
    public GameState CurrentState { get; private set; }
    public static event Action<GameState> OnStateChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            CurrentState = GameState.MainMenu;
        }
    }

    private void Start()
    {
        OnStateChanged?.Invoke(CurrentState);
    }

    public void UpdateState(GameState newState)
    {
        if (CurrentState == newState) return;
        CurrentState = newState;
        OnStateChanged?.Invoke(newState);
        Debug.Log("Updated state: " + newState);
    }

    // Called by start btn, instant.
    public void StartGame()
    {
        UpdateState(GameState.Playing);
    }

    public void EndGame()
    {
        UpdateState(GameState.GameOver);
    }

    // Called by continue btn after game over.
    public void GoToMainMenu()
    {
        if (CurrentState == GameState.GameOver)
        {
            UpdateState(GameState.MainMenu);
        }
    }
}

// @Author F.B.