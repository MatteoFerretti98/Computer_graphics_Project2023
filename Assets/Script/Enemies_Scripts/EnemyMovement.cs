using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    Transform player;
    public float moveSpeed;
    Vector3 currentMovement;
    public float rotationSpeed;

    // Start is called before the first frame update
    void Start()
    {
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
        currentMovement = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime); //Constantly move the enemy towards the player
        transform.position = currentMovement;
    }

    void handleRotation()
    {
        Vector3 positionToLookAt;
        // the change in position our character should point to
        positionToLookAt.x = currentMovement.x;
        positionToLookAt.y = 0f;
        positionToLookAt.z = currentMovement.z;
        // the current rotation of our character
        Quaternion currentRotation = transform.rotation;
        // crates a new rotation based on where the player is currently pressing
        Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
        // rotate the character to face the positionToLookAt
        transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
