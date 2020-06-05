using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public PatternGenerator pattern;
    public CurveNoteGeneration curveRight;
    public CurveNoteGeneration curveLeft;
    public CurveNoteGeneration curveUp;
    public CurveNoteGeneration curveDown;
    public GameManager gm;

    public AudioSource missSound;
    public AudioSource starSound;
    public AudioSource hitSound;

    public ParticleSystem stars;
    public ParticleSystem hit;

    public CarController player;
    [Range(0,1)]
    public float maxMistake;

    public float offset;

    public int nrOfNotesHit;
    public int nrOfNotesMissed;

    private CurveNoteGeneration targetCurve;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
        if (pattern.musicPointNr > 0)
        {
            NoteMissed();
        }
        
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
            targetCurve = curveLeft;
        }
        if (Input.GetButton("Down"))
        {
            notePressed = 0;
            targetCurve = curveDown;
        }
        if (Input.GetButton("Right"))
        {
            notePressed = 4;
            targetCurve = curveRight;
        }
        if (Input.GetButton("Up"))
        {
            notePressed = 5;
            targetCurve = curveUp;
        }
        curveRight.UpdateMaterial(false);
        curveLeft.UpdateMaterial(false);
        curveUp.UpdateMaterial(false);
        curveDown.UpdateMaterial(false);

        if (notePressed != -10)
        {
           
            HasCurveAround(notePressed);
            
        }
        else stars.Stop();
    }

    void HasNoteAround()
    {
        if (pattern.musicStructure[pattern.musicPointNr].hasNote == true && pattern.musicStructure[pattern.musicPointNr].isHit == false)
        {
            if (pattern.playerPosAlongTick < maxMistake + offset)
            {
                Debug.Log("late by : " + pattern.playerPosAlongTick);
                pattern.musicStructure[pattern.musicPointNr].isHit = true;
                nrOfNotesHit++;
                hit.Play(withChildren: false);
                hitSound.Play();
                gm.IncreaseScore();
            }

        }
        if (pattern.musicStructure[(pattern.musicPointNr + 1)].hasNote == true && pattern.musicStructure[pattern.musicPointNr+1].isHit == false)
        {
            if (pattern.playerPosAlongTick > (1 - (maxMistake + offset)))
            {
                Debug.Log("early by : " + (1 - pattern.playerPosAlongTick));
                pattern.musicStructure[pattern.musicPointNr+1].isHit = true;
                nrOfNotesHit++;
                hit.Play(withChildren: false);
                hitSound.Play();
                gm.IncreaseScore();
            }

        }
        if(!pattern.musicStructure[pattern.musicPointNr].hasNote && !pattern.musicStructure[(pattern.musicPointNr + 1)].hasNote)
        {
            missSound.Play();
            gm.ResetMultiplier();
        }
        
    }
    void HasCurveAround(int key)
    {
        if (pattern.musicStructure[pattern.musicPointNr].Guitar == key)
        {
            pattern.musicStructure[pattern.musicPointNr].isHit = true;

            targetCurve.UpdateMaterial(true);
            gm.IncreaseMultiplier();
            if (!stars.isPlaying)
            {
                stars.Play();
               
            }
            if (!starSound.isPlaying)
            {
                starSound.Play();
                
            }
            

        }
        if (pattern.musicStructure[(pattern.musicPointNr + 1)].Guitar == key)
        {
            pattern.musicStructure[pattern.musicPointNr].isHit = true;
            targetCurve.UpdateMaterial(true);
            gm.IncreaseMultiplier();
            if (!stars.isPlaying)
            {
                stars.Play();
               
            }
            if (!starSound.isPlaying)
            {
                starSound.Play();
               
            }

        }
        if(pattern.musicStructure[pattern.musicPointNr].Guitar != key && pattern.musicStructure[(pattern.musicPointNr + 1)].Guitar != key)
        {
            curveRight.UpdateMaterial(false);
            curveLeft.UpdateMaterial(false);
            curveUp.UpdateMaterial(false);
            curveDown.UpdateMaterial(false);
            stars.Stop();
            starSound.Stop();
        }
    }

    bool NoteMissed()
    {
        if(pattern.musicStructure[pattern.musicPointNr-1].hasNote == true)
        {
            if(pattern.musicStructure[pattern.musicPointNr - 1].isHit == false)
            {
                nrOfNotesMissed++;
                gm.ResetMultiplier();
                missSound.Play();
                Debug.Log("noteMissed");

                pattern.musicStructure[pattern.musicPointNr - 1].isHit = true;
                return true;
            }
        }
        return false;
    }
    
}
