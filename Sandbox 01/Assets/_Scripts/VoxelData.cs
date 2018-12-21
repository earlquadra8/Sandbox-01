using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelData
{

    bool[,,] patternData = new bool[,,]
    {// just the output voxels pattern
        {
            {true, true, true, true},
            {true, true, true, true},
            {true, true, true, true},
            {true, true, true, true}
        },
        {
            {true, false, false, true},
            {true, false, false, true},
            {true, true, true, true},
            {true, true, true, true}
        },
        {
            {false, false, false, false},
            {false, false, false, false},
            {false, true, true, false},
            {true, true, true, true}
        },

    };

    public int Width
    {
        get { return patternData.GetLength(1); }
    }
    public int Depth
    {
        get { return patternData.GetLength(2); }
    }
    public int Height
    {
        get { return patternData.GetLength(0); }
    }

    public bool GetCellPattern(int x, int y, int z)// return the element int the pattern array
    {
        return patternData[y, x, z];// as the building order is y - x - z, it is for the pattern reading only, therefore returning in other order is ok here
    }
    //                      tile position
    public bool GetNeighbour(int x, int y, int z, FaceDirection dir)// get the neighour in that dir
    {
        DataCoordintate offsetToCheck = offsets[(int)dir];// the direction we are checking for a neighbour
        DataCoordintate neigbourCoord = new DataCoordintate(x + offsetToCheck.x, y + offsetToCheck.y, z + offsetToCheck.z);

        if (neigbourCoord.y < 0 || neigbourCoord.y >= Height || neigbourCoord.x < 0 || neigbourCoord.x >= Width || neigbourCoord.z < 0 || neigbourCoord.z >= Depth)
        {
            return false;
        }
        else
        {
            return GetCellPattern(neigbourCoord.x, neigbourCoord.y ,neigbourCoord.z);
        }
    }

    struct DataCoordintate
    {
        public int y;
        public int x;
        public int z;

        public DataCoordintate(int x, int y, int z)
        {
            this.y = y;
            this.x = x;
            this.z = z;
        }
    }

    DataCoordintate[] offsets =
    {
        new DataCoordintate( 0,  0,  1),// North
        new DataCoordintate( 1,  0,  0),// East
        new DataCoordintate( 0,  0, -1),// South
        new DataCoordintate(-1,  0,  0),// West
        new DataCoordintate( 0,  1,  0),// Up
        new DataCoordintate( 0, -1,  0),// Down
    };

}

public enum FaceDirection
{
    North,
    East,
    South,
    West,
    Up,
    Down,
}
