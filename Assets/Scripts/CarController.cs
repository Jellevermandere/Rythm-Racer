using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public Path path;
    private PatternGenerator pattern;


    public int pointNr;
    public float playerPos = 0f;
    public float frontWheelRadius;
    public float rearWheelRadius;
    public float rumbleAmount = 0.1f;
    private float playerTimer = 0f;

    public Transform[] rearWheels;
    public Transform[] frontWheels;
    public Transform car;

    public bool inCorner;

    public float maxRotAngleX;
    public float maxRotAngleY;

    // Start is called before the first frame update
    void Start()
    {
        path = GameObject.FindGameObjectWithTag("TrackManager").GetComponent<Path>();
        pattern = path.gameObject.GetComponent<PatternGenerator>();
        playerPos = 1;
    }

    // Update is called once per frame
   
    // Update is called once per frame
    void Update()
    {

        if (pointNr < path.points.Count - 2)
        {
            moveToPoint(pattern.playerSpeed, pattern.CurrentBpm);
            RotateWheels(pattern.playerSpeed, pattern.CurrentBpm);
        }
        CarRumble(rumbleAmount);

        RotateCar(maxRotAngleX, maxRotAngleY);



    }
    /*
    private void FixedUpdate()
    {
        playerTimer += Time.deltaTime;
        if (playerTimer >= 60f / pattern.bpm)
        {
            moveToPoint(2);
            playerTimer = 0f;
        }
    }
    */

    void moveToPoint(int speed, float bpm)
    {
        if (pattern.playerPointNr < path.points.Count - 2)
        {
            playerPos = pattern.playerPosAlongTick;
            transform.position = Vector3.Slerp(path.points[pattern.playerPointNr].Position, path.points[pattern.playerPointNr + 1].Position, playerPos);
            transform.rotation = Quaternion.Slerp(path.points[pattern.playerPointNr + 1].Rotation, path.points[pattern.playerPointNr + 2].Rotation, playerPos);
        }
        else
        {

        }
        
        
        
    }

    public void RotateCar(float angleX,float angleY)
    {

        car.localEulerAngles = new Vector3(angleX * Input.GetAxis("Vertical"), angleY * Input.GetAxis("Horizontal"), 0);
    }

    void CarRumble(float amount)
    {
        car.localPosition = new Vector3(Random.Range(-1f, 1f) * amount / 10, Random.Range(-1f, 1f) * amount / 10, Random.Range(-1f, 1f) * amount / 10);
    }

    void RotateWheels(int speed, float bpm)
    {
       
        foreach (Transform wheel in rearWheels)
        {
            float rotAngle = pattern.stepDistance / (60f / bpm) * speed / rearWheelRadius;
            wheel.Rotate(rotAngle * Mathf.Rad2Deg, 0f, 0f,Space.Self);
        }

        foreach (Transform wheel in frontWheels)
        {
            float rotAngle = pattern.stepDistance / (60f / bpm) * speed / frontWheelRadius;
            wheel.Rotate(rotAngle * Mathf.Rad2Deg, 0f, 0f, Space.Self);
        }
    }


    void skipToPoint(int amount)
    {

        pointNr += amount;
        if (pointNr > path.points.Count - 1)
        {
            Debug.Log("Finish");
        }
        else
        {
            transform.position = path.points[pattern.playerPointNr].Position;
            transform.rotation = path.points[pattern.playerPointNr].Rotation;

        }
    }

}
