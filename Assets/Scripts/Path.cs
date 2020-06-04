using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Generates a sequence of points along a path

public class Path : MonoBehaviour
{
    [SerializeField]
    private GameObject testSphere;
    public float maxStepAngle = 10;

    public List <AnchorPoint> points = new List<AnchorPoint>();


    // generates the 2 first point of the path
    public void GenerateStartPoints(float step, int amount)
    {
        points.Add(new AnchorPoint() { Position = Vector3.zero,Rotation = Quaternion.identity, Scale = Vector3.one });
        points.Add(new AnchorPoint() { Position = Vector3.forward * step, Rotation = Quaternion.identity, Scale = Vector3.one });
        AddStraight(step,amount - 2);
        
    }

    public void AddSegment(float step, int amount, int direction, float offset)
    {
        if(direction == -1)
        {
            AddStraight(step, amount);
        }
        else if(direction == 2)
        {
            AddCurve(step, amount, -1);
        }
        else if (direction == 4)
        {
            AddCurve(step, amount, 1);
        }
        else if(direction == 0)
        {
            AddDrop(step, amount);
        }
        else if(direction == 5)
        {
            AddLoop(step, amount, offset);
        }
    }

    // adds a piece of track in line with the current last point
    public void AddStraight(float step, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            // adds a new anchorpoint, the rotation Quaternion is used to rotate the up vector
            Vector3 newPos = points[points.Count - 1].Position + points[points.Count - 1].Rotation * Vector3.forward * step;
            Quaternion newRot = points[points.Count - 1].Rotation;

            points.Add(new AnchorPoint() { Position = newPos, Rotation = newRot, Scale = Vector3.one, Straight = true });
        }
    }

    // adds a corner with a variable angle and raduis
    public void AddCurve(float step, int amount, int direction)
    {
        float currAngle = 0f;
        //float currBank = 0f;
        float radius = step * amount / (Mathf.PI/2f);
        float stepAngle = direction * 90 / (float)amount;
       
        //Debug.Log(nrOfSteps);

        AnchorPoint startPoint = points[points.Count - 1];
        AnchorPoint anchorPoint = startPoint;
        anchorPoint.Position += startPoint.Rotation * Vector3.right * radius * direction;

        for (int i = 0; i < amount; i++)
        {
            currAngle += 90 / (float) amount * direction;

            AnchorPoint newPoint = anchorPoint;
            newPoint.Rotation *= Quaternion.AngleAxis(currAngle, Vector3.up);
            newPoint.Position += newPoint.Rotation * Vector3.left * radius * direction;
            newPoint.Straight = false;
            //newPoint.Rotation *= Quaternion.AngleAxis(currBank, Vector3.forward);
            points.Add(newPoint);
            //Debug.Log(newPoint.Rotation.eulerAngles.y);
        }

    }

    // adds a looping of a certain length
    public void AddLoop(float step, int amount, float offset)
    {
        // add the looping with a point offset
        float currAngle = 0f;
        float currOffset = 0f;
        float radius = step * amount / (2 * Mathf.PI);

        //Debug.Log(nrOfSteps);

        AnchorPoint startPoint = points[points.Count - 1];
        AnchorPoint anchorPoint = startPoint;
        anchorPoint.Position += startPoint.Rotation * Vector3.up * radius;

        // perform the DownWards turn
        for (int i = 0; i < amount; i++)
        {
            currAngle += (360f) / (float)amount;
            currOffset += offset / (float)amount;

            AnchorPoint newPoint = anchorPoint;
            newPoint.Rotation *= Quaternion.AngleAxis(currAngle, Vector3.left);
            newPoint.Position += newPoint.Rotation * Vector3.down * radius + newPoint.Rotation * Vector3.left * currOffset;
            newPoint.Straight = false;
            points.Add(newPoint);
            //Debug.Log(newPoint.Rotation.eulerAngles.x);
        }
    }

    // adds a drop of a certain length
    public void AddDrop(float step, int amount)
    {
        float currAngle = 0f;
        int nrOfSteps = Mathf.RoundToInt(90f / maxStepAngle);
        float radius = step * nrOfSteps / (Mathf.PI / 2f);
        //Debug.Log(nrOfSteps);

        AnchorPoint startPoint = points[points.Count - 1];
        AnchorPoint anchorPoint = startPoint;
        anchorPoint.Position += startPoint.Rotation * Vector3.down * radius;

        // perform the DownWards turn
        for (int i = 0; i < nrOfSteps; i++)
        {
            currAngle += (90f) / (float)nrOfSteps;

            AnchorPoint newPoint = anchorPoint;
            newPoint.Rotation *= Quaternion.AngleAxis(currAngle, Vector3.right);
            newPoint.Position += newPoint.Rotation * Vector3.up * radius;
            newPoint.Straight = false;
            points.Add(newPoint);
            //Debug.Log(newPoint.Rotation.eulerAngles.x);
        }
        // add a number of straight segments

        for (int i = 0; i < amount - 2 * nrOfSteps; i++)
        {
            AddStraight(step, 1);
        }

        // add an upward turn
        currAngle = 0f;
        startPoint = points[points.Count - 1];
        anchorPoint = startPoint;
        anchorPoint.Position += startPoint.Rotation * Vector3.up * radius;

        // perform the DownWards turn
        for (int i = 0; i < nrOfSteps; i++)
        {
            currAngle += (90f) / (float)nrOfSteps;

            AnchorPoint newPoint = anchorPoint;
            newPoint.Rotation *= Quaternion.AngleAxis(currAngle, Vector3.left);
            newPoint.Position += newPoint.Rotation * Vector3.down * radius;
            newPoint.Straight = false;
            points.Add(newPoint);
            //Debug.Log(newPoint.Rotation.eulerAngles.x);
        }
    }



    // adds a corner with a variable angle and raduis
    public void AddCorner( float degrees, float radius, float maxBankAmount, float step)
    {
        float currAngle = 0f;
        float currBank = 0f;
        int nrOfSteps = Mathf.RoundToInt(Mathf.Abs(degrees) * Mathf.Deg2Rad * radius / step);
        //Debug.Log(nrOfSteps);
        
        AnchorPoint startPoint = points[points.Count - 1];
        AnchorPoint anchorPoint = startPoint;
        anchorPoint.Position +=  startPoint.Rotation * Vector3.right * radius * Mathf.Sign(degrees);

        for (int i = 0; i < nrOfSteps; i++)
        {
            currAngle += degrees / (float)nrOfSteps;

            if(i < nrOfSteps/2) currBank += maxBankAmount / (float)nrOfSteps;
            else currBank -= maxBankAmount / (float)nrOfSteps;

            AnchorPoint newPoint = anchorPoint;
            newPoint.Rotation *= Quaternion.AngleAxis(currAngle,  Vector3.up);
            newPoint.Position += newPoint.Rotation * Vector3.left * radius * Mathf.Sign(degrees);
            newPoint.Rotation *= Quaternion.AngleAxis(currBank, Vector3.forward);
            points.Add(newPoint);
            //Debug.Log(newPoint.Rotation.eulerAngles.y);
        }
    }
    

    // adds  looping
    public void AddLooping( float radius, float roadWidth, float step)
    {
        // add the looping with a point offset
        float currAngle = 0f;
        float currOffset = 0f;
        int nrOfSteps = Mathf.RoundToInt((Mathf.PI * 2) * radius / step);
        //Debug.Log(nrOfSteps);

        AnchorPoint startPoint = points[points.Count - 1];
        AnchorPoint anchorPoint = startPoint;
        anchorPoint.Position += startPoint.Rotation * Vector3.up * radius;

        // perform the DownWards turn
        for (int i = 0; i < nrOfSteps; i++)
        {
            currAngle += (360f) / (float)nrOfSteps;
            currOffset += roadWidth / (float)nrOfSteps;

            AnchorPoint newPoint = anchorPoint;
            newPoint.Rotation *= Quaternion.AngleAxis(currAngle, Vector3.left);
            newPoint.Position += newPoint.Rotation * Vector3.down * radius + newPoint.Rotation * Vector3.left * currOffset;
            points.Add(newPoint);
            //Debug.Log(newPoint.Rotation.eulerAngles.x);
        }

    }

    
    // adds a downwards fall
    public void AddDownFall(int lenght, float radius, float step )
    {
        float currAngle = 0f;
        int nrOfSteps = Mathf.RoundToInt((Mathf.PI/2) * radius / step);
        //Debug.Log(nrOfSteps);

        AnchorPoint startPoint = points[points.Count - 1];
        AnchorPoint anchorPoint = startPoint;
        anchorPoint.Position += startPoint.Rotation * Vector3.down * radius;

        // perform the DownWards turn
        for (int i = 0; i < nrOfSteps; i++)
        {
            currAngle += (90f) / (float)nrOfSteps;

            AnchorPoint newPoint = anchorPoint;
            newPoint.Rotation *= Quaternion.AngleAxis(currAngle, Vector3.right);
            newPoint.Position += newPoint.Rotation * Vector3.up * radius;
            points.Add(newPoint);
            //Debug.Log(newPoint.Rotation.eulerAngles.x);
        }
        // add a number of straight segments

        for (int i = 0; i < lenght; i++)
        {
            AddStraight(step,1);
        }

        // add an upward turn
        currAngle = 0f;
        startPoint = points[points.Count - 1];
        anchorPoint = startPoint;
        anchorPoint.Position += startPoint.Rotation * Vector3.up * radius;

        // perform the DownWards turn
        for (int i = 0; i < nrOfSteps; i++)
        {
            currAngle += (90f) / (float)nrOfSteps;

            AnchorPoint newPoint = anchorPoint;
            newPoint.Rotation *= Quaternion.AngleAxis(currAngle,  Vector3.left);
            newPoint.Position += newPoint.Rotation * Vector3.down * radius;
            points.Add(newPoint);
            //Debug.Log(newPoint.Rotation.eulerAngles.x);
        }
    }
    
}
