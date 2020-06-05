using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotator : MonoBehaviour
{
    public Vector3 RotationSpeed;
    public float randomScaleMultiplier = 0;

    // Start is called before the first frame update
    void Start()
    {
        transform.localScale *= Random.Range(1f - randomScaleMultiplier, 1f + randomScaleMultiplier);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(RotationSpeed * Time.deltaTime);
    }
}
