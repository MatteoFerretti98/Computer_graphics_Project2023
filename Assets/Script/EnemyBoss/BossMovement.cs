using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMovement : MonoBehaviour
{
    BossStats boss;
    Transform player;
    bool isFrozen = false;
    Vector3 originalPosition;

    // Boss Attack parameters
    public GameObject fireballPrefab;
    public GameObject damageZonePrefab;
    public float fireballSpeed = 5f;
    public int maxAttacksPerPhase = 10;
    public float damageZoneRadius = 20f;

    private int currentPhase = 1;
    private int currentAttackIndex = 0;
    private Animator animator;

    void Start()
    {
        boss = GetComponent<BossStats>();
        player = FindObjectOfType<AnimationAndMovementController>().transform;
        originalPosition = transform.position;
    }

    void Update()
    {
        if (!isFrozen)
        {
            HandleMovement();
            HandleRotation();
            HandleAttacks();
        }
    }

    void HandleMovement()
    {
        Vector3 targetPosition = player.position;
        targetPosition.y = transform.position.y; // Mantieni la stessa posizione lungo l'asse y
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, boss.currentMoveSpeed * Time.deltaTime);
    }

    void HandleRotation()
    {
        transform.LookAt(player);
    }

    void HandleAttacks()
    {
        if (boss.currentHealth <= 0)
        {
            // Il boss è stato sconfitto
            return;
        }

        switch (currentPhase)
        {
            case 1:
                // Prima fase: il boss rincorre il giocatore
                break;
            case 2:
                // Seconda fase: il boss spara palle di fuoco nella direzione del giocatore
                ShootFireball();
                break;
            case 3:
                // Terza fase: il boss crea zone di danno casuali all'interno di un raggio di 20 blocchi
                CreateDamageZone();
                break;
        }
    }

    void ShootFireball()
    {
        // Calcola la direzione in cui sparare la palla di fuoco
        Vector3 direction = (player.position - transform.position).normalized;

        // Istanzia una palla di fuoco utilizzando il fireballPrefab
        GameObject fireball = Instantiate(fireballPrefab, transform.position, Quaternion.identity);

        // Applica una forza alla palla di fuoco nella direzione calcolata
        Rigidbody fireballRb = fireball.GetComponent<Rigidbody>();
        fireballRb.velocity = direction * fireballSpeed;

        // Assegna eventuali attributi o comportamenti alla palla di fuoco, ad esempio il danno causato al giocatore

        // Incrementa l'indice dell'attacco corrente
        currentAttackIndex++;

        // Se l'indice dell'attacco corrente raggiunge un valore massimo, passa alla fase successiva
        if (currentAttackIndex >= maxAttacksPerPhase)
        {
            currentPhase++;
            currentAttackIndex = 0;
        }
    }

    void CreateDamageZone()
    {
        // Genera una posizione casuale all'interno del raggio specificato
        Vector3 randomPosition = transform.position + Random.insideUnitSphere * damageZoneRadius;

        // Istanzia la zona di danno utilizzando il damageZonePrefab nella posizione casuale generata
        GameObject damageZone = Instantiate(damageZonePrefab, randomPosition, Quaternion.identity);

        // Assegna eventuali attributi o comportamenti alla zona di danno, ad esempio la durata o il danno inflitto

        // Incrementa l'indice dell'attacco corrente
        currentAttackIndex++;

        // Se l'indice dell'attacco corrente raggiunge un valore massimo, passa alla fase successiva
        if (currentAttackIndex >= maxAttacksPerPhase)
        {
            currentPhase++;
            currentAttackIndex = 0;
        }
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
