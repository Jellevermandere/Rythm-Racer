using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //public static float inputDelay;

    public float bpm = 120;

    [Header("Sounds")]
    [SerializeField]
    private AudioSource drum;
    [SerializeField]
    private AudioSource kick;
    [SerializeField]
    private AudioSource bassC;
    [SerializeField]
    private AudioSource guitarC;
    public float beatMultiplier;



    private float musicTimer;
    private float drumInterval;
    


    // Start is called before the first frame update
    void Start()
    {
        musicTimer = 0;
        drumInterval = 60 / bpm;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        musicTimer += Time.deltaTime;
        PlaySound(drum, beatMultiplier);
        //PlaySound(kick, beatMultiplier);

    }

    void PlaySound(AudioSource source, float interval)
    {
        drumInterval += Time.deltaTime;
        if(drumInterval >= 60 / (bpm * interval))
        {
            source.Play();
            Debug.Log("beat " + drumInterval);
            drumInterval = 0f;
        }
    }

    void PlayNote(AudioSource source, int startOffset, int nrSkips, float currTimer)
    {
        
    }

    void UpdateTimers()
    {

    }
}
