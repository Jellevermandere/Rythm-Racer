using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// generates a sequence of values, the corners, loopings and beats

[RequireComponent(typeof(Path))]
[RequireComponent(typeof(TerrainGeneration))]

public class PatternGenerator : MonoBehaviour
{
    private Path path;
    private TerrainGeneration terrainGeneration;

    public float roadSideOffset;
    public float bpm = 120f;
    public float stepDistance;


    // Start is called before the first frame update
    void Start()
    {
        path = GetComponent<Path>();
        terrainGeneration = GetComponent<TerrainGeneration>();

        CreatePattern();
    }

    // Update is called once per frame
    void Update()
    {
        CreatePatternInput();
    }

    

    void CreatePattern()
    {
        path.GenerateStartPoints(stepDistance);

        for (int i = 0; i < 5; i++)
        {
            path.AddStraight(stepDistance);
        }
        path.AddCorner(90, 20, 0, stepDistance);
        for (int i = 0; i < 5; i++)
        {
            path.AddStraight(stepDistance);
        }
        path.AddDownFall(3, 20, stepDistance);
        for (int i = 0; i < 5; i++)
        {
            path.AddStraight(stepDistance);
        }
        path.AddLooping(20, roadSideOffset, stepDistance);

        terrainGeneration.UpdateRoad(stepDistance);
    }

    // for debugging
    void CreatePatternInput()
    {
        if (Input.GetKeyDown("space"))
        {
            path.AddStraight(stepDistance);
            terrainGeneration.UpdateRoad(stepDistance);
        }
        if (Input.GetKeyDown("right"))
        {
            path.AddCorner(90, 20, 0, stepDistance);
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
