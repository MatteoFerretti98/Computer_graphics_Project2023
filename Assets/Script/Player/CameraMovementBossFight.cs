using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementBossFight : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    PlayerStats player;
    // Start is called before the first frame update
    void Start()
    {
        player = FindAnyObjectByType<PlayerStats>();
        target = player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = target.position + offset;
    }
}
