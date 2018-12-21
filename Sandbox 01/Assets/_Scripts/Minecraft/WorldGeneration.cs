using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGeneration : MonoBehaviour
{
    public GameObject player;

    public int sizeX = 1;
    public int sizeZ = 1;

    public int groundHeight;
    public float terDetail = 20;
    public float terHeight = 20;
    int seed;

    public GameObject[] blocks;

	void Start ()
    {
        seed = Random.Range(100000, 999999);
        GenerateTerrain();
	}

    void Update ()
    {
		
	}

    private void GenerateTerrain()
    {
        for (int x = 0; x < sizeX; x++)
        {
            for (int z = 0; z < sizeZ; z++)
            {
                int maxY = (int)(Mathf.PerlinNoise((x * 0.5f + seed) / terDetail, (z * 0.5f + seed) / terDetail) * terHeight);
                maxY += groundHeight;
                GameObject grass = Instantiate(blocks[0], new Vector3(x, maxY, z), Quaternion.identity);
                grass.transform.SetParent(GameObject.Find("Environment").transform);

                // fill the space with stone and dirt
                for (int y = 0; y < maxY; y++)
                {
                    int dirtLayer = Random.Range(1, 5);
                    if (y >= maxY - dirtLayer)
                    {
                        GameObject dirt = Instantiate(blocks[2], new Vector3(x, y, z), Quaternion.identity);
                        dirt.transform.SetParent(GameObject.Find("Environment").transform);
                    }
                    else
                    {
                        GameObject stone = Instantiate(blocks[1], new Vector3(x, y, z), Quaternion.identity);
                        stone.transform.SetParent(GameObject.Find("Environment").transform);
                    }
                }

                if (x == (int)(sizeX * 0.5f) && z == (int)(sizeZ * 0.5f))
                {
                    Instantiate(player, new Vector3(x, maxY + 3, z), Quaternion.identity);
                }
            }
        }
    }

}
