using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameStreamController : WeaponController
{
    GameObject fire;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    protected override void Attack()
    {
        base.Attack();

        // Instanzia il fuoco nella posizione del personaggio e con la direzione corretta
        fire = Instantiate(weaponData.Prefab, transform.position, Quaternion.identity);

        // Imposta la direzione iniziale del fuoco in base alla direzione del personaggio
        Vector3 characterDirection = transform.forward;
        fire.transform.forward = characterDirection;
    }

    protected override void Update()
    {
        base.Update();

        // Aggiorna la posizione del fuoco per seguire la posizione del personaggio
        if (fire != null)
        {
            //fire.transform.position = transform.position;
            fire.transform.position = new Vector3 (transform.position.x, transform.position.y + 0.5f, transform.position.z);

            // Ruota la fiamma nella direzione in cui è ruotato il personaggio
            fire.transform.rotation = transform.rotation;
        }
    }
}
