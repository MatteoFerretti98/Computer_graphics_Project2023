using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FreezeEnemyController : DefensivePowerUpController
{
    public float durataCongelamento = 10f;
    public Material textureCongelata;
    public string enemyTag = "Enemy";
    public string bossTag = "EnemyBoss";

    private bool inCongelamento = false;
    private float timer;
    private Dictionary<GameObject, Material> originalMaterials = new Dictionary<GameObject, Material>();

    protected override void Defend()
    {
        base.Defend();
        GameObject spawnedFreezeEffect = Instantiate(defensivePowerUpData.Prefab);
        spawnedFreezeEffect.transform.position = transform.position; //Assign the position to be the same as this object which is parented to the player
        spawnedFreezeEffect.transform.parent = transform; // So that is spawns below this object
        CongelaOggetti();
    }

    private void CongelaOggetti()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);

        foreach (GameObject enemy in enemies)
        {
            Rigidbody enemyRigidbody = enemy.GetComponent<Rigidbody>();
            if (enemyRigidbody != null)
            {
                enemyRigidbody.isKinematic = true;
            }

            Renderer renderer = enemy.GetComponentInChildren<Renderer>();
            if (renderer != null)
            {
                // Salva il materiale originale
                if (!originalMaterials.ContainsKey(enemy))
                {
                    originalMaterials[enemy] = renderer.material;
                }

                renderer.material = textureCongelata;
            }

            EnemyMovement enemyMovement = enemy.GetComponent<EnemyMovement>();
            if (enemyMovement != null)
            {
                enemyMovement.enabled = false; // Disabilita temporaneamente l'EnemyMovement
                StartCoroutine(EnableEnemyMovement(enemyMovement)); // Riabilita l'EnemyMovement dopo il congelamento
            }
            EnemyChestMovement enemyChestMovement = enemy.GetComponent<EnemyChestMovement>();
            if (enemyChestMovement != null)
            {
                enemyChestMovement.enabled = false; // Disabilita temporaneamente l'EnemyMovement
                StartCoroutine(EnableEnemyMovement(enemyChestMovement)); // Riabilita l'EnemyMovement dopo il congelamento
            }

            Animator animator = enemy.GetComponent<Animator>();
            if (animator != null)
            {
                animator.enabled = false;
            }
        }

        inCongelamento = true;
        timer = 0f;
        StartCoroutine(RestoreTextures());
    }

    private IEnumerator EnableEnemyMovement(EnemyMovement enemyMovement)
    {
        yield return new WaitForSeconds(durataCongelamento);

        if (enemyMovement != null) // Controlla se l'oggetto EnemyMovement esiste ancora
        {
            enemyMovement.enabled = true; // Riabilita l'EnemyMovement dopo il congelamento
        }
    }

    private IEnumerator EnableEnemyMovement(EnemyChestMovement enemyChestMovement)
    {
        yield return new WaitForSeconds(durataCongelamento);

        if (enemyChestMovement != null) // Controlla se l'oggetto EnemyMovement esiste ancora
        {
            enemyChestMovement.enabled = true; // Riabilita l'EnemyMovement dopo il congelamento
        }
    }


    private IEnumerator RestoreTextures()
    {
        yield return new WaitForSeconds(durataCongelamento);

        List<GameObject> destroyedEnemies = new List<GameObject>(); // Lista per tenere traccia dei nemici distrutti

        foreach (GameObject enemy in originalMaterials.Keys)
        {
            if (enemy == null)
            {
                destroyedEnemies.Add(enemy); // Aggiungi nemici distrutti alla lista
                continue; // Salta al prossimo nemico se l'oggetto nemico � nullo
            }

            Renderer renderer = enemy.GetComponentInChildren<Renderer>();
            if (renderer != null)
            {
                // Ripristina il materiale originale solo se l'oggetto nemico esiste ancora
                if (originalMaterials.TryGetValue(enemy, out Material originalMaterial))
                {
                    renderer.material = originalMaterial;
                }
            }

            Animator animator = enemy.GetComponent<Animator>();
            if (animator != null)
            {
                animator.enabled = true;
            }
        }

        // Rimuovi i materiali dei nemici distrutti dal dizionario
        foreach (GameObject destroyedEnemy in destroyedEnemies)
        {
            if (originalMaterials.ContainsKey(destroyedEnemy))
            {
                originalMaterials.Remove(destroyedEnemy);
            }
        }

        inCongelamento = false;
    }


}
