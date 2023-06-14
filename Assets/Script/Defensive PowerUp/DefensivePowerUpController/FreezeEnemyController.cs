using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeEnemyController : DefensivePowerUpController
{
    public float durataCongelamento = 10f;
    public Material textureCongelata;
    public string enemyTag = "Enemy";

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
        enemyMovement.enabled = true; // Riabilita l'EnemyMovement dopo il congelamento
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
                continue; // Salta al prossimo nemico se l'oggetto nemico è nullo
            }

            Renderer renderer = enemy.GetComponentInChildren<Renderer>();
            if (renderer != null)
            {
                // Ripristina il materiale originale
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
            originalMaterials.Remove(destroyedEnemy);
        }

        inCongelamento = false;
    }
}
