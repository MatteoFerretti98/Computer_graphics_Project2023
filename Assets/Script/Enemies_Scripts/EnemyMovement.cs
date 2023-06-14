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
        Vector3 targetPosition = player.position;
        targetPosition.y = transform.position.y; // Mantieni la stessa posizione lungo l'asse y
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, enemy.currentMoveSpeed * Time.deltaTime);
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
