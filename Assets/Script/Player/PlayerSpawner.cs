using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSpawner : MonoBehaviour
{
    
    private string playerChoose;
    private string weaponChoose;
    [System.Serializable]
    public class CharacterGroup
    {
        public string characterID;
        public GameObject characterPrefab;
    }
    [System.Serializable]
    public class WeaponGroup
    {
        public string weaponID;
        public CharacterScriptableObject weapon;
    }
    public List<CharacterGroup> players;
    public List<WeaponGroup> weapons;

    public static PlayerSpawner instance;
    public List<Image> passiveItemUISlots = new List<Image>(6);
    public List<Image> weaponUISlots = new List<Image>(6);
    public List<InventoryManager.UpgradeUI> upgradeUIOptions = new List<InventoryManager.UpgradeUI>();
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            playerChoose = CharacterSelectorController.instance.characterSelected;
            weaponChoose = CharacterSelectorController.instance.weaponSelected;
            SpawnPlayer(playerChoose, weaponChoose);
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
        InventoryManager inventoryManager = FindObjectOfType<InventoryManager>();
        inventoryManager.upgradeUIOptions = upgradeUIOptions;
        inventoryManager.passiveItemUISlots = passiveItemUISlots;
        inventoryManager.weaponUISlots = weaponUISlots;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void SpawnPlayer(string playerChoose,string weaponChoose)
    {
        foreach (WeaponGroup weaponGroup in weapons)
        {
            if (weaponChoose == weaponGroup.weaponID)
            {
                CharacterSelectorController.instance.characterData = weaponGroup.weapon;
                break;
            }
        }
        foreach (CharacterGroup characterGroup in players)
        {
            if(playerChoose == characterGroup.characterID)
            {
                
                GameObject character = Instantiate(characterGroup.characterPrefab);//, Vector3.zero, Quaternion.identity);
                CameraMovement cameraMovement = FindObjectOfType<CameraMovement>();
                cameraMovement.target = character.transform;
                GenerateGrid generateGrid = FindObjectOfType<GenerateGrid>();
                generateGrid.player = character;
                GameManager gameManager = FindObjectOfType<GameManager>();
                gameManager.playerObject = character;
                break;
            }
        }

    }

    public void DestroySingleton()
    {
        instance = null;
        Destroy(gameObject);
    }

}