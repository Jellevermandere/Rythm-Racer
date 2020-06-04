using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

public class TerrainGeneration : MonoBehaviour
{
    [Header("Track Shape Settings")]
    public float roadWidth = 1;
    public float railingWidth = 1f;
    public float railingHeight = 1f;
    public float topRailingWidth = 0.5f;
    public float roadHeight = 0.1f;

    [Header ("Visual Settings")]
    public float tiling = 1;
    public bool generateSpheres;
    public float updateInteral;


    public GameObject groundPoint;
    private Path path;

    private List<GameObject> groundPoints = new List<GameObject>();


    private float pathLength;


    

    // Start is called before the first frame update
    void Awake()
    {
        path = GetComponent<Path>();
        //GeneratePointsInCurve();
        if(generateSpheres) CreateSpheres();
        //else UpdateRoad();
    }


    //create debug spheres
    public void CreateSpheres()
    {
        if(groundPoints.Count < path.points.Count)
        {
            for (int i = groundPoints.Count; i < path.points.Count; i++)
            {
                GameObject newpoint = Instantiate(groundPoint, Vector3.zero, Quaternion.identity);
                groundPoints.Add(newpoint);

            }
        }
        else if(groundPoints.Count > path.points.Count)
        {
            for (int i = 0; i < groundPoints.Count - path.points.Count; i++)
            {
                GameObject oldPoint = groundPoints[i + path.points.Count];
                groundPoints.Remove(oldPoint);
                Destroy(oldPoint);
            }
            
        }
        for (int i = 0; i < path.points.Count; i++)
        {
            groundPoints[i].transform.position = path.points[i].Position;
            groundPoints[i].transform.rotation = path.points[i].Rotation;
        }

    }

    public void UpdateRoad(float stepDistance)
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = CreateRoadMesh(path.points.ToArray(),stepDistance);
        meshFilter.mesh.RecalculateBounds();

