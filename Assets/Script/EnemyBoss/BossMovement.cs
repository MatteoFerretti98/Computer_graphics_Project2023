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
    public float fireballSpeed = 10f;
    public int maxAttacksPerPhase = 5;
    public float damageZoneRadius = 20f;
    private float maxHealth = 0;

    private int currentPhase = 1;
    private int currentAttackIndex = 0;
    private Animator animator;

    // Cooldown parameters
    private bool isCooldown = false;
    public float cooldownDuration = 3f;
    private float cooldownTimer = 0f;

    void Start()
    {
        boss = GetComponent<BossStats>();
        maxHealth = boss.currentHealth;
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
            UpdateCooldown();
        }
    }

    void HandleMovement()
    {
        if (currentPhase == 2)
        {
            // La fase 2 richiede al boss di muoversi verso il centro dell'arena senza seguire il giocatore
            Vector3 centerPosition = new Vector3(0f, transform.position.y, 0f);
            transform.position = Vector3.MoveTowards(transform.position, centerPosition, boss.currentMoveSpeed * Time.deltaTime);

            // Verifica se il boss ha raggiunto il centro dell'arena
            if (transform.position == centerPosition)
            {
                currentAttackIndex++;
            }
        }
        else if (currentPhase == 3)
        {
            // Terza fase: il boss rimane alle coordinate x=0 e z=0
            Vector3 targetPosition = new Vector3(0f, transform.position.y, 0f);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, boss.currentMoveSpeed * Time.deltaTime);
        }
        else
        {
            // Altrimenti, il boss segue il giocatore normalmente
            Vector3 targetPosition = player.position;
            targetPosition.y = transform.position.y; // Mantieni la stessa posizione lungo l'asse y
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, boss.currentMoveSpeed * Time.deltaTime);
        }
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

        if (boss.currentHealth < (maxHealth / 3f))
        {
            currentPhase = 3;
        }
        else if (boss.currentHealth < ((maxHealth / 3f) * 2f))
        {
            currentPhase = 2;
        }

        switch (currentPhase)
        {
            case 1:
                // Prima fase: il boss rincorre il giocatore
                break;
            case 2:
                // Seconda fase: il boss torna al centro dell'arena e poi spara palle di fuoco
                if (currentAttackIndex == 0)
                {
                    MoveToCenter();
                }
                else
                {
                    ShootFireball();
                }
                break;
            case 3:
                // Terza fase: il boss rimane alle coordinate x=0 e z=0 e crea zone di danno casuali
                StartCoroutine(CreateDamageZones());
                break;
        }
    }

    void MoveToCenter()
    {
        Vector3 centerPosition = new Vector3(0f, transform.position.y, 0f);
        transform.position = Vector3.MoveTowards(transform.position, centerPosition, boss.currentMoveSpeed * Time.deltaTime);
    }

    void ShootFireball()
    {
        if (!isCooldown)
        {
            // Calcola la direzione in cui sparare la palla di fuoco
            Vector3 direction = new Vector3(player.position.x - transform.position.x, 2f, player.position.z - transform.position.z).normalized;

            // Istanzia una palla di fuoco utilizzando il fireballPrefab
            GameObject fireball = Instantiate(fireballPrefab, transform.position, Quaternion.identity);

            // Applica una forza alla palla di fuoco nella direzione calcolata
            Rigidbody fireballRb = fireball.GetComponent<Rigidbody>();
            fireballRb.velocity = direction * fireballSpeed;

            // Incrementa l'indice dell'attacco corrente
            currentAttackIndex++;

            // Se l'indice dell'attacco corrente raggiunge un valore massimo, passa alla fase successiva
            if (currentAttackIndex >= maxAttacksPerPhase)
            {
                currentPhase++;
                currentAttackIndex = 0;
            }

            // Attiva il cooldown
            isCooldown = true;
            cooldownTimer = cooldownDuration;
        }
    }

    IEnumerator CreateDamageZones()
    {
        for (int i = 0; i < maxAttacksPerPhase; i++)
        {
            // Genera una posizione casuale all'interno del raggio specificato
            Vector3 randomPosition = new Vector3(transform.position.x + Random.insideUnitSphere.x * damageZoneRadius, transform.position.y+2f, transform.position.z + Random.insideUnitSphere.z * damageZoneRadius);

            // Istanzia la zona di danno utilizzando il damageZonePrefab nella posizione casuale generata
            GameObject damageZone = Instantiate(damageZonePrefab, randomPosition, Quaternion.identity);

            // Assegna eventuali attributi o comportamenti alla zona di danno, ad esempio la durata o il danno inflitto

            // Distruggi la zona di danno dopo 4 secondi
            Destroy(damageZone, 2f);

            yield return new WaitForSeconds(10f); // Attendere 2 secondi tra ogni zona di danno
        }

        // Incrementa l'indice dell'attacco corrente
        currentAttackIndex++;

        // Se l'indice dell'attacco corrente raggiunge un valore massimo, passa alla fase successiva
        if (currentAttackIndex >= maxAttacksPerPhase)
        {
            currentPhase++;
            currentAttackIndex = 0;
        }
    }

    void UpdateCooldown()
    {
        if (isCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
            {
                isCooldown = false;
            }
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
