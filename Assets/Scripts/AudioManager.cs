using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //public static float inputDelay;

  

    [Header("Sounds")]
    [SerializeField]
    private AudioSource drum;
    [SerializeField]
    private AudioSource kick;
    [SerializeField]
    private AudioSource bassC;
    [SerializeField]
    private AudioSource guitarC;
    [SerializeField]
    private AudioSource guitarLoop;
    public List <int> musicScale;

    public PatternGenerator pattern;

    private int LastPitch;


    // Start is called before the first frame update
    void Start()
    {
        LastPitch = -10;
    }


    public void PlaySound(int tickNr)
    {
        //PlayNote(drum, 0);
        if (pattern.musicStructure[tickNr].Drum == 1)
        {
            PlayNote(drum, 0);
        }
        if (pattern.musicStructure[tickNr].Kick == 1)
        {
            PlayNote(kick, 0);
        }
        //Debug.Log(pattern.musicStructure[tickNr].Guitar);
        
        PlayLoopNote(guitarLoop, pattern.musicStructure[tickNr].Guitar);
            //Debug.Log("play");
        
           
        PlayNote(bassC, pattern.musicStructure[tickNr].Bass);

    }

    public void PlayNote(AudioSource source, int pitch)
    {

        if (musicScale.Contains(pitch))
        {
            source.pitch = Mathf.Pow(2, (pitch) / 12.0f);
            source.Play();
        }
        else source.Stop();
        /*  pitch = 0;  // C
            pitch = 2;  // D
            pitch = 4;  // E
            pitch = 5;  // F
            pitch = 7;  // G
            pitch = 9;  // A
            pitch = 11; // B
            pitch = 12; // C
            pitch = 14; // D
        */

        
            
    }
    public void PlayLoopNote(AudioSource source, int pitch)
    {


        if (musicScale.Contains(pitch))
        {
            if(LastPitch != pitch)
            {
                source.pitch = Mathf.Pow(2, (pitch) / 12.0f);
                source.Play();
                LastPitch = pitch;
            }
            
        }
        else source.Stop();
        /*  pitch = 0;  // C
            pitch = 2;  // D
            pitch = 4;  // E
            pitch = 5;  // F
            pitch = 7;  // G
            pitch = 9;  // A
            pitch = 11; // B
            pitch = 12; // C
            pitch = 14; // D
        */



    }


    void UpdateTimers()
    {

    }
}
