using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralGrid : MonoBehaviour
{
    public float cellSize = 1;
    public Vector3 gridOffset;// move the whole grid
    public int gridSize = 1;// how big the grid is

    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;


	void Awake ()
    {
        mesh = GetComponent<MeshFilter>().mesh;
	}
	
	void Start ()
    {
        MakeContiguousProceduralMesh();
        UpdateMesh();
    }

    void MakeDiscreteProceduralMesh()
    {
        vertices = new Vector3[4 * gridSize * gridSize];
        triangles = new int[6 * gridSize * gridSize];

        int v = 0;
        int t = 0;
        // center the tile
        float vertexOffset = cellSize * 0.5f;

        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {   // make the tile spawn in other position of the grid
                Vector3 cellOffset = new Vector3(x * cellSize, 0, y * cellSize);

                vertices[v    ] = new Vector3(-vertexOffset, 0, -vertexOffset) + cellOffset + gridOffset;
                vertices[v + 1] = new Vector3(-vertexOffset, 0,  vertexOffset) + cellOffset + gridOffset;
                vertices[v + 2] = new Vector3( vertexOffset, 0, -vertexOffset) + cellOffset + gridOffset;
                vertices[v + 3] = new Vector3( vertexOffset, 0,  vertexOffset) + cellOffset + gridOffset;

                triangles[t    ] = v;
                triangles[t + 1] = triangles[t + 4] = v + 1;
                triangles[t + 2] = triangles[t + 3] = v + 2;
                triangles[t + 5] = v + 3;

                v += 4;
                t += 6;
            }
        }

    }

    void MakeContiguousProceduralMesh()
    {
        vertices = new Vector3[(gridSize + 1) * (gridSize + 1)];
        triangles = new int[6 * gridSize * gridSize];

        int v = 0;
        int t = 0;
        // center the tile
        float vertexOffset = cellSize * 0.5f;
        //create vertices grid
        for (int x = 0; x < gridSize + 1; x++)
        {
            for (int y = 0; y < gridSize + 1; y++)
            {
                vertices[v] = new Vector3((x * cellSize) - vertexOffset, 0, (y * cellSize) - vertexOffset);
                v++;
            }
        }
        //reset vertex tracker
        v = 0;
        // set each cell's triangles
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                triangles[t    ] = v;
                triangles[t + 1] = triangles[t + 4] = v + 1;
                triangles[t + 2] = triangles[t + 3] = v + (gridSize + 1);// 3rd tri point is up one row
                triangles[t + 5] = v + (gridSize + 1) + 1;
                v++;// move right on a row
                t += 6;
            }
            v++;// move to a new row;
        }

    }

    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

    }
}
