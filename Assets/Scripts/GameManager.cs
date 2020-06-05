using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public float bpm = 120;
    public static float highScore;
    public static float lastScore;
    public static int notesMissed;

    public float Score;
    public float curveMultiplier;
    public int noteStreak;
    public int noteMultiplier;

    public float currentMultiplier;


    // Start is called before the first frame update
    void Start()
    {
        currentMultiplier = 1;
        noteMultiplier = 1;
        UnPauseGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Finish()
    {
        if(Score > highScore)
        {
            highScore = Score;
        }
        lastScore = Score;

        Invoke("EndScreen", 2f);

    }
    public void IncreaseMultiplier()
    {
        currentMultiplier += curveMultiplier;
    }

    public void IncreaseScore()
    {
        Score += 1 * currentMultiplier * noteMultiplier;
        noteStreak++;
        noteMultiplier = 1;

        if (noteStreak > 4)
        {
            noteMultiplier = 2;
        }
        if (noteStreak > 8)
        {
            noteMultiplier = 4;
        }
        if (noteStreak > 16)
        {
            noteMultiplier =8;
        }
    }
    public void ResetMultiplier()
    {
        currentMultiplier = 1;
        noteStreak = 1;
        noteMultiplier = 1;

    }

    void EndScreen()
    {
        SceneManager.LoadScene(4);
    }

    public void StartGame( int nr)
    {
        SceneManager.LoadScene(nr);
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }
    public void UnPauseGame()
    {
        Time.timeScale = 1f;
    }

    public void StartScreen()
    {
        SceneManager.LoadScene(0);
    }
}
