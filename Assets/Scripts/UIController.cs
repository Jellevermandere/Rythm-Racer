using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Text scoreText;
    public Text noteStreak;
    public Text curveMultiplier;

    public GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = Mathf.Round(gm.Score).ToString();
        noteStreak.text = "X " + gm.noteMultiplier.ToString();
        curveMultiplier.text = "x " + (Mathf.Round(gm.currentMultiplier * 10)/10f).ToString();


    }
}
