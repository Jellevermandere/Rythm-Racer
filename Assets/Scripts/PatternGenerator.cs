using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// generates a sequence of values, the corners, loopings and beats

[RequireComponent(typeof(Path))]
[RequireComponent(typeof(TerrainGeneration))]

public class PatternGenerator : MonoBehaviour
{
    public AudioManager audio;
    private Path path;
    private TerrainGeneration terrainGeneration;
    public CurveNoteGeneration curveNoteGeneration;
    [Header("Path generation settings")]
    public bool endlessMode;
    public int startOffset;
    [Tooltip ("expressed in ticks (1/4 of a beat)")]
    public int levelDuration = 30;
    public float bpm = 120f;
    public float roadSideOffset;
    public float stepDistance;
    [Header ("player settings")]
    public int playerSpeed = 3;

    public float playerPosAlongTick;
    
    public int musicPointNr;
    public int playerPointNr;

    [Header("Random Seed Settings")]
    [Range (0,1)]
    public float drumSeed;
    [Range(0, 1)]
    public float guitarSeed;
    [Range(0, 1)]
    public float bassSeed;
    [Range (0,1)]
    public float drumAmount;
    [Range(0, 0.5f)]
    public float guitarTreshholdCorner;
    [Range(0, 0.5f)]
    public float guitarTreshholdLoop;
    public int minNoteDuration;
    public int maxNoteDuration;

    public float CurrentBpm;


    /* array dimentions: each with 4x4 segments the value indicates a tone shift
     * 0: drum
     * 1: Kick
     * 2: Bass
     * 3: guitar
    */
    public NoteIndex[] musicPatternSegments = new NoteIndex[64];
    public NoteIndex[] musicStructure;


    // Start is called before the first frame update
    void Start()
    {
        CurrentBpm = bpm;
        path = GetComponent<Path>();
        terrainGeneration = GetComponent<TerrainGeneration>();
        musicStructure = new NoteIndex[levelDuration + startOffset*2];
        ResetNotes(musicStructure);

        //CreateKickSequence();
        CreateDrumSequence();
        CreateKickSequence();
        CreateRoadPattern();

    }

    // Update is called once per frame
    void Update()
    {
        //CreatePatternInput();
        
        //CreateBassSequence();


        playerPosAlongTick += Time.deltaTime / (60f / (float)(playerSpeed *4* CurrentBpm));
        if (playerPosAlongTick >= 1)
        {
            playerPosAlongTick = 0f;
            playerPointNr++;
            


            if(playerPointNr % playerSpeed == 0 && musicPointNr < musicStructure.Length-1)
            {
                musicPointNr++;
                //Debug.Log(musicStructure[musicPointNr].Guitar);
                audio.PlaySound(musicPointNr);

            }
            
            
        }
        if(playerPointNr > (musicStructure.Length - startOffset) * playerSpeed )
        {
            
            CurrentBpm = Mathf.Lerp(0, bpm, (musicStructure.Length*playerSpeed - playerPointNr) / (musicStructure.Length * startOffset * playerSpeed));
            
        }
       
    }
    void ResetNotes(NoteIndex[] notes)
    {
        for (int i = 0; i < notes.Length; i++)
        {
            notes[i].Drum = 0;
            notes[i].Kick = 0;
            notes[i].Bass = -1;
            notes[i].Guitar = -10;
            notes[i].hasNote = false;
            notes[i].isHit = false;
            notes[i].Direction = 0;


        }
    }

    void CreateRoadPattern()
    {
        path.GenerateStartPoints(stepDistance, startOffset * playerSpeed);
        for (int i = 0; i < musicStructure.Length-startOffset*2; i++)
        {
            musicStructure[i + startOffset].Drum = musicPatternSegments[i % 16].Drum;
            musicStructure[i + startOffset].Kick = musicPatternSegments[i % 16].Kick;
            musicStructure[i + startOffset].hasNote = musicPatternSegments[i % 16].hasNote;

        }
        int nrOfNotesPlaced = 2;
        int lastnote = -10;
        int typeNote = 0;
        while (nrOfNotesPlaced < musicStructure.Length-1- startOffset*2)
        {
            do
            {
                typeNote = Random.Range(-3, 4);
            } while (typeNote == lastnote);
            
            int noteDuration = 0;

            switch (typeNote)
            {
                case -3:
                    noteDuration = Random.Range(minNoteDuration, maxNoteDuration);
                    typeNote = 0;
                    break;
                case -2:
                    noteDuration = Random.Range(minNoteDuration, maxNoteDuration);
                    typeNote = 2;
                    break;
                case -1:
                    noteDuration = Random.Range(4, maxNoteDuration);
                    typeNote = -1;
                    break;
                case 0:
                    noteDuration = Random.Range(4, maxNoteDuration);
                    typeNote = -1;
                    break;
                case 1:
                    noteDuration = Random.Range(4, maxNoteDuration);
                    typeNote = -1;
                    break;
                case 2:
                    noteDuration = Random.Range(4, maxNoteDuration);
                    typeNote = 4;
                    break;
                case 3:
                    noteDuration = Random.Range(minNoteDuration, maxNoteDuration);
                    typeNote = 5;
                    break;
                default:
                    break;
            }
            if(noteDuration + nrOfNotesPlaced > musicStructure.Length-1- startOffset*2)
            {
                noteDuration = musicStructure.Length-1- startOffset*2 - nrOfNotesPlaced;
            }
            
            for (int i = 0; i < noteDuration; i++)
            {
               
                musicStructure[nrOfNotesPlaced + i + startOffset].Guitar = typeNote;
             
            }
            path.AddSegment(stepDistance, noteDuration * playerSpeed, typeNote, roadSideOffset);
            nrOfNotesPlaced += noteDuration;
            //Debug.Log(typeNote + " " + noteDuration + " " + nrOfNotesPlaced);

        }
        
        path.AddStraight(stepDistance, startOffset * playerSpeed);
        terrainGeneration.UpdateRoad(stepDistance);
        curveNoteGeneration.UpdateRoad(stepDistance);
        Debug.Log(path.points.Count + " " + nrOfNotesPlaced);

    }

