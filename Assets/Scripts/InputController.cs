using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public PatternGenerator pattern;
    public CurveNoteGeneration curve;
    public CarController player;
    [Range(0,1)]
    public float maxMistake;

    public float offset;

    public int nrOfNotesHit;
    public int nrOfNotesMissed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
        NoteMissed();
    }

    void CheckInput()
    {
        if (Input.GetButtonDown("Jump"))
        {
            HasNoteAround();
        }
        int notePressed = -10;
        if (Input.GetButton("Left"))
        {
            notePressed = 2;
        }
        if (Input.GetButton("Down"))
        {
            notePressed = 0;
        }
        if (Input.GetButton("Right"))
        {
            notePressed = 4;
        }
        if (Input.GetButton("Up"))
        {
            notePressed = 5;
        }

        if(notePressed != -10)
        {
            HasCurveAround(notePressed);
            
        }
        else
        {
            curve.UpdateMaterial(false);
        }
    }

    void HasNoteAround()
    {
        if (pattern.musicStructure[pattern.musicPointNr].hasNote == true)
        {
            if (pattern.playerPosAlongTick < maxMistake + offset)
            {
                Debug.Log("late by : " + pattern.playerPosAlongTick);
                pattern.musicStructure[pattern.musicPointNr].isHit = true;
                nrOfNotesHit++;
            }

        }
        if (pattern.musicStructure[(pattern.musicPointNr + 1)].hasNote == true)
        {
            if (pattern.playerPosAlongTick > (1 - (maxMistake + offset)))
            {
                Debug.Log("early by : " + (1 - pattern.playerPosAlongTick));
                pattern.musicStructure[pattern.musicPointNr].isHit = true;
                nrOfNotesHit++;

            }

        }
    }
    void HasCurveAround(int key)
    {
        if (pattern.musicStructure[pattern.musicPointNr].Guitar == key)
        {
            pattern.musicStructure[pattern.musicPointNr].isHit = true;
            curve.UpdateMaterial(true);


        }
        else if (pattern.musicStructure[(pattern.musicPointNr + 1)].Guitar == key)
        {
            pattern.musicStructure[pattern.musicPointNr].isHit = true;
            curve.UpdateMaterial(true);


        }
        else curve.UpdateMaterial(false);
    }

    bool NoteMissed()
    {
        if(pattern.musicStructure[pattern.musicPointNr-1].hasNote == true || pattern.musicStructure[pattern.musicPointNr - 1].Guitar != -10)
        {
            if(pattern.musicStructure[pattern.musicPointNr - 1].isHit == false)
            {
                nrOfNotesMissed++;

                Debug.Log("noteMissed");
                pattern.musicStructure[pattern.musicPointNr - 1].isHit = true;
                return true;
            }
        }
        return false;
    }
    
}
