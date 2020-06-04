using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Path))]

public class ObjectSpawner : MonoBehaviour
{
    public GameObject note;
    public GameObject startLine;
    

    private Path path;
    private PatternGenerator pattern;

    // Start is called before the first frame update
    void Start()
    {
        path = GetComponent<Path>();
        pattern = GetComponent<PatternGenerator>();

        spawnNote();
        SetStartLine();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void spawnNote()
    {
        for (int i = 0; i < path.points.Count/(float) pattern.playerSpeed; i++)
        {
            if(pattern.musicStructure[i].hasNote == true )
            {
                Instantiate(note, path.points[i * pattern.playerSpeed].Position, path.points[i * pattern.playerSpeed+1].Rotation);
                
            }
        }
    }

    void SetStartLine()
    {
        Instantiate(startLine, path.points[pattern.startOffset*pattern.playerSpeed].Position, path.points[pattern.startOffset * pattern.playerSpeed].Rotation);
        Instantiate(startLine, path.points[path.points.Count-1- pattern.startOffset * pattern.playerSpeed].Position, path.points[path.points.Count - 1 - pattern.startOffset * pattern.playerSpeed].Rotation);
    }
}
