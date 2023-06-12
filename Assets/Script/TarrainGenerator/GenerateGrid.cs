using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GenerateGrid : MonoBehaviour
{
    public GameObject blockGameObject;

    public GameObject player;

    public int radius = 20;

    public int noiseHeight = 5;

    public float detailScale = 8f;

    public Vector3 startPosition = new Vector3(0,10,0);

    private Hashtable blockContainer = new Hashtable();

    public GameObject specialBlockPrefab; // Prefab da spawnare quando si verifica la condizione
    public GameObject SpawnEffect;

    public bool spawnSpecialBlock = false; // Variabile di condizione per determinare se spawnare l'oggetto speciale
    public bool specialBlockIsNotSpawned = true; // Variabile di condizione per verificare che l'oggetto sia già stato spawnato


    //private GameManager manager;


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

                SpawnBlock(pos);
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
                        
                        if (ShouldSpawnSpecialBlock() && specialBlockIsNotSpawned) // Controllo per la condizione di spawn dell'oggetto speciale
                        {
                            SpawnSpecialBlock(pos);
                            specialBlockIsNotSpawned = false;
                        }
                        else
                        {
                            SpawnBlock(pos);
                        }
                    }
                    else if (ShouldSpawnSpecialBlock() && specialBlockIsNotSpawned) // Controllo per la condizione di spawn dell'oggetto speciale
                    {
                        SpawnSpecialBlockInOldBlock(pos);
                        specialBlockIsNotSpawned = false;
                    }
                }
            }
        }
    }

    private void SpawnBlock(Vector3 position)
    {
        GameObject block = Instantiate(blockGameObject, position, Quaternion.identity) as GameObject;
        blockContainer.Add(position, block);
        block.transform.SetParent(this.transform);
    }

    private void SpawnSpecialBlock(Vector3 position)
    {
        GameObject specialBlock = Instantiate(specialBlockPrefab, new Vector3(20f, 1.5f, 20f), Quaternion.Euler(0,0f,0)) as GameObject;
        blockContainer.Add(position, specialBlock);
        specialBlock.transform.SetParent(this.transform);

        // Spawn dell'effetto di spawn
        if (SpawnEffect != null)
        {
            GameObject spawnEffectInstance = Instantiate(SpawnEffect, specialBlock.transform.position, Quaternion.identity);
            Destroy(spawnEffectInstance, 5f); // Distruzione dell'effetto dopo 3 secondi
        }
    }

    private void SpawnSpecialBlockInOldBlock(Vector3 position)
    {
        GameObject specialBlock = Instantiate(specialBlockPrefab, new Vector3(xPlayerLocation + 20f, 1.5f, zPlayerLocation + 20f), Quaternion.Euler(0, 0f, 0)) as GameObject;
        specialBlock.transform.SetParent(this.transform);
        // Spawn dell'effetto di spawn
        if (SpawnEffect != null)
        {
            GameObject spawnEffectInstance = Instantiate(SpawnEffect, specialBlock.transform.position, Quaternion.identity);
            Destroy(spawnEffectInstance, 5f); // Distruzione dell'effetto dopo 3 secondi
        }
    }

    private bool ShouldSpawnSpecialBlock()
    {
        // Inserire la condizione per spawnare l'arena del boss
        if (GameManager.instance.BossFightTime)
        {
            // Questo viene messo a true se il tempo è arrivato a 10 minuti
            spawnSpecialBlock = true;
        }
        return spawnSpecialBlock;
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