using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutHole : MonoBehaviour
{
    public Transform cutter;
    public bool useCutter;

	void Start ()
    {

	}

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CutMesh();
        }
    }

    void CutMesh()
    {
        if (!gameObject.GetComponent<MeshCollider>())
        {
            gameObject.AddComponent<MeshCollider>();
        }

        RaycastHit hitInfo;
        Ray ray;
        if (useCutter)
        {
            ray = new Ray(cutter.position, -cutter.up);
        }
        else
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        }
        if (Physics.Raycast(ray, out hitInfo, 1000.0f))
        {
            bool isConvex = false;
            if (hitInfo.transform.GetComponent<MeshCollider>().convex)
            {
                print("Hit the convex");
                isConvex = true;
                hitInfo.transform.GetComponent<MeshCollider>().convex = false;
                Physics.Raycast(ray, out hitInfo, 1000.0f);
            }
            int hitTri = hitInfo.triangleIndex;
            print("hitTri: " + hitTri);

            int[] triangles = transform.GetComponent<MeshFilter>().mesh.triangles;
            Vector3[] vertices = transform.GetComponent<MeshFilter>().mesh.vertices;
            //find all vertices of the hit tri
            Vector3 p0 = vertices[triangles[hitTri * 3 + 0]];
            Vector3 p1 = vertices[triangles[hitTri * 3 + 1]];
            Vector3 p2 = vertices[triangles[hitTri * 3 + 2]];
            //find all edges of the hit tri
            float edge01 = Vector3.Distance(p0, p1);
            float edge02 = Vector3.Distance(p0, p2);
            float edge12 = Vector3.Distance(p1, p2);
            //common vertex position
            Vector3 sharedVertex0;
            Vector3 sharedVertex1;
            //find longest edge
            if (edge01 > edge02 && edge01 > edge12)
            {
                sharedVertex0 = p0;
                sharedVertex1 = p1;
            }
            else if (edge02 > edge01 && edge02 > edge12)
            {
                sharedVertex0 = p0;
                sharedVertex1 = p2;
            }
            else
            {
                sharedVertex0 = p1;
                sharedVertex1 = p2;
            }

            int vertexIndex0 = FindVertexIndex(sharedVertex0);// the vertex index in the trianles[]
            int vertexIndex1 = FindVertexIndex(sharedVertex1);


            DeleteSquare(hitTri, FindTri(vertices[vertexIndex0], vertices[vertexIndex1], hitTri), isConvex);
        }

    }

    int FindTri(Vector3 vertex0, Vector3 vertex1, int hitTriIndex)// find the tri index with two vertices
    {
        int[] triangles = transform.GetComponent<MeshFilter>().mesh.triangles;
        Vector3[] vertices = transform.GetComponent<MeshFilter>().mesh.vertices;
        int k = 0;

        while (k < triangles.Length)
        {
            if (k / 3 != hitTriIndex)// if it is not the tri already have
            {   //  qxx                                   qox                                    qxo
                if (vertices[triangles[k]] == vertex0 && (vertices[triangles[k+1]] == vertex1 || vertices[triangles[k+2]] == vertex1))
                {
                    return k / 3;
                }//      qxx                                   qox                                      qxo
                else if (vertices[triangles[k]] == vertex1 && (vertices[triangles[k + 1]] == vertex0 || vertices[triangles[k + 2]] == vertex0))
                {
                    return k / 3;
                }//      xqx                                       oqx                                  xqo
                else if (vertices[triangles[k + 1]] == vertex1 && (vertices[triangles[k]] == vertex0 || vertices[triangles[k + 2]] == vertex0))
                {
                    return k / 3;
                }//      xqx                                       oqx                                  xqo
                else if (vertices[triangles[k + 1]] == vertex0 && (vertices[triangles[k]] == vertex1 || vertices[triangles[k + 2]] == vertex1))
                {
                    return k / 3;
                }
            }
            k += 3;
        }
        return -1;
    }

    int FindVertexIndex(Vector3 vertex)
    {
        Vector3[] vertices = transform.GetComponent<MeshFilter>().mesh.vertices;
        for (int i = 0; i < vertices.Length; i++)
        {
            if (vertices[i] == vertex)
            {
                return i;
            }
        }
        return -1;
    }

    void DeleteTri(int index)
    {
        Destroy(gameObject.GetComponent<MeshCollider>());
                
        Mesh mesh = transform.GetComponent<MeshFilter>().mesh;
        int[] oldTriangles = mesh.triangles;
        int[] newTriangles = new int[mesh.triangles.Length - 3];

        int i = 0;
        int j = 0;
        while (j < mesh.triangles.Length)
        {
            if (j != index * 3)// check is it the target tri
            {
                newTriangles[i++] = oldTriangles[j++];
                newTriangles[i++] = oldTriangles[j++];
                newTriangles[i++] = oldTriangles[j++];
            }
            else
            {
                j += 3;// by pass the target tri
            }
        }
        mesh.triangles = newTriangles;
        gameObject.AddComponent<MeshCollider>();
    }

    void DeleteSquare(int index0, int index1, bool isConvex)
    {
        Destroy(gameObject.GetComponent<MeshCollider>());
        Mesh mesh = transform.GetComponent<MeshFilter>().mesh;
        int[] oldTriangles = mesh.triangles;
        int[] newTriangles = new int[mesh.triangles.Length - 3];

        int i = 0;
        int j = 0;

        while (j < mesh.triangles.Length)
        {
            if (j != index0 * 3 && j != index1 * 3)// check is it the target tris
            {
                newTriangles[i++] = oldTriangles[j++];
                newTriangles[i++] = oldTriangles[j++];
                newTriangles[i++] = oldTriangles[j++];
            }
            else
            {
                j += 3;// by pass the target tri
            }
        }
        mesh.triangles = newTriangles;

        MeshCollider mc = gameObject.AddComponent<MeshCollider>();
        mc.convex = isConvex;
        mc.sharedMesh = mesh;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (cutter != null && useCutter)
        {
            Gizmos.DrawRay(cutter.position, -cutter.up * 5f);
        }
    }
}
