using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropRateManagerFinalChest : MonoBehaviour
{
    [System.Serializable]
    public class Drops
    {
        public string name;
        public GameObject itemPrefab;
        public GameObject sparklePrefab;
        public float dropRate;
    }

    public List<Drops> drops;

    void OnDestroy()
    {
        if (!gameObject.scene.isLoaded)
        {
            return;
        }

        float randomNumber = UnityEngine.Random.Range(0f, 100f);
        List<Drops> possibleDrops = new List<Drops>();

        foreach (Drops rate in drops)
        {
            if (randomNumber <= rate.dropRate)
            {
                possibleDrops.Add(rate);
            }
        }

        if (possibleDrops.Count > 0)
        {
            Drops selectedDrop = possibleDrops[UnityEngine.Random.Range(0, possibleDrops.Count)];
            Vector3 newPosition = new Vector3(transform.position.x, 1f, transform.position.z);

            // Instantiate the sparkle prefab
            GameObject sparkle = Instantiate(selectedDrop.sparklePrefab, new Vector3(newPosition.x, newPosition.y+0.5f, newPosition.z), Quaternion.identity);

            // Instantiate the item prefab
            GameObject instantiatedItem = Instantiate(selectedDrop.itemPrefab, newPosition, Quaternion.Euler(0f, 230f, 0f));
            instantiatedItem.transform.localScale = new Vector3(0.015f, 0.015f, 0.015f);

            // Destroy the sparkle object after a certain delay
            float sparkleDuration = 3f; // Adjust the duration as needed
            Destroy(sparkle, sparkleDuration);
        }
    }
}
