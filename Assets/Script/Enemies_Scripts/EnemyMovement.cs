using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    EnemyStats enemy;
    Transform player;


    void Start()
    {
        enemy = GetComponent<EnemyStats>();
        player = FindObjectOfType<AnimationAndMovementController>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        handleMovement();
        handleRotation();
    }

    void handleMovement()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.position, enemy.currentMoveSpeed * Time.deltaTime); //Constantly move the enemy towards the player
    }

    void handleRotation()
    {
        transform.LookAt(player);
    }
}
