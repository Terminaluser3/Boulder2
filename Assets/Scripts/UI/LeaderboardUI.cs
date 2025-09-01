using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class LeaderboardUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreDisplayText;

    // Automatically called on active GO.
    private void Start()
    {
        HandleLeaderboardUISync();
    }

    private void OnEnable()
    {
        HandleLeaderboardUISync();
    }

    private void HandleLeaderboardUISync()
    {
        if (scoreDisplayText == null || ScoreManager.Instance == null) return;

        List<int> scores = ScoreManager.Instance.GetHighScores();
        if (scores == null || scores.Count == 0)
        {
            scoreDisplayText.text = "No scores yet.";
            return;
        }

        // Build string for display.
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        for (int i = 0; i < scores.Count; i++)
        {
            sb.AppendLine((i + 1) + ".   " + scores[i]);
        }
        scoreDisplayText.text = sb.ToString();
    }
}

// @Author F.B.