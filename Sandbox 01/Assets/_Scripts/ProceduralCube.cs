using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralCube : MonoBehaviour
{
    Mesh mesh;
    List<Vector3> vertices;
    List<int> triangles;

    public float scale = 1f;
    public Vector3 pos;

    float _adjScale;

    void Awake()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        _adjScale = scale * 0.5f;
    }

    void Start()
    {
        MakeCube(_adjScale, pos * scale);// the pos scale with cube scale for voxels building
        UpdateMesh();
    }

    void Update ()
    {
		
	}

    void MakeCube(float cubeScale, Vector3 cubePos)
    {
        vertices = new List<Vector3>();
        triangles = new List<int>();

        for (int i = 0; i < 6; i++)
        {
            MakeFace(i, cubeScale, cubePos);
        }
    }

    void MakeFace(int faceDir, float faceScale, Vector3 facePos)
    {
        vertices.AddRange(CubeMeshData.faceVerteices(faceDir, faceScale, facePos));

        int vCount = vertices.Count;// == 4
        triangles.Add(vCount - 4 + 0);
        triangles.Add(vCount - 4 + 1);
        triangles.Add(vCount - 4 + 2);
        triangles.Add(vCount - 4 + 0);
        triangles.Add(vCount - 4 + 2);
        triangles.Add(vCount - 4 + 3);
    }

    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();

    }
}
