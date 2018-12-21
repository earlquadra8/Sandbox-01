using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CubeMeshData
{

    public static Vector3[] vertices = // the relative vertices of a cube
    {
        new Vector3( 1,  1,  1),
        new Vector3(-1,  1,  1),
        new Vector3(-1, -1,  1),
        new Vector3( 1, -1,  1),
        new Vector3(-1,  1, -1),
        new Vector3( 1,  1, -1),
        new Vector3( 1, -1, -1),
        new Vector3(-1, -1, -1),
    };

    public static int[][] faceTriangles = // the vertices that form the faces, in sequence
    {
        new int [] {0, 1, 2, 3},// North face
        new int [] {5, 0, 3, 6},// East face
        new int [] {4, 5, 6, 7},// South face
        new int [] {1, 4, 7, 2},// West face
        new int [] {5, 4, 1, 0},// Up face
        new int [] {3, 2, 7, 6},// Down face
    };

    public static Vector3[] faceVerteices(int faceDir, float scale, Vector3 pos)//pos == the voxel spawn pos
    {
        Vector3[] fv = new Vector3[4];// a face has 4 vertices
        for (int i = 0; i < fv.Length; i++)
        {
            fv[i] = (vertices[faceTriangles[faceDir][i]] * scale) + pos;
        }
        return fv;
    }
    public static Vector3[] faceVerteices(FaceDirection faceDir, float scale, Vector3 pos)
    {
        return faceVerteices((int)faceDir, scale,pos);
    }
}
