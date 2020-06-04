using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// a struct to store the PSR of a given anchor point, used instead of an empty Gameobject with a transform component
public struct AnchorPoint
{
    public Vector3 Position;
    public Quaternion Rotation;
    public Vector3 Scale;
    public bool Straight;
    
}

