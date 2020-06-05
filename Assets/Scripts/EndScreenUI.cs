using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndScreenUI : MonoBehaviour
{
    public Text scoreText;
    public Text highScore;

    // Start is called before the first frame update
    void Start()
    {
        scoreText.text = GameManager.lastScore.ToString() + (GameManager.lastScore == GameManager.highScore ? "! New Highscore!" : "! Can you beat your HighScore?");
        highScore.text = GameManager.highScore.ToString();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
