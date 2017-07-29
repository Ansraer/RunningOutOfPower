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
    public GenObjects[] interactables;

    public int playfieldLength;
    public int playfieldWidth;

    public GameObject playfield;

    private int[,] playfieldTiles;
    private int[,] playfieldInteractables;



    void Start()
    {
        genTestfield1();
    }

    void genTestfield1()
    {
        playfieldTiles = new int[playfieldLength, playfieldWidth];
        playfieldInteractables = new int[playfieldLength, playfieldWidth];
        //fill it with dirt
        for (int len = 0; len < playfieldLength; len++)
        {
            for (int wid = 0; wid < playfieldWidth; wid++)
            {
                playfieldTiles[len, wid] = 0;
            }
        }
        //add bushes (hopefully randomly later)
        playfieldInteractables[0, 0] = 1;
        for (int len = 0; len < playfieldLength; len++)
        {
            for (int wid = 0; wid < playfieldWidth; wid++)
            {
                int objectIndex = playfieldTiles[len, wid];
                GameObject tempTile = Instantiate(tiles[objectIndex].gameObject, new Vector3(len, wid, 0.5f), Quaternion.identity);
                tempTile.transform.parent = playfield.transform;
                if (objectIndex != 0)
                {
                    Instantiate(interactables[objectIndex].gameObject, new Vector3(len, wid, 0.0f), Quaternion.identity);
                }
                    
            }
        }

    }
	// Use this for initialization
	//void Start () {
	//	for(int i = 0; i < tiles.Length; i++)
 //       {
 //           tiles[i + 1] += tiles[i];
 //       }
 //       for(int i = 0; i < interactables.Length; i++)
 //       {
 //           interactables[i + 1] += interactables[i];
 //       }
 //   }

 //   void generateWorld()
 //   {
 //       Random randInt = new Random();
 //       for(int len = 0; len < playfieldLength; len++)
 //       {
 //           for(int wid=0;wid<playfieldWidth; wid++)
 //           {
 //               int randIndex = randInt.Next(0, 2048);
 //               GameObject thisTile = getTile(randIndex);
 //               GameObject thisInteractible = get
 //           }
 //       }
 //   }

 //   int getObject(int index, string objectClass)
 //   {
 //       if (objectClass == "tile")
 //       {

 //       }
 //       elif (objectClass == "interactible")
 //   }

    // Update is called once per frame
    void Update () {
		
	}
}
