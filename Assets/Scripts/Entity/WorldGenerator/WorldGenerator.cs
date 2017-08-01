using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WorldGenerator : MonoBehaviour {

    [System.Serializable]
    public struct GenObjects
    {
        public GameObject gameObject;
        public int probability;
    }

    //the probability numbers in the whole array have to add up to 2048 for the gen to work correctly
    public GenObjects[] tiles;
    public GenObjects[] objects;

    public GameObject wall;

    public int worldHeight;
    public int worldWidth;

    public GameObject world;

    public float freeSpaceRadius = 8;

    public GameObject centerBuilding;

    private int[,] worldGroundTiles;
    private int[,] worldObjects;

    public int astarResolution = 2;

    void Start()
    {
        GenWorldGround();
        GenWorldOre();

        GenWorldObjects();
        GenWorldWall();


        InstantiateGround();
        InstantiateObjects();

        //make sure that 0 is at the center of the map
        world.transform.position = new Vector3(-worldWidth / 2, -worldHeight / 2, 0);

        GameObject tempTile = Instantiate(centerBuilding, new Vector3(0, 0, 0), Quaternion.identity);

        AstarPath.active.data.gridGraph.SetDimensions(worldWidth*this.astarResolution, worldHeight * this.astarResolution, 1f/this.astarResolution);
        AstarPath.active.Scan();

    }

    void GenWorldGround()
    {
        worldGroundTiles = new int[worldHeight, worldWidth];

        //fill it with dirt
        for (int len = 0; len < worldHeight; len++)
        {
            for (int wid = 0; wid < worldWidth; wid++)
            {
                worldGroundTiles[len, wid] = 0;
            }
        }


    }

    void GenWorldOre()
    {
        worldGroundTiles = new int[worldHeight, worldWidth];

        //fill it with dirt

        for (int id = 1; id<=this.tiles.Length-1; id++) {

        for (int len = 0; len < worldHeight; len++)
        {
            for (int wid = 0; wid < worldWidth; wid++)
            {
                if (Random.Range(0f, 100f) < this.tiles[id].probability)





                    for (int lenOffset = 0; lenOffset < 5; lenOffset++)
                    {
                        for (int widOffset = 0; widOffset < 5; widOffset++)
                        {
                            float lenOre = len + lenOffset - 2;
                            float widOre = wid + widOffset - 2;

                            if (lenOre >= 0 && lenOre < worldHeight)
                            {
                                if (widOre >= 0 && widOre < worldWidth)
                                {
                                        if (Random.Range(0f, 100f) < 80)
                                        worldGroundTiles[len, wid] = id;
                                }
                            }
                        }
                    }



            }
        }

        }
    }

    void GenWorldObjects()
    {
        worldObjects = new int[worldHeight, worldWidth];


        for (int id = 0; id <= this.objects.Length - 1; id++)
        {

            for (int len = 0; len < worldHeight; len++)
            {
                for (int wid = 0; wid < worldWidth; wid++)
                {

                    worldObjects[len, wid] = -1;

                    if (Random.Range(0f, 100f) < this.objects[id].probability)
                    {
                        worldObjects[len, wid] = id;
                    }



                    //clearing up center
                    float distance = Mathf.Sqrt(Mathf.Pow(len - worldHeight / 2, 2) + Mathf.Pow(wid - worldWidth / 2, 2));
                    if (distance < freeSpaceRadius)
                    {
                        worldObjects[len, wid] = -1;
                    }

                }
            }

        }

    }




    void GenWorldWall()
    {
       
            for (int len = 0; len < worldHeight; len++)
            {
                worldObjects[len, 0] = -2;
                worldObjects[len, worldWidth-1] = -2;
            
            }

            for (int wid = 0; wid < worldWidth; wid++)
            {

            worldObjects[0, wid] = -2;
            worldObjects[worldHeight-1, wid] = -2;

            }

    }

    void InstantiateGround()
    {
        for (int len = 0; len < worldHeight; len++)
        {
            for (int wid = 0; wid < worldWidth; wid++)
            {
                int objectIndex = worldGroundTiles[len, wid];
                if (objectIndex != -1) {
                    GameObject tempTile = Instantiate(tiles[objectIndex].gameObject, new Vector3(wid, len, 0.5f), Quaternion.identity);
                    tempTile.transform.parent = world.transform;
                }
            }
        }
    }

    void InstantiateObjects()
    {
        for (int len = 0; len < worldHeight; len++)
        {
            for (int wid = 0; wid < worldWidth; wid++)
            {
                if (worldObjects[len, wid] != -1)
                {
                    int objectIndex = worldObjects[len, wid];

                    if (objectIndex == -2)
                    {
                        GameObject tempTile = Instantiate(this.wall, new Vector3(wid, len, 0), Quaternion.identity);
                        tempTile.transform.parent = world.transform;
                    } else
                    {
                        GameObject tempTile = Instantiate(objects[objectIndex].gameObject, new Vector3(wid, len, 0), Quaternion.identity);
                        tempTile.transform.parent = world.transform;
                    }


                }
            }
        }
    }



    void Update () {
		
	}
}