        int textureRepeat = Mathf.RoundToInt(tiling * pathLength * 0.05f);
        GetComponent<MeshRenderer>().sharedMaterial.mainTextureScale = new Vector2(1, textureRepeat);
    }

    //generates a mesh
    public Mesh CreateRoadMesh(AnchorPoint[] transforms, float stepDistance)
    {
        Vector3[] points = new Vector3[transforms.Length];
        pathLength = (points.Length - 1) * stepDistance;
        for (int i = 0; i < transforms.Length; i++)
        {
            points[i] = transforms[i].Position;
        }
        Vector3[] verts = new Vector3[points.Length * 8];
        Vector2[] uvs = new Vector2[verts.Length];
        int[] tris = new int[16 * (points.Length-1) * 3];
        int vertIndex = 0;
        int triIndex = 0;

        float pathDist = 0f;

        for (int i = 0; i < points.Length; i++)
        {
            Vector3 forward = transforms[i].Rotation * Vector3.forward;
            /*
            if(i<points.Length - 1)
            {
                forward += points[(i + 1)] - points[i];
            }
            if (i > 0)
            {
                forward += points[i] - points[(i - 1)];
            }
            */
            
            
            forward.Normalize();
            Vector3 left = transforms[i].Rotation * Vector3.left; //new Vector3(-forward.z, 0, forward.x);
            //Vector3 left = Vector3.Normalize(-points[i] + worldOffset);

            verts[vertIndex]     = points[i] + left * (roadWidth * 0.5f + railingWidth);
            verts[vertIndex + 1] = points[i] + left * (roadWidth * 0.5f + (railingWidth - topRailingWidth) / 2f + topRailingWidth) + transforms[i].Rotation*Vector3.up * railingHeight;
            verts[vertIndex + 2] = points[i] + left * (roadWidth * 0.5f + (railingWidth - topRailingWidth) / 2f) + transforms[i].Rotation*Vector3.up * railingHeight;
            verts[vertIndex + 3] = points[i] + left * (roadWidth * 0.5f) + transforms[i].Rotation*Vector3.up * roadHeight;
            verts[vertIndex + 4] = points[i] - left * (roadWidth * 0.5f) + transforms[i].Rotation*Vector3.up * roadHeight;
            verts[vertIndex + 5] = points[i] - left * (roadWidth * 0.5f + (railingWidth - topRailingWidth) / 2f) + transforms[i].Rotation*Vector3.up * railingHeight;
            verts[vertIndex + 6] = points[i] - left * (roadWidth * 0.5f + (railingWidth - topRailingWidth) / 2f + topRailingWidth) + transforms[i].Rotation*Vector3.up * railingHeight;
            verts[vertIndex + 7] = points[i] - left * (roadWidth * 0.5f + railingWidth);



            float completionPercent = pathDist / pathLength; //i / (float)(points.Length - 1);
            float v =  completionPercent;
            uvs[vertIndex] = new Vector2(0, v);
            uvs[vertIndex + 1] = new Vector2(0.1f, v);
            uvs[vertIndex + 2] = new Vector2(0.15f, v);
            uvs[vertIndex + 3] = new Vector2(0.25f, v);
            uvs[vertIndex + 4] = new Vector2(0.75f, v);
            uvs[vertIndex + 5] = new Vector2(0.85f, v);
            uvs[vertIndex + 6] = new Vector2(0.9f, v);
            uvs[vertIndex + 7] = new Vector2(1, v);

            if (i < points.Length - 1)
            {
                tris[triIndex] = vertIndex;
                tris[triIndex + 1] = (vertIndex + 8);
                tris[triIndex + 2] = vertIndex + 1;

                tris[triIndex + 3] = vertIndex + 1;
                tris[triIndex + 4] = (vertIndex + 9);
                tris[triIndex + 5] = vertIndex + 2;

                tris[triIndex + 6] = vertIndex + 2;
                tris[triIndex + 7] = (vertIndex + 10);
                tris[triIndex + 8] = vertIndex + 3;

                tris[triIndex + 9] = vertIndex + 3;
                tris[triIndex + 10] = (vertIndex + 11);
                tris[triIndex + 11] = vertIndex + 4;

                tris[triIndex + 12] = vertIndex + 4;
                tris[triIndex + 13] = (vertIndex + 12);
                tris[triIndex + 14] = vertIndex + 5;

                tris[triIndex + 15] = vertIndex + 5;
                tris[triIndex + 16] = (vertIndex + 13);
                tris[triIndex + 17] = vertIndex + 6;

                tris[triIndex + 18] = vertIndex + 6;
                tris[triIndex + 19] = (vertIndex + 14);
                tris[triIndex + 20] = vertIndex + 7;


                tris[triIndex + 21] = vertIndex + 1;
                tris[triIndex + 22] = (vertIndex + 8);
                tris[triIndex + 23] = (vertIndex + 9);

                tris[triIndex + 24] = vertIndex + 2;
                tris[triIndex + 25] = (vertIndex + 9);
                tris[triIndex + 26] = (vertIndex + 10);

                tris[triIndex + 27] = vertIndex + 3;
                tris[triIndex + 28] = (vertIndex + 10);
                tris[triIndex + 29] = (vertIndex + 11);

                tris[triIndex + 30] = vertIndex + 4;
                tris[triIndex + 31] = (vertIndex + 11);
                tris[triIndex + 32] = (vertIndex + 12);

                tris[triIndex + 33] = vertIndex + 5;
                tris[triIndex + 34] = (vertIndex + 12);
                tris[triIndex + 35] = (vertIndex + 13);

                tris[triIndex + 36] = vertIndex + 6;
                tris[triIndex + 37] = (vertIndex + 13);
                tris[triIndex + 38] = (vertIndex + 14);

                tris[triIndex + 39] = vertIndex + 7;
                tris[triIndex + 40] = (vertIndex + 14);
                tris[triIndex + 41] = (vertIndex + 15);

                tris[triIndex + 42] = vertIndex + 7;
                tris[triIndex + 43] = (vertIndex + 8);
                tris[triIndex + 44] = (vertIndex );

                tris[triIndex + 45] = vertIndex + 7;
                tris[triIndex + 46] = (vertIndex + 15);
                tris[triIndex + 47] = (vertIndex + 8);
            }
            pathDist += stepDistance;
            vertIndex += 8;
            triIndex += 16 * 3;
        }

        Mesh mesh = new Mesh();
        mesh.vertices = verts;
        mesh.triangles = tris;
        mesh.uv = uvs;
        mesh.RecalculateNormals();

        return mesh;

    }
}
