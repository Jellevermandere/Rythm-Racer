using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public CarController player;
    public PatternGenerator pattern;

    private Path path;

    public Vector3 offset;
    public int lookAhead;
    

    // Start is called before the first frame update
    void Start()
    {
        path = GameObject.FindGameObjectWithTag("TrackManager").GetComponent<Path>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (pattern.playerPointNr < path.points.Count -2 - lookAhead)
        {
            lookToNextPoint();
        }
    }
    public void lookToNextPoint()
    {
        Vector3 lastpoint = path.points[pattern.playerPointNr + lookAhead].Position;
        Vector3 nextPoint = path.points[pattern.playerPointNr + 1 + lookAhead].Position ;
        

        
        transform.LookAt(Vector3.Slerp(lastpoint, nextPoint, pattern.playerPosAlongTick),player.gameObject.transform.up);

    }
}
