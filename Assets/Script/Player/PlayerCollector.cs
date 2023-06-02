using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollector : MonoBehaviour
{
    PlayerStats player;
    SphereCollider playerCollector;
    public float pullSpeed;

    private void Start()
    {
        player = FindObjectOfType<PlayerStats>();
        playerCollector = GetComponent<SphereCollider>();
    }

    void Update()
    {
        playerCollector.radius = player.currentMagnet;
    }

    void OnTriggerEnter(Collider col)
    {
        //Check if the other game object has the ICollectible interface
        if (col.gameObject.TryGetComponent(out ICollectible collectible))         // DA DECOMMENTARE
        {
            //Pulling animation
            //Gets the Rigidbody component on the item
            Rigidbody rb = col.gameObject.GetComponent<Rigidbody>();
            //Vector3 pointing from the item to the player
            Vector3 forceDirection = (transform.position - col.transform.position).normalized;
            //Applies force to the item in the forceDirection with pullSpeed
            rb.AddForce(forceDirection * pullSpeed);

            //If it does, call the OnCollect method
            collectible.Collect();
        }
    }
}
