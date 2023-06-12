using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UICoins : MonoBehaviour
{
    public TextMeshProUGUI playerCoins;
    // Start is called before the first frame update
    void Start()
    {
        if (PersistenceManager.PersistenceInstance != null)
            playerCoins.text = PersistenceManager.PersistenceInstance.Coins.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void incrementCoins(int coins)
    {
        playerCoins.text = (coins + 1).ToString();
    }
}
