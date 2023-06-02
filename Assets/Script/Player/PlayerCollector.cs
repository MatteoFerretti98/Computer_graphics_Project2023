using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollector : MonoBehaviour
{
    void OnTriggerEnter(Collider col)
    {
        //Check if the other game object has the ICollectible interface
        /*if (col.gameObject.TryGetComponent(out ICollectible collectible))         //DA DECOMMENTARE
        {
            //If it does, call the OnCollect method
            collectible.Collect();
        }*/
    }
}
