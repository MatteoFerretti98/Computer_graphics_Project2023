using UnityEngine;

public class BossController : MonoBehaviour
{
    public Transform player;
    public GameObject fireballPrefab;
    public GameObject damageZonePrefab;
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    public float attackRadius = 20f;

    public float maxHealth = 100f;
    private float currentHealth;

    private int currentPhase = 1;
    private int currentAttackIndex = 0;
    private Animator animator;
    PlayerStats playerInput;

    public float bossDamage = 100f;

    private void Start()
    {
        playerInput = FindAnyObjectByType<PlayerStats>();
        player = playerInput.transform;
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    private void Update()
    {
        // Calcola la direzione verso il giocatore
        Vector3 directionToPlayer = player.position - transform.position;
        directionToPlayer.y = 0f; // Ignora la differenza di altezza

        // Ruota verso il giocatore
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // Controlla la distanza dal giocatore solo nella prima fase
        if (currentPhase == 1)
        {
            float distanceToPlayer = directionToPlayer.magnitude;
            if (distanceToPlayer > attackRadius)
            {
                // Sposta il boss verso il giocatore con la velocità specificata
                transform.position += transform.forward * moveSpeed * Time.deltaTime;

                // Attiva l'animazione "walk"
                animator.SetBool("IsWalking", true);
            }
            else
            {
                // Disattiva l'animazione "walk"
                animator.SetBool("IsWalking", false);

                // Attacca il giocatore
                Attack();
            }
        }
        else
        {
            // Disattiva l'animazione "walk"
            animator.SetBool("IsWalking", false);

            // Attacca il giocatore
            Attack();
        }
    }

    private void Attack()
    {
        switch (currentPhase)
        {
            case 1:
                // Fase 1: Nessun attacco specifico
                break;
            case 2:
                // Fase 2: Lancia sfere di fuoco
                animator.SetTrigger("AttackFireball");
                break;
            case 3:
                // Fase 3: Crea zone di danno
                animator.SetTrigger("AttackDamageZone");
                break;
            default:
                break;
        }
    }

    public void LaunchFireball()
    {
        // Crea e lancia una sfera di fuoco nella direzione del giocatore
        Vector3 directionToPlayer = player.position - transform.position;
        GameObject fireball = Instantiate(fireballPrefab, transform.position, Quaternion.identity);
        fireball.GetComponent<Rigidbody>().velocity = directionToPlayer.normalized * 10f;
    }

    public void CreateDamageZone()
    {
        // Crea una zona di danno sul terreno in una posizione casuale
        float randomX = Random.Range(-10f, 10f);
        float randomZ = Random.Range(-10f, 10f);
        Vector3 spawnPosition = new Vector3(randomX, 0f, randomZ) + transform.position;
        Instantiate(damageZonePrefab, spawnPosition, Quaternion.identity);
    }

    public void TakeDamage(float dmg)
    {
        currentHealth -= dmg;
        Debug.LogWarning("damage:" + dmg);
        if (currentHealth <= 0)
        {
            Debug.LogWarning("Death Monster");
            Kill();
        }
    }

    public void Kill()
    {
        Destroy(gameObject);
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

        // Verifica se il boss deve passare alla fase successiva
        int newPhase = Mathf.CeilToInt((float)(maxHealth - currentHealth) / (float)(maxHealth / 3));
        if (newPhase > currentPhase)
        {
            currentPhase = newPhase;
            StartNextPhase();
        }
    }

    protected virtual void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            PlayerStats player = col.gameObject.GetComponent<PlayerStats>();
            player.TakeDamage(bossDamage);
        }
    }

    void OnCollisionStay(Collision col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            PlayerStats player = col.gameObject.GetComponent<PlayerStats>();
            player.TakeDamage(bossDamage);
        }
    }

    public void StartNextPhase()
    {
        // Implementa il comportamento specifico per la nuova fase
        switch (currentPhase)
        {
            case 2:
                // Fase 2
                moveSpeed = 5f;
                attackRadius = 9999f;
                break;
            case 3:
                // Fase 3
                moveSpeed = 3f;
                attackRadius = 9999f;
                break;
            default:
                // Fase 1 (predefinita)
                moveSpeed = 5f;
                attackRadius = 20f;
                break;
        }
    }
}