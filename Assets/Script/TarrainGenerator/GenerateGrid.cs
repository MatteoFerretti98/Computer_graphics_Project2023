using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Things changed;
        - Worldx, worldz replaced with a radius.
        - startPos changed to : startpos = vector3.zero AND made the player spawn on the startPos.
        - created a detailscale var as you want the seed to be consistent for the noise.
        - removed all the objectToSpawn stuff from this class.
*/
public class GenerateGrid : MonoBehaviour
{
    public GameObject blockGameObject;

    public GameObject player;

    public int radius = 20;

    public int noiseHeight = 5;

    public float detailScale = 8f;

    public Vector3 startPosition = new Vector3(0,10,0);

    private Hashtable blockContainer = new Hashtable();

    void Start()
    {
        player.transform.position = startPosition;

        for (int x = -radius; x < radius; x++)
        {
            for (int z = -radius; z < radius; z++)
            {
                Vector3 pos = new Vector3(x * 1 + startPosition.x,
                    generateNoise(x, z, detailScale) * noiseHeight,
                    z * 1 + startPosition.z);

                GameObject block = Instantiate(blockGameObject,
                pos,
                Quaternion.identity) as GameObject;

                blockContainer.Add(pos, block);

                block.transform.SetParent(this.transform);
            }
        }
    }


    void Update()
    {

        if (Mathf.Abs(xPlayerMove) >= 1 || Mathf.Abs(zPlayerMove) >= 1)
        {
            for (int x = -radius; x < radius; x++)
            {
                for (int z = -radius; z < radius; z++)
                {
                    Vector3 pos = new Vector3(x * 1 + xPlayerLocation,
                        generateNoise(x + xPlayerLocation, z + zPlayerLocation, detailScale) * noiseHeight,
                        z * 1 + zPlayerLocation);

                    if (!blockContainer.ContainsKey(pos))
                    {
                        GameObject block = Instantiate(blockGameObject,
                        pos,
                        Quaternion.identity) as GameObject;

                        blockContainer.Add(pos, block);

                        block.transform.SetParent(this.transform);
                    }
                }
            }
        }
    }

    public int xPlayerMove
    {
        get
        {
            return (int)(player.transform.position.x - startPosition.x);
        }
    }

    private int zPlayerMove
    {
        get
        {
            return (int)(player.transform.position.z - startPosition.z);
        }
    }

    private int xPlayerLocation
    {
        get
        {
            return (int)Mathf.Floor(player.transform.position.x);
        }
    }

    private int zPlayerLocation
    {
        get
        {
            return (int)Mathf.Floor(player.transform.position.z);
        }
    }

    private float generateNoise(int x, int z, float detailScale)
    {
        float xNoise = (x + this.transform.position.x) / detailScale;
        float zNoise = (z + this.transform.position.y) / detailScale;

        return Mathf.PerlinNoise(xNoise, zNoise);
    }

}