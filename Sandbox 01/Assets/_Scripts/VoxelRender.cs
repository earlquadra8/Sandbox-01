using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelRender : MonoBehaviour
{
    Mesh mesh;
    List<Vector3> vertices;
    List<int> triangles;

    public float scale = 1f;

    float _adjScale;

    void Awake()
    {
        if (!gameObject.GetComponent<MeshFilter>())
        {
            gameObject.AddComponent<MeshFilter>();
        }
        if (!gameObject.GetComponent<MeshRenderer>())
        {
            MeshRenderer mr = gameObject.AddComponent<MeshRenderer>();
        }
        gameObject.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/Blue");

        mesh = GetComponent<MeshFilter>().mesh;
        _adjScale = scale * 0.5f;
    }

    void Start()
    {
        GenerateVoxelMesh(new VoxelData());
        UpdateMesh();
    }
    // decide where to make a cube
    void GenerateVoxelMesh(VoxelData patternData)
    {
        vertices = new List<Vector3>();
        triangles = new List<int>();
        
        for (int z = 0; z < patternData.Depth; z++)
        {
            for (int y = 0; y < patternData.Height; y++)
            {
                for (int x = 0; x < patternData.Width; x++)
                {
                    if (patternData.GetCellPattern(x, y, z) != false)//get the pattern map, decide where to make a cube, false means no voxel
                    {
                        MakeCube(_adjScale, new Vector3(x * scale, y * scale, z * scale), x, y, z, patternData);
                    }
                }
            }
        }
    }
    //decide where to make a face
    //                                              which cube
    void MakeCube(float cubeScale, Vector3 cubePos, int x, int y, int z, VoxelData patternData)
    {
        for (int i = 0; i < 6; i++)// one cube has 6 faces
        {
            if (patternData.GetNeighbour(x, y, z, (FaceDirection)i) == false)// if this cube has no neighbour on that face direction
            {
                MakeFace((FaceDirection)i, cubeScale, cubePos);
            }
        }
    }

    void MakeFace(FaceDirection faceDir, float faceScale, Vector3 facePos)
    {
        vertices.AddRange(CubeMeshData.faceVerteices(faceDir, faceScale, facePos));// get the cooked vertices of faces

        int vCount = vertices.Count;
        // put the triangle order in a sequence; triangles = {0, 1, 2, 0, 2, 3, 4, 5, 6, 4, 6, 7...}
        triangles.Add(vCount - 4 + 0);// 0 relative pos of a face   0- -3
        triangles.Add(vCount - 4 + 1);// 1 relative pos of a face   |   |
        triangles.Add(vCount - 4 + 2);// 2 relative pos of a face   1- -2
        triangles.Add(vCount - 4 + 0);// 0 relative pos of a face
        triangles.Add(vCount - 4 + 2);// 2 relative pos of a face
        triangles.Add(vCount - 4 + 3);// 3 relative pos of a face
    }

    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
    }
}
	

