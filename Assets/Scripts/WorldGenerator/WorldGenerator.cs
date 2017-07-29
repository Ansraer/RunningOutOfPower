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

    public int worldLength;
    public int worldWidth;

    public GameObject world;

    private int[,] worldGroundTiles;
    private int[,] worldObjects;



    void Start()
    {
        GenWorldGround();
        GenWorldObjects();


        InstantiateGround();
        InstantiateObjects();
    }

    void GenWorldGround()
    {
        worldGroundTiles = new int[worldLength, worldWidth];

        //fill it with dirt
        for (int len = 0; len < worldLength; len++)
        {
            for (int wid = 0; wid < worldWidth; wid++)
            {
                worldGroundTiles[len, wid] = 0;
            }
        }


    }

    void GenWorldObjects()
    {
        worldObjects = new int[worldLength, worldWidth];

        //fill it with dirt
        for (int len = 0; len < worldLength; len++)
        {
            for (int wid = 0; wid < worldWidth; wid++)
            {
                worldObjects[len, wid] = 0;
            }
        }


    }

    void InstantiateGround()
    {
        for (int len = 0; len < worldLength; len++)
        {
            for (int wid = 0; wid < worldWidth; wid++)
            {
                int objectIndex = worldGroundTiles[len, wid];
                GameObject tempTile = Instantiate(tiles[objectIndex].gameObject, new Vector3(wid, len, 0.5f), Quaternion.identity);
                tempTile.transform.parent = world.transform;

            }
        }
    }

    void InstantiateObjects()
    {
        for (int len = 0; len < worldLength; len++)
        {
            for (int wid = 0; wid < worldWidth; wid++)
            {
                int objectIndex = worldGroundTiles[len, wid];
                GameObject tempTile = Instantiate(objects[objectIndex].gameObject, new Vector3(wid, len, 0), Quaternion.identity);
                tempTile.transform.parent = world.transform;

            }
        }
    }



    void Update () {
		
	}
}
