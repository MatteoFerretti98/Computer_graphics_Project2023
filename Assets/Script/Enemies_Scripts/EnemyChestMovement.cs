using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChestMovement : MonoBehaviour
{
    EnemyStats enemy;
    Transform player;
    bool isIdle = true;
    bool isFrozen = false;
    Vector3 originalPosition;

    // Aggiungi variabili per le animazioni
    Animator animator;
    static readonly int MoveSpeedHash = Animator.StringToHash("MoveSpeed");
    static readonly int AttackTriggerHash = Animator.StringToHash("ChestMonster");

    void Start()
    {
        enemy = GetComponent<EnemyStats>();
        player = FindObjectOfType<AnimationAndMovementController>().transform;
        originalPosition = transform.position;

        // Ottieni il componente Animator
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!isFrozen)
        {
            if (isIdle)
            {
                HandleIdle();
            }
            else
            {
                HandleMovement();
                HandleRotation();
            }
        }
    }

    void HandleIdle()
    {
        // Calcola la distanza tra il nemico e il giocatore
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= 5f)
        {
            // Il giocatore si è avvicinato, passa al controller con le animazioni
            isIdle = false;
            animator.SetBool("Idle", false);
            //animator.SetFloat(MoveSpeedHash, enemy.currentMoveSpeed);
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

        // Reimposta lo stato di idle
        isIdle = true;
        animator.SetBool("Idle", true);
    }
}
