using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    EnemyStats enemy;
    Transform player;
    bool isFrozen = false;
    Vector3 originalPosition;

    void Start()
    {
        enemy = GetComponent<EnemyStats>();
        player = FindObjectOfType<AnimationAndMovementController>().transform;
        originalPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isFrozen)
        {
            HandleMovement();
            HandleRotation();
        }
    }

    void HandleMovement()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.position, enemy.currentMoveSpeed * Time.deltaTime); //Constantly move the enemy towards the player
    }

    void HandleRotation()
    {
        transform.LookAt(player);
    }

    public void FreezeEnemy(float duration)
    {
        StartCoroutine(FreezeCoroutine(duration));
    }

    IEnumerator FreezeCoroutine(float duration)
    {
        isFrozen = true;
        yield return new WaitForSeconds(duration);
        isFrozen = false;
        transform.position = originalPosition;
    }
}
