using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BlockCombineController : MonoBehaviour
{
    public GameObject addBlockPrefab;
    public Camera placeBlockCamera;
    public GameObject[] indicators;

    void Update()
    {
        if (Input.GetMouseButtonDown (0)) {
            RaycastHit hitInfo;
            Ray ray = placeBlockCamera.ScreenPointToRay (Input.mousePosition);

            if (Physics.Raycast (ray, out hitInfo, Mathf.Infinity)) {
                int hitTri = hitInfo.triangleIndex;
                print ("hitTri: " + hitTri);

                GameObject hitBlock = hitInfo.transform.gameObject;
                MeshFilter hitBlockMeshFilter = hitBlock.GetComponent<MeshFilter> ();

                int[] faceTri = hitBlockMeshFilter.mesh.triangles;
                print ("faceTriArry: " + faceTri[hitTri]);
                print ("faceTriArry: " + faceTri.Length);
                Vector3[] faceVertices = hitBlockMeshFilter.mesh.vertices;
                print ("faceVertices: " + faceVertices[faceTri[hitTri] + 0]);
                print ("faceVertices: " + faceVertices[faceTri[hitTri] + 1]);
                print ("faceVertices: " + faceVertices[faceTri[hitTri] + 2]);
                print ("faceVertices: " + faceVertices.Length);

            }
        }
    }

    private void AddBlock( GameObject toAddBlock )
    {
        if (Input.GetMouseButtonDown (0)) {
            RaycastHit hitInfo;
            Ray ray = placeBlockCamera.ScreenPointToRay (Input.mousePosition);
            if (Physics.Raycast (ray, out hitInfo, 500)) {
                int hitTriIndex = hitInfo.triangleIndex;
                Vector3 blockPos = hitInfo.point + hitInfo.normal * toAddBlock.transform.localScale.x * 0.5f;
                GameObject hitBlock = hitInfo.transform.gameObject;
                MeshFilter hitBlockMeshFilter = hitBlock.GetComponent<MeshFilter> ();

                int[] hitBlockTri = hitBlockMeshFilter.mesh.triangles;
                Vector3[] hitBlockVertices = hitBlockMeshFilter.mesh.vertices;

                //Find vertices of the hit tri
                Vector3[] hitTriVertices = {
                    hitBlockVertices[hitBlockTri[hitTriIndex*3 +0]],
                    hitBlockVertices[hitBlockTri[hitTriIndex*3 +1]],
                    hitBlockVertices[hitBlockTri[hitTriIndex*3 +2]],
                };
                //Find edges of the hit tri
                float[] hitTriEdgesLength ={
                    Vector3.Distance(hitTriVertices[0], hitTriVertices[1]), //01
                    Vector3.Distance(hitTriVertices[0], hitTriVertices[2]), //02
                    Vector3.Distance(hitTriVertices[1], hitTriVertices[2]) //12
                };
                //Find shared vertex of longest edge
                Vector3[] sharedVertices = new Vector3[2];
                if (hitTriEdgesLength[0] > hitTriEdgesLength[1] && hitTriEdgesLength[0] > hitTriEdgesLength[2]) {
                    sharedVertices[0] = hitTriVertices[0];
                    sharedVertices[1] = hitTriVertices[1];
                } else if (hitTriEdgesLength[1] > hitTriEdgesLength[0] && hitTriEdgesLength[1] > hitTriEdgesLength[2]) {
                    sharedVertices[0] = hitTriVertices[1];
                    sharedVertices[1] = hitTriVertices[2];
                } else {
                    sharedVertices[0] = hitTriVertices[1];
                    sharedVertices[1] = hitTriVertices[2];
                }

                int anotherTrisIndex = FindAnotherTri (sharedVertices, hitTriIndex, hitBlockTri, hitBlockVertices);

            }
        }
    }

    private int FindAnotherTri( Vector3[] vertices, int hitTriIndex, int[] meshTri, Vector3[] meshVertices )
    {
        int chunkStartIndex = 0;
        while (chunkStartIndex < meshTri.Length) {
            if (chunkStartIndex / 3 != hitTriIndex) {
                if (( meshVertices[meshTri[chunkStartIndex]] == vertices[0] && ( meshVertices[meshTri[chunkStartIndex + 1]] == vertices[1] || meshVertices[meshTri[chunkStartIndex + 2]] == vertices[1] ) ) ||
                    ( meshVertices[meshTri[chunkStartIndex]] == vertices[1] && ( meshVertices[meshTri[chunkStartIndex + 1]] == vertices[0] || meshVertices[meshTri[chunkStartIndex + 2]] == vertices[0] ) ) ||
                    ( meshVertices[meshTri[chunkStartIndex + 1]] == vertices[1] && ( meshVertices[meshTri[chunkStartIndex]] == vertices[0] || meshVertices[meshTri[chunkStartIndex + 2]] == vertices[0] ) ) ||
                    ( meshVertices[meshTri[chunkStartIndex + 1]] == vertices[0] && ( meshVertices[meshTri[chunkStartIndex]] == vertices[1] || meshVertices[meshTri[chunkStartIndex + 2]] == vertices[1] ) )) {
                    return chunkStartIndex / 3;
                }
            }
            chunkStartIndex += 3;
        }
        return -1;
    }

    private Vector3[] FindBlockFaceVertices( int hitTriIndex, int[] meshTri, Vector3[] meshVertices )
    {
        // hit tri vertices
        List<Vector3> hitTriVertices = new List<Vector3> ();
        hitTriVertices.Add (meshVertices[meshTri[hitTriIndex + 0]]);
        hitTriVertices.Add (meshVertices[meshTri[hitTriIndex + 1]]);
        hitTriVertices.Add (meshVertices[meshTri[hitTriIndex + 2]]);

        // find long edge which share with anther tri
        List<float> hitTriDists = new List<float> ();
        hitTriDists.Add (Vector3.Distance (hitTriVertices[0], hitTriVertices[1]));
        hitTriDists.Add (Vector3.Distance (hitTriVertices[0], hitTriVertices[2]));
        hitTriDists.Add (Vector3.Distance (hitTriVertices[1], hitTriVertices[2]));

        List<int> longEdgeVerticeIndex = new List<int> ();
        if (hitTriDists[0] > hitTriDists[1] &&
            hitTriDists[0] > hitTriDists[2]) {
            longEdgeVerticeIndex.Add (0);
            longEdgeVerticeIndex.Add (1);
        } else if (hitTriDists[1] > hitTriDists[0] &&
            hitTriDists[1] > hitTriDists[2]) {
            longEdgeVerticeIndex.Add (0);
            longEdgeVerticeIndex.Add (2);
        } else if (hitTriDists[2] > hitTriDists[1] &&
            hitTriDists[2] > hitTriDists[0]) {
            longEdgeVerticeIndex.Add (1);
            longEdgeVerticeIndex.Add (2);
        }

        // find anther tri
        List<Vector3> anotherTriVertices = new List<Vector3> ();
        for (int i = 0; i < meshTri.Length; i += 3) {
            if (i == hitTriIndex) {
                continue;
            }
            if (meshVertices[meshTri[i]] == meshVertices[meshTri[longEdgeVerticeIndex[0]]] && ( meshVertices[meshTri[i] + 1] == meshVertices[meshTri[longEdgeVerticeIndex[1]]] || meshVertices[meshTri[i] + 2] == meshVertices[meshTri[longEdgeVerticeIndex[1]]] ||
                meshVertices[meshTri[i]] == meshVertices[meshTri[longEdgeVerticeIndex[1]]] && ( meshVertices[meshTri[i] + 1] == meshVertices[meshTri[longEdgeVerticeIndex[0]]] || meshVertices[meshTri[i] + 2] == meshVertices[meshTri[longEdgeVerticeIndex[0]]] ||
                meshVertices[meshTri[i] + 1] == meshVertices[meshTri[longEdgeVerticeIndex[0]]] && ( meshVertices[meshTri[i]] == meshVertices[meshTri[longEdgeVerticeIndex[1]]] || meshVertices[meshTri[i] + 2] == meshVertices[meshTri[longEdgeVerticeIndex[1]]] ||
                meshVertices[meshTri[i] + 1] == meshVertices[meshTri[longEdgeVerticeIndex[1]]] && ( meshVertices[meshTri[i]] == meshVertices[meshTri[longEdgeVerticeIndex[0]]] || meshVertices[meshTri[i] + 2] == meshVertices[meshTri[longEdgeVerticeIndex[0]]] ) ) ) )) {
                anotherTriVertices.Add (meshVertices[meshTri[i + 0]]);
                anotherTriVertices.Add (meshVertices[meshTri[i + 1]]);
                anotherTriVertices.Add (meshVertices[meshTri[i + 2]]);
                break;
            }
        }
        // face vertices
        List<Vector3> faceVertice = new List<Vector3> ();
        foreach (var item in hitTriVertices) {
            if (!faceVertice.Contains (item)) {
                hitTriVertices.Add (item);
            }
        }
        foreach (var item in anotherTriVertices) {
            if (!faceVertice.Contains (item)) {
                hitTriVertices.Add (item);
            }
        }

        return faceVertice.ToArray ();
    }
}
