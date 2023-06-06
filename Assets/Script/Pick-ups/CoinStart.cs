using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinStar : MonoBehaviour, ICollectible
{
    // The amount of money to add when this item is collected
    public int CoinToAdd;

    public void Collect()
    {
        PlayerStats stats = FindObjectOfType<PlayerStats>();

        //QUI VA AGGIUNTA LA RIGA PER L'INCREMENTO DELLE MONETE
        //stats.RestoreHealth(healthToRestore);
        Destroy(gameObject);
    }
}
