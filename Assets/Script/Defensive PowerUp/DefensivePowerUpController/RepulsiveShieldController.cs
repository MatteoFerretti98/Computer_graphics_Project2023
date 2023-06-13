using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepulsiveShieldController : DefensivePowerUpController
{
    public float repulsionForce = 10f;
    public GameObject repulsor;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    protected override void Defend()
    {
        base.Defend();

        Collider[] colliders = Physics.OverlapSphere(transform.position, 1.5f);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                Vector3 direction = collider.transform.position - transform.position;
                direction.y = 0f; // Ignore the vertical direction
                direction.Normalize();

                Rigidbody rb = collider.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddForce(direction * repulsionForce, ForceMode.Impulse);
                }
            }
        }

        // Move the repulsor object in the opposite direction
        Vector3 repulsorDirection = -transform.forward;
        repulsorDirection.y = 0f; // Ignore the vertical direction
        repulsorDirection.Normalize();

        repulsor.transform.position += repulsorDirection * repulsionForce * Time.deltaTime;
    }
}
