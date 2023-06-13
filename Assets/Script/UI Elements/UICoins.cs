using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UICoins : MonoBehaviour
{
    public TextMeshProUGUI playerCoins;
    public bool gameCoins = false;
    // Start is called before the first frame update
    void Start()
    {
        if (PersistenceManager.PersistenceInstance != null && !gameCoins)
            playerCoins.text = PersistenceManager.PersistenceInstance.Coins.ToString();
        else
            playerCoins.text = "0";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
