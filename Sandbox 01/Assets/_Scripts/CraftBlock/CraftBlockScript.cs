using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftBlockScript : MonoBehaviour
{
    public float scale = 1;
    float _adjScale;

    public Vector3 spawnPos = Vector3.zero;
    Mesh mesh;
    MeshCollider meshCollider;
    Rigidbody rigidbody;

    public GameObject[] neighbours;
    public bool[] hasNeighours;

    List<Vector3> myVertices;
    List<int> myTriangles;
    private void Awake()
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
    private void Start()
    {
        SetNeighbours();
        MakeVolex(spawnPos);
        CreatMesh();
        CallNeigbours();
    }

    public void MakeVolex(Vector3 pos)
    {
        myVertices = new List<Vector3>();
        myTriangles = new List<int>();

        for (int i = 0; i < 6; i++)
        {
          MakeFace((BlockFaceDirection)i, _adjScale, pos);
        }
    }

    void MakeFace(BlockFaceDirection faceDir, float adjScale, Vector3 pos)
    {
        myVertices.AddRange(CraftBlockData.GetFaceVertices(faceDir, adjScale, pos));

        int indexShift = myVertices.Count;
        myTriangles.Add(indexShift - 4 + 0);
        myTriangles.Add(indexShift - 4 + 1);
        myTriangles.Add(indexShift - 4 + 2);
        myTriangles.Add(indexShift - 4 + 1);
        myTriangles.Add(indexShift - 4 + 0);
        myTriangles.Add(indexShift - 4 + 3);
    }

    void CreatMesh()
    {
        mesh.Clear();
        mesh.vertices = myVertices.ToArray();
        mesh.triangles = myTriangles.ToArray();
        mesh.RecalculateNormals();

        if (!GetComponent<MeshCollider>())
        {
            meshCollider = gameObject.AddComponent<MeshCollider>();
        }
        if (!GetComponent<Rigidbody>())
        {
            meshCollider.convex = true;
            rigidbody = gameObject.AddComponent<Rigidbody>();
            rigidbody.isKinematic = true;
        }
    }

    public void SetNeighbours()
    {
        neighbours = new GameObject[6];
        hasNeighours = new bool[6];
        Ray[] rays =
        {
           new Ray(transform.position, Vector3.left),
           new Ray(transform.position, Vector3.right),
           new Ray(transform.position, Vector3.down),
           new Ray(transform.position, Vector3.up),
           new Ray(transform.position, Vector3.back),
           new Ray(transform.position, Vector3.forward),
        };
        for (int i = 0; i < neighbours.Length; i++)
        {
            RaycastHit hit;
            if(Physics.Raycast(rays[i], out hit, 0.51f))
            {
                neighbours[i] = hit.transform.gameObject;
                hasNeighours[i] = true;
            }
            else
            {
                neighbours[i] = this.gameObject;
                hasNeighours[i] = false;
            }
        }
    }

    void CallNeigbours()
    {
        for (int i = 0; i < neighbours.Length; i++)
        {
            if (neighbours[i].GetComponent<CraftBlockScript>())
            {
                neighbours[i].GetComponent<CraftBlockScript>().SetNeighbours();
            }
        }
    }


}

