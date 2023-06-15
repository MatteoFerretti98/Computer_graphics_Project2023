using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static InventoryManager;

public class HealthBarBossFight : MonoBehaviour
{

    public static HealthBarBossFight instance;
	
    public Transform cam;

    

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); 
            AddReferenceObjects();
        }
        else
        {
            Debug.LogWarning("EXTRA " + this + " DELETED");
            Destroy(gameObject);
        }
    }

    private void AddReferenceObjects()
    {
        HealthBarController healthBarController = FindObjectOfType<HealthBarController>();
        healthBarController.cam = cam;
        
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void DestroySingleton()
    {
        instance = null;
        Destroy(gameObject);
    }

}