    void CreateDrumSequence()
    {
        for (int i = 0; i < 4; i++)
        {

            int NotePos = Random.Range(0, 4);//Mathf.FloorToInt(Mathf.PerlinNoise(i/4, drumSeed) * 6);
            musicPatternSegments[i * 4 + NotePos].Drum = 1;//Random.Range(0, 2); //(i / (float)musicPatternSegments.Length, drumSeed) > drumAmount? 1:0;
            

            //Debug.Log(musicPatternSegments[i].Drum + " " + Mathf.PerlinNoise(i / (float)musicPatternSegments.Length, drumSeed));
        }
    }
    void CreateKickSequence()
    {
        for (int i = 0; i < 4; i++)
        {

            int NotePos = Random.Range(0, 4);//Mathf.FloorToInt(Mathf.PerlinNoise(i/4, drumSeed) * 6);
            musicPatternSegments[i * 4 + NotePos].Kick = 1;//Random.Range(0, 2); //(i / (float)musicPatternSegments.Length, drumSeed) > drumAmount? 1:0;
            musicPatternSegments[i * 4 + NotePos].hasNote = true;

            //Debug.Log(musicPatternSegments[i].Drum + " " + Mathf.PerlinNoise(i / (float)musicPatternSegments.Length, drumSeed));
        }
    }
    void CreateBassSequence()
    {
        for (int i = 0; i < 16; i++)
        {
            musicPatternSegments[i].Bass = Mathf.FloorToInt(Mathf.PerlinNoise(i / (float)musicPatternSegments.Length * Mathf.PI, bassSeed) * 12);
        }
    }
    void CreateGuitarSequence()
    {
       
        for (int i = 0; i < musicPatternSegments.Length; i++)
        {
            float randValue = Mathf.PerlinNoise(i / (float)(musicPatternSegments.Length) , guitarSeed) - 0.5f;
            
            int value = -1;
            if(randValue > guitarTreshholdCorner)
            {
                if (randValue > guitarTreshholdLoop) value = 5 ;
                else value = 4;
            }
            if (randValue < -guitarTreshholdCorner)
            {
                if (randValue < -guitarTreshholdLoop) value = 0;
                else value = 2;
            }


            musicPatternSegments[i].Guitar = value;
            //Debug.Log(randValue + " " + value);
        }
    }

    void CreateStartPattern()
    {
        //path.GenerateStartPoints(stepDistance);

        /*
        
        for (int i = 0; i < 20; i++)
        {
            path.AddStraight(stepDistance,1);
        }
        path.AddCorner(90, 20, 0, stepDistance);
        for (int i = 0; i < 30; i++)
        {
            path.AddStraight(stepDistance,1);
        }
        path.AddDownFall(3, 20, stepDistance);
        for (int i = 0; i < 20; i++)
        {
            path.AddStraight(stepDistance,1);
        }
        path.AddLooping(16, roadSideOffset, stepDistance);

        for (int i = 0; i < 50; i++)
        {
            path.AddStraight(stepDistance,1);
        }
        
        for (int i = 0; i < 64; i++)
        {
            path.AddStraight(stepDistance,1);
        }
        for (int i = 0; i < 20; i++)
        {
            path.AddStraight(stepDistance,1);
        }
        path.AddCorner(90, 20, 0, stepDistance);
        for (int i = 0; i < 30; i++)
        {
            path.AddStraight(stepDistance,1);
        }
        path.AddDownFall(3, 20, stepDistance);
        for (int i = 0; i < 20; i++)
        {
            path.AddStraight(stepDistance,1);
        }
        path.AddLooping(15, roadSideOffset, stepDistance);

        for (int i = 0; i < 50; i++)
        {
            path.AddStraight(stepDistance,1);
        }

        for (int i = 0; i < 64; i++)
        {
            path.AddStraight(stepDistance,1);
        }

        */
        //path.AddSegment

        terrainGeneration.UpdateRoad(stepDistance);
    }


    // for debugging
    void CreatePatternInput()
    {
        if (Input.GetKeyDown("space"))
        {
            path.AddStraight(stepDistance,10);
            terrainGeneration.UpdateRoad(stepDistance);
        }
        if (Input.GetKeyDown("right"))
        {
            path.AddCorner(45, 20, 0, stepDistance);
            terrainGeneration.UpdateRoad(stepDistance);
        }
        if (Input.GetKeyDown("left"))
        {
            path.AddCorner(-90, 20, 0, stepDistance);
            terrainGeneration.UpdateRoad(stepDistance);
        }
        if (Input.GetKeyDown("down"))
        {
            path.AddDownFall(3, 20, stepDistance);
            terrainGeneration.UpdateRoad(stepDistance);
        }
        if (Input.GetKeyDown("up"))
        {
            path.AddLooping(20, roadSideOffset, stepDistance);
            terrainGeneration.UpdateRoad(stepDistance);
        }
    }
}

public struct NoteIndex
{

    public int Drum;
    public int Kick;
    public int Bass;
    public int Guitar;
    public bool hasNote;
    public bool isHit;
    public int Direction;

}
