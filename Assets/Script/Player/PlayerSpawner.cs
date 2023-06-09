using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static InventoryManager;

public class PlayerSpawner : MonoBehaviour
{
    
    private string playerChoose;
    private string weaponChoose;
    private string playerName;
    private Sprite playerImage;
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
    public List<Image> defensivePowerUpUISlots = new List<Image>(6);
    public List<InventoryManager.UpgradeUI> upgradeUIOptions = new List<InventoryManager.UpgradeUI>();
	//aggiunti Sam
	public TextMeshProUGUI upgradeDescriptionDisplay;
    public TextMeshProUGUI upgradeNameDisplay;
    public Button ChooseButton;
    public TextMeshProUGUI levelGame;
	
    public Transform cam;

    [Header("UI")]
    // public Image healthBar;
    public Image expBar;
    public TextMeshProUGUI levelText;

    public Slider[] sliders;
    public AudioManager audioManager;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            playerChoose = CharacterSelectorController.instance.characterSelected;
            weaponChoose = CharacterSelectorController.instance.weaponSelected;
            playerName = CharacterSelectorController.instance.nameCharacter;
            playerImage = CharacterSelectorController.instance.imageCharacter;
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
        PlayerStats playerStats = FindObjectOfType<PlayerStats>();
        HealthBarController healthBarController = FindObjectOfType<HealthBarController>();
        sliders = FindObjectsOfType<Slider>();
        AudioManager audioManager = FindObjectOfType<AudioManager>().GetComponent<AudioManager>();
        inventoryManager.upgradeUIOptions = upgradeUIOptions;
        inventoryManager.passiveItemUISlots = passiveItemUISlots;
        inventoryManager.weaponUISlots = weaponUISlots;
        inventoryManager.defensivePowerUpUISlots = defensivePowerUpUISlots;
		inventoryManager.upgradeDescriptionDisplay = upgradeDescriptionDisplay;
        inventoryManager.upgradeNameDisplay = upgradeNameDisplay;
        inventoryManager.ChooseButton = ChooseButton;
        inventoryManager.levelGame = levelGame;
        // playerStats.healthBar = healthBar;
        playerStats.levelText = levelText;
        playerStats.expBar = expBar;
        playerStats.playerName = playerName;
        playerStats.playerImage = playerImage;
        healthBarController.cam = cam;
        

        for(int i = 0; i < sliders.Length; i++)
        {
            Debug.Log(i);
            if (sliders[i].gameObject.name == "SFX Slider")
            {
                Debug.Log("ok SFX");
                Slider.SliderEvent sliderEvent = new Slider.SliderEvent();
                sliderEvent.AddListener(audioManager.gameObject.GetComponent<AudioManager>().SfXVolume);
                sliders[i].GetComponent<Slider>().onValueChanged = sliderEvent;
            }
            else if (sliders[i].gameObject.name == "Music Slider")
            {
                Debug.Log("ok Music");
                Slider.SliderEvent sliderEvent = new Slider.SliderEvent();
                sliderEvent.AddListener(audioManager.gameObject.GetComponent<AudioManager>().MusicVolume);
                sliders[i].GetComponent<Slider>().onValueChanged = sliderEvent;
            }
            
        }
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
