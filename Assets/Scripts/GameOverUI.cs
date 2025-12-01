using TMPro;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    public TextMeshProUGUI scoreText; // Reference to your score UI text
    public TextMeshProUGUI highScoreText; // Reference to your high score UI text

    private void Start()
    {
        float finalScore = PlayerPrefs.GetFloat("CurrentScore", 0);
        float highScore = PlayerPrefs.GetFloat("HighScore", 0);

        scoreText.text = "SCORE: " + Mathf.RoundToInt(finalScore).ToString();
        highScoreText.text = "BEST: " + Mathf.RoundToInt(highScore).ToString();

        PlayerPrefs.DeleteKey("CurrentScore");

    }
}
