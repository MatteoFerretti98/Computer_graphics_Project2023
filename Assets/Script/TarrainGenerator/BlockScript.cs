using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
  You need make your 'blockGameObject' (e.g - the cube thats spawned) into a prefab and attach this script to it.
  Once attached; you need to populate the objectToSpawn with the object that you are spawning (e.g - trees, rocks, etc)
  Play around with the values of probability AND yAxis restrictions or just use probability if you wish 
  (just used those to test but they seem to work pretty well)
    - Rus
*/

public class BlockScript : MonoBehaviour
{
    public List<GameObject> objectsToSpawn;

    private void Start()
    {
        foreach (GameObject objectToSpawn in objectsToSpawn)
        {
            if (ShouldSpawnObject())
            {
                SpawnObject(objectToSpawn);
            }
        }
    }

    private bool ShouldSpawnObject()
    {
        return Random.Range(0, 100) >= 99 && Random.Range(0, 100) <= 100;
    }

    private void SpawnObject(GameObject objectToSpawn)
    {
        Instantiate(objectToSpawn, new Vector3(
            transform.position.x,
            transform.position.y + GetSpawnOffset(),
            transform.position.z), Quaternion.identity);
    }

    private float GetSpawnOffset()
    {
        return ShouldApplyYAxisRestrictions() ? 1.5f : 0.5f;
    }

    private bool ShouldApplyYAxisRestrictions()
    {
        return transform.position.y <= 2.0f && transform.position.y >= 1.5f;
    }
}