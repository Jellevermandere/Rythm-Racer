using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    private Path path;
    private PatternGenerator pattern;

    private int pointNr;
    private float playerTimer = 0f;


    // Start is called before the first frame update
    void Start()
    {
        path = GameObject.FindGameObjectWithTag("TrackManager").GetComponent<Path>();
        pattern = path.gameObject.GetComponent<PatternGenerator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        playerTimer += Time.deltaTime;
        if(playerTimer >= 60f/pattern.bpm)
        {
            moveToPoint(2);
            playerTimer = 0f;
        }


    }


    void moveToPoint(int amount)
    {
        
        pointNr += amount;
        if (pointNr > path.points.Count - 1)
        {
            Debug.Log("Finish");
        }
        else
        {
            transform.position = path.points[pointNr].Position;
            transform.rotation = path.points[pointNr].Rotation;

        }
    }
}
