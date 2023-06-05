using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropRateManager : MonoBehaviour
{
    [System.Serializable]   //Serialize the class
    public class Drops
    {
        public string name;
        public GameObject itemPrefab;
        public float dropRate;
    }

    public List<Drops> drops;


    void OnDestroy()
    {
        float randomNumber = UnityEngine.Random.Range(0f, 100f);
        List<Drops> possibleDrops = new List<Drops>();

        foreach (Drops rate in drops)
        {
            if (randomNumber <= rate.dropRate)
            {
                possibleDrops.Add(rate);
            }
        }

        // Check if there are possible drops
        if (possibleDrops.Count > 0)
        {
            Drops selectedDrop = possibleDrops[UnityEngine.Random.Range(0, possibleDrops.Count)];
            Vector3 newPosition = transform.position + Vector3.up; // Add 1 to y position
            GameObject instantiatedItem = Instantiate(selectedDrop.itemPrefab, newPosition, Quaternion.Euler(-90f, 0f, 0f));
            instantiatedItem.transform.localScale = new Vector3(0.015f, 0.015f, 0.015f); // Set scale to 0.015
        }
    }


}