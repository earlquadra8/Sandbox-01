using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CraftBlockData
{
    static Vector3[] localVertices =
    {
        new Vector3(-1, -1, -1),
        new Vector3(-1, -1,  1),
        new Vector3(-1,  1, -1),
        new Vector3(-1,  1,  1),
        new Vector3( 1, -1, -1),
        new Vector3( 1, -1,  1),
        new Vector3( 1,  1, -1),
        new Vector3( 1,  1,  1),
    };
    static int[][] faceVertices =
    {
        new int [] {0, 3, 2, 1},//-x
        new int [] {4, 7, 5, 6},//+x
        new int [] {0, 5, 1, 4},//-y
        new int [] {2, 7, 6, 3},//+y
        new int [] {0, 6, 4, 2},//-z
        new int [] {1, 7, 3, 5},//+z
    };

    public static Vector3[] GetFaceVertices(BlockFaceDirection faceDir, float scale, Vector3 transformPos)
    {
        Vector3[] fv = new Vector3[4];
        for (int i = 0; i < fv.Length; i++)
        {
            fv[i] = (localVertices[faceVertices[(int)faceDir][i]] * scale) + transformPos;// return an array that the vertices inside is in the order of the corresponding faceVertices array
        }
        return fv;
    }

    public static Vector3 GetDirectionFromEnum(BlockFaceDirection dir)
    {
        switch (dir)
        {
            case BlockFaceDirection.xNeg:
                return Vector3.left;
            case BlockFaceDirection.xPlus:
                return Vector3.right;
            case BlockFaceDirection.yNeg:
                return Vector3.down;
            case BlockFaceDirection.yPlus:
                return Vector3.up;
            case BlockFaceDirection.zNeg:
                return Vector3.back;
            case BlockFaceDirection.zPlus:
                return Vector3.forward;
            default:
                return Vector3.zero;
        }
    }
}

public enum BlockFaceDirection
{
    xNeg,
    xPlus,
    yNeg,
    yPlus,
    zNeg,
    zPlus,
}
