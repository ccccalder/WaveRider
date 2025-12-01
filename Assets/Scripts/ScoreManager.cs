using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{

    public float Score;

    public int scoreMultiplier = 1;
    public float pointsPerSecond;

    public float highScore = 0;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;

    private bool isDead = false;

    public GameObject pointPopupPrefab;

    public Transform canvasTransform;
    public Transform popUpInstantiationPoint;


    private void Start()
    {
        ResetScore();
        highScore = PlayerPrefs.GetFloat("HighScore", 0);
        highScoreText.text = "BEST: " + Mathf.RoundToInt(highScore).ToString();

    }

    private void Update()
    {
        //this whole thing adds points over time
        if (isDead) return;
        Score += pointsPerSecond * Time.deltaTime * scoreMultiplier;
        scoreText.text = "Score: " + Mathf.RoundToInt(Score).ToString();
        if (Score > highScore)
        {
            highScore = Score;
            PlayerPrefs.SetFloat("HighScore", highScore);
            highScoreText.text = "BEST: " + Mathf.RoundToInt(Score).ToString(); ;
        }
    }

    public void AddScore(int amount)
    {
        if (isDead) return;
        Score += amount;
        //updating text
        scoreText.text = "Score: " + Mathf.RoundToInt(Score).ToString();

        if (Score > highScore)
        {
            highScore = Score;
            PlayerPrefs.SetFloat("HighScore", highScore);
            highScoreText.text = "BEST: " + Mathf.RoundToInt(Score).ToString(); ;
        }
    }

    public void SaveFinalScore()
    {
        isDead = true;
        PlayerPrefs.SetFloat("CurrentScore", Score);
        PlayerPrefs.Save();

    }

    public void ResetScore()
    {
        Score = 0;
        scoreText.text = "Score: " + Mathf.RoundToInt(Score).ToString(); ;
    }

    public void ShowPoints(int points)
    {
        
        Vector2 screenPosition = popUpInstantiationPoint.position; 

        GameObject popupInstance = Instantiate(pointPopupPrefab, canvasTransform);

        popupInstance.transform.position = screenPosition;

        PointPopup popupScript = popupInstance.GetComponentInChildren<PointPopup>();

        if (popupScript != null)
        {
            popupScript.Setup(points);
        }
    }
}
