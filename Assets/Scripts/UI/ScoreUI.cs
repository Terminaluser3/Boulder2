using UnityEngine;
using TMPro;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    private void OnEnable()
    {
        ScoreManager.OnScoreChanged += UpdateScoreText;
        UpdateScoreText(0);
    }

    private void OnDisable()
    {
        ScoreManager.OnScoreChanged -= UpdateScoreText;
    }

    private void Start()
    {
        UpdateScoreText(0);
    }

    private void UpdateScoreText(int score)
    {
        scoreText.text = "Score: " + score;
    }
}

// @Author F.B.