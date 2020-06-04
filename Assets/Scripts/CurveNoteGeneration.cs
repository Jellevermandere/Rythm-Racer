using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class CurveNoteGeneration : MonoBehaviour
{
    [Header("Track Shape Settings")]
    public float roadWidth = 1;
    public float roadHeight = 0.1f;

    public Material standardMaterial;
    public Material glowMaterial;

    public Path path;

    private List<GameObject> groundPoints = new List<GameObject>();
    private MeshRenderer meshRend;

    private void Start()
    {
        meshRend = GetComponent<MeshRenderer>();
    }

    public void UpdateRoad(float stepDistance)
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = CreateCurveMesh(path.points.ToArray(), stepDistance);
        meshFilter.mesh.RecalculateBounds();
    }

    public void UpdateMaterial(bool active)
    {

        if (active) meshRend.material = glowMaterial;
        else meshRend.material = standardMaterial;

    }

    public Mesh CreateCurveMesh(AnchorPoint[] transforms, float stepDistance)
    {
        Vector3[] points = new Vector3[transforms.Length];
        for (int i = 0; i < transforms.Length; i++)
        {
            points[i] = transforms[i].Position;
        }

        Vector3[] verts = new Vector3[points.Length * 2];
        Vector2[] uvs = new Vector2[verts.Length];
        int[] tris = new int[2 * (points.Length - 1) * 3];
        int vertIndex = 0;
        int triIndex = 0;


        for (int i = 0; i < points.Length; i++)
        {
            Vector3 forward = transforms[i].Rotation * Vector3.forward;


            forward.Normalize();
            Vector3 left = transforms[i].Rotation * Vector3.left;

            verts[vertIndex] = points[i] + left * roadWidth * 0.5f +transforms[i].Rotation * Vector3.up * roadHeight;
            verts[vertIndex + 1] = points[i] - left * roadWidth * 0.5f +transforms[i].Rotation * Vector3.up * roadHeight;



            float v = i / (float)(points.Length-1); 
            uvs[vertIndex] = new Vector2(0, v);
            uvs[vertIndex + 1] = new Vector2(1, v);
            //Debug.Log(transforms[i].Straight);
            if (i < (points.Length - 1) && !transforms[i].Straight)
            {
                tris[triIndex] = vertIndex;
                tris[triIndex + 1] = (vertIndex + 2);
                tris[triIndex + 2] = vertIndex + 1;

                tris[triIndex + 3] = vertIndex + 1;
                tris[triIndex + 4] = (vertIndex + 2);
                tris[triIndex + 5] = (vertIndex + 3);
            }
            vertIndex += 2;
            triIndex += 6;
        }

        Mesh mesh = new Mesh();
        mesh.vertices = verts;
        mesh.triangles = tris;
        mesh.uv = uvs;
        mesh.RecalculateNormals();

        return mesh;

    }
}
