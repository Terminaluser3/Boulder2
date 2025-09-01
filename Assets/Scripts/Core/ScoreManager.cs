using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }
    public int Score { get; private set; }
    public static event Action<int> OnScoreChanged;

    private const string HighScoreKey = "HighScores";
    private List<int> highScores = new List<int>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            LoadHighScores();
        }
    }

    private void OnEnable()
    {
        GameManager.OnStateChanged += HandleStateChange;
    }

    private void OnDisable()
    {
        GameManager.OnStateChanged -= HandleStateChange;
    }

    private void HandleStateChange(GameManager.GameState state)
    {
        switch (state)
        {
            case GameManager.GameState.GameOver:
                SaveCurrentScore();
                break;
            case GameManager.GameState.MainMenu:
                ResetScore();
                break;
            case GameManager.GameState.Playing:
                break;
        }
    }

    public void AddScore(int amount)
    {
        Score += amount;
        OnScoreChanged?.Invoke(Score);
    }

    private void ResetScore()
    {
        Score = 0;
        OnScoreChanged?.Invoke(Score);
    }

    public void SaveCurrentScore()
    {
        highScores.Add(Score);
        // Sort scors and keep top 5.
        highScores = highScores.OrderByDescending(s => s).Take(5).ToList();

        // PlayerPrefs can't store list, so convert to single-comma-seperated-string.
        string scoreString = string.Join(",", highScores);
        PlayerPrefs.SetString(HighScoreKey, scoreString);
        PlayerPrefs.Save();
    }

    private void LoadHighScores()
    {
        if (PlayerPrefs.HasKey(HighScoreKey))
        {
            string scoreString = PlayerPrefs.GetString(HighScoreKey);
            if (!string.IsNullOrEmpty(scoreString))
            {
                highScores = scoreString.Split(',').Select(int.Parse).ToList();
            }
        }
    }

    public List<int> GetHighScores()
    {
        return highScores;
    }
}

// @Author F.B.