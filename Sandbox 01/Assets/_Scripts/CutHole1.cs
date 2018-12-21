using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutHole1 : MonoBehaviour
{
    public Transform cutter;

    Vector3 shapeCenter;
    Vector3 shapeHalfSize;
    Vector3 castDir;
    RaycastHit[] hitInfos;

    void Start ()
    {
        shapeHalfSize = Vector3.one * 0.5f;
        castDir = Vector3.down;
    }

    void Update()
    {
        shapeCenter = cutter.transform.position;
        if (Input.GetMouseButtonDown(0))
        {
            CutMesh();
        }
    }

    void CutMesh()
    {
        //RaycastHit hitInfo;
        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //Ray ray = new Ray(cutter.position, -cutter.up);
        //if (Physics.Raycast(ray, out hitInfo, 1000.0f))
        //{
        //    int hitTri = hitInfo.triangleIndex;
        //    print(hitTri);
        //    DeleteTri(hitTri);
        //}
        hitInfos = Physics.BoxCastAll(shapeCenter, shapeHalfSize, castDir);
        if (hitInfos.Length > 0)
        {
            for (int i = 0; i < hitInfos.Length; i++)
            {
                print(hitInfos[i].triangleIndex);
                DeleteTri(hitInfos[i].triangleIndex);
            }
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

    void DeleteSquare(int index0, int index1)
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
        gameObject.AddComponent<MeshCollider>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (cutter != null)
        {
            Gizmos.DrawRay(cutter.position, -cutter.up * 5f);
        }
    }
}
