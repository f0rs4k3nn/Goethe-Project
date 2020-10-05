using System;
using UnityEngine;
using UnityEngine.UI;

public class ScoringSystem : MonoBehaviour
{
    public GameObject scoreText;

    private void Awake()
    {
        GameManager.Instance.ScoringSystem = this;
        UpdateScoreText(Score);
    }
    
    public int Score
    {
        get { return PlayerPrefs.GetInt("Points", 0); }
        set { PlayerPrefs.SetInt("Points", value); UpdateScoreText(value);}
    }

    void UpdateScoreText(int value)
    {
        scoreText.GetComponent<Text>().text = "SCORE: " + value;
    }
}
