using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatMesh : MonoBehaviour
{
    Mesh mesh;

    Vector3[] vertices;
    int[] triangles;

    private void Awake()
    {
        mesh = gameObject.GetComponent<MeshFilter>().mesh;
    }

    void Start ()
    {
        MakeMeshData();
        UpdateMesh();
	}
	
	void MakeMeshData()
    {
        vertices = new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 1), new Vector3(1, 0, 0), new Vector3(1, 0, 1) };
        triangles = new int[] { 0, 1, 2, 1, 3, 2 };
	}

    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
    }
}
