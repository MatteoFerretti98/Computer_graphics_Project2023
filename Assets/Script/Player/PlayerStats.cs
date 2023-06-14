using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public CharacterScriptableObject characterData;
    //Current stats
    public float currentHealth;
    float currentRecovery;
    float currentMoveSpeed;
    float currentMight;
    float currentProjectileSpeed;
    float currentMagnet;
    int currentCoins;

    #region Current Stats Properties
    public float CurrentHealth
    {
        get { return currentHealth; }
        set
        {
            //Check if the value has changed
            if (currentHealth != value)
            {
                if (value >= characterData.MaxHealth) currentHealth = characterData.MaxHealth;
                else currentHealth = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentHealthDisplay.text = "" + currentHealth;
                }
                //Add any additional logic here that needs to be executed when the value changes
            }
        }
    }

    public float CurrentRecovery
    {
        get { return currentRecovery; }
        set
        {
            //Check if the value has changed
            if (currentRecovery != value)
            {
                currentRecovery = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentRecoveryDisplay.text = "" + currentRecovery;
                }
                //Update the real time value of the stat
                //Add any additional logic here that needs to be executed when the value changes
            }
        }
    }

    public float CurrentMoveSpeed
    {
        get { return currentMoveSpeed; }
        set
        {
            //Check if the value has changed
            if (currentMoveSpeed != value)
            {
                currentMoveSpeed = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentMoveSpeedDisplay.text = "" + currentMoveSpeed;
                }
                //Update the real time value of the stat
                //Add any additional logic here that needs to be executed when the value changes
            }
        }
    }

    public float CurrentMight
    {
        get { return currentMight; }
        set
        {
            //Check if the value has changed
            if (currentMight != value)
            {
                currentMight = value;

                if (GameManager.instance != null)
                {
                    GameManager.instance.currentMightDisplay.text = "" + currentMight;
                }

                //Update the real time value of the stat
                //Add any additional logic here that needs to be executed when the value changes
            }
        }
    }

    public float CurrentProjectileSpeed
    {
        get { return currentProjectileSpeed; }
        set
        {
            //Check if the value has changed
            if (currentProjectileSpeed != value)
            {
                currentProjectileSpeed = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentProjectileSpeedDisplay.text = "" + currentProjectileSpeed;
                }
                //Update the real time value of the stat
                //Add any additional logic here that needs to be executed when the value changes
            }
        }
    }

    public float CurrentMagnet
    {
        get { return currentMagnet; }
        set
        {
            //Check if the value has changed
            if (currentMagnet != value)
            {
                currentMagnet = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentMagnetDisplay.text = "" + currentMagnet;
                }
                //Update the real time value of the stat
                //Add any additional logic here that needs to be executed when the value changes
            }
        }
    }

    public int CurrentCoins
    {
        get { return currentCoins; }
        set
        {
            //Check if the value has changed
            //if (currentCoins != value)
            //{
            Debug.Log(currentCoins);
            currentCoins = value;
            if (GameManager.instance != null)
            {
                GameManager.instance.currentCoinsDisplay.text = "" + currentCoins;
            }
            stars.playerCoins.text = "" + currentCoins;
            //Add any additional logic here that needs to be executed when the value changes
            //}
        }
    }
    #endregion

    //Experience and level of the player
    [Header("Experience/Level")]
    public int experience = 0;
    public int level = 1;
    public int experienceCap;

    //Class for defining a level range and the corresponding experience cap increase for that range
    [System.Serializable]
    public class LevelRange
    {
        public int startLevel;
        public int endLevel;
        public int experienceCapIncrease;
    }

    //I-Frames
    [Header("I-Frames")]
    public float invincibilityDuration;
    float invincibilityTimer;
    bool isInvincible;

    public List<LevelRange> levelRanges;

    InventoryManager inventory;
    public int weaponIndex;
    public int passiveItemIndex;
    public int defensivePowerUpIndex;

    [Header("UI")]
    public Image healthBar;
    public Image expBar;
    public TextMeshProUGUI levelText;

    public GameObject secondWeaponTest;
    public GameObject firstPassiveItemTest, secondPassiveItemTest;
    public UICoins stars;

    public string playerName;
    public Sprite playerImage;
    void Awake()
    {
        characterData = CharacterSelectorController.GetData();
        CharacterSelectorController.instance.DestroySingleton();
        PlayerSpawner.instance.DestroySingleton();

        inventory = GetComponent<InventoryManager>();

        PersistenceManager.PersistenceInstance.readFile();
        
        //Assign the variables
        CurrentHealth = characterData.MaxHealth;
        CurrentRecovery = characterData.Recovery;
        CurrentMoveSpeed = characterData.MoveSpeed;
        CurrentMight = characterData.Might;
        CurrentProjectileSpeed = characterData.ProjectileSpeed;
        CurrentMagnet = characterData.Magnet;
        
        //Spawn the starting weapon
       
        //SpawnWeapon(secondWeaponTest);
        //SpawnPassiveItem(firstPassiveItemTest);
        //SpawnPassiveItem(secondPassiveItemTest);
    }

    void Start()
    {
        SpawnWeapon(characterData.StartingWeapon);

        stars = GameObject.FindGameObjectWithTag("PlayerStars").GetComponent<UICoins>();
        CurrentCoins = 0;
        //Initialize the experience cap as the first experience cap increase
        experienceCap = levelRanges[0].experienceCapIncrease;

        //Set the current stats display
        GameManager.instance.currentHealthDisplay.text = "" + currentHealth;
        GameManager.instance.currentRecoveryDisplay.text = "" + currentRecovery;
        GameManager.instance.currentMoveSpeedDisplay.text = "" + currentMoveSpeed;
        GameManager.instance.currentMightDisplay.text = "" + currentMight;
        GameManager.instance.currentProjectileSpeedDisplay.text = "" + currentProjectileSpeed;
        GameManager.instance.currentMagnetDisplay.text = "" + currentMagnet;
        GameManager.instance.currentCoinsDisplay.text = "" + currentCoins;

        GameManager.instance.AssignChosenCharacterUI(this.playerName,this.playerImage);
        
        UpdateHealthBar();
        UpdateExpBar();
        UpdateLevelText();
    }

    void Update()
    {
        if (invincibilityTimer >= 0)
        {
            invincibilityTimer -= Time.deltaTime;
        }
        //If the invincibility timer has reached 0, set the invincibility flag to false
        else if (isInvincible)
        {
            isInvincible = false;
        }

        Recover();
    }

    public void IncreaseExperience(int amount)
    {
        experience += amount;

        LevelUpChecker();
        UpdateExpBar();
    }

    void LevelUpChecker()
    {
        if (experience >= experienceCap)
        {
            //Level up the player and reduce their experience by the experience cap
            level++;
            experience -= experienceCap;

            //Find the experience cap increase for the current level range
            int experienceCapIncrease = 0;
            foreach (LevelRange range in levelRanges)
            {
                if (level >= range.startLevel && level <= range.endLevel)
                {
                    experienceCapIncrease = range.experienceCapIncrease;
                    break;
                }
            }
            experienceCap += experienceCapIncrease;
            UpdateLevelText();
            GameManager.instance.StartLevelUp();
        }
    }

    void UpdateExpBar()
    {
        // Update exp bar fill amount
        expBar.fillAmount = (float)experience / experienceCap;
        
    }

    void UpdateLevelText()
    {
        // Update level text
        levelText.text = "LV " + level.ToString();
    }

    public void TakeDamage(float dmg)
    {
        //If the player is not currently invincible, reduce health and start invincibility
        if (!isInvincible)
        {
            CurrentHealth -= dmg;

            invincibilityTimer = invincibilityDuration;
            isInvincible = true;

            Debug.LogWarning("Damage Player: " + dmg);

            if (CurrentHealth <= 0)
            {
                Debug.LogWarning("Death Player");
                Kill();
            }

            UpdateHealthBar();
        }
    }

    void UpdateHealthBar()
    {
        //Update the health bar
        healthBar.fillAmount = CurrentHealth / characterData.MaxHealth;
    }

    public void Kill()
    {

        Debug.Log("Game over:" + GameManager.instance.isGameOver);
        if (!GameManager.instance.isGameOver)
        {
            GameManager.instance.AssignLevelReachedUI(level);
            GameManager.instance.AssignChosenWeaponsAndPassiveItemsUI(inventory.weaponUISlots, inventory.passiveItemUISlots, inventory.defensivePowerUpUISlots);
            GameManager.instance.GameOver();
        }
    }
    public void IncrementCoins()
    {
        CurrentCoins += 1;
    }
    public void RestoreHealth(float amount)
    {
        // Only heal the player if their current health is less than their maximum health
        if (CurrentHealth < characterData.MaxHealth)
        {
            CurrentHealth += amount;

            // Make sure the player's health doesn't exceed their maximum health
            if (CurrentHealth > characterData.MaxHealth)
            {
                CurrentHealth = characterData.MaxHealth;
            }
        }
    }

    void Recover()
    {
        if (CurrentHealth < characterData.MaxHealth)
        {
            CurrentHealth += CurrentRecovery * Time.deltaTime;

            // Make sure the player's health doesn't exceed their maximum health
            if (CurrentHealth > characterData.MaxHealth)
            {
                CurrentHealth = characterData.MaxHealth;
            }
        }
    }

    public void SpawnWeapon(GameObject weapon)
    {
        inventory.DisableAllUIElements();
        //Checking if the slots are full, and returning if it is
        if (weaponIndex >= inventory.weaponSlots.Count - 1) //Must be -1 because a list starts from 0
        {
            Debug.LogError("Inventory slots already full");
            return;
        }

        //Spawn the starting weapon
        GameObject spawnedWeapon = Instantiate(weapon, transform.position, Quaternion.identity);
        spawnedWeapon.transform.SetParent(transform);    //Set the weapon to be a child of the player
        inventory.AddWeapon(weaponIndex, spawnedWeapon.GetComponent<WeaponController>());   //Add the weapon to it's slot // instance error
        weaponIndex++;  //Need to increase so slots don't overlap [INCREMENT ONLY AFTER ADDING THE WEAPON TO THE SLOT]
    }

    public void SpawnPassiveItem(GameObject passiveItem)
    {
        inventory.DisableAllUIElements();
        //Checking if the slots are full, and returning if it is
        if (passiveItemIndex >= inventory.passiveItemSlots.Count - 1) //Must be -1 because a list starts from 0
        {
            Debug.LogError("Inventory slots already full");
            return;
        }

        //Spawn the passive item
        GameObject spawnedPassiveItem = Instantiate(passiveItem, transform.position, Quaternion.identity);
        spawnedPassiveItem.transform.SetParent(transform);    //Set the passive item to be a child of the player
        inventory.AddPassiveItem(passiveItemIndex, spawnedPassiveItem.GetComponent<PassiveItem>());   //Add the passive item to it's slot // instance error

        passiveItemIndex++;  //Need to increase so slots don't overlap [INCREMENT ONLY AFTER ADDING THE PASSIVE ITEM TO THE SLOT]
    }

    public void SpawnBoostItem(GameObject boostItem)
    {
        inventory.DisableAllUIElements();
        //Spawn the boost item
        GameObject spawnedBoostItem = Instantiate(boostItem, transform.position, Quaternion.identity);
        spawnedBoostItem.transform.SetParent(transform);    //Set the boost item to be a child of the player
        // Destroy(spawnedBoostItem);

        if(GameManager.instance != null && GameManager.instance.choosingUpgrade)
        {
            GameManager.instance.EndLevelUp();
        }

    }

    public void SpawnDefensivePowerUp(GameObject defensivePowerUp)
    {
        inventory.DisableAllUIElements();
        //Checking if the slots are full, and returning if it is
        if (defensivePowerUpIndex >= inventory.defensivePowerUpSlots.Count - 1) //Must be -1 because a list starts from 0
        {
            Debug.LogError("Inventory slots already full");
            return;
        }

        //Spawn the passive item
        GameObject spawnedDefensivePowerUp = Instantiate(defensivePowerUp, transform.position, Quaternion.identity);
        spawnedDefensivePowerUp.transform.SetParent(transform);    //Set the defensive power up to be a child of the player
        inventory.AddDefensivePowerUp(defensivePowerUpIndex, spawnedDefensivePowerUp.GetComponent<DefensivePowerUpController>());   //Add the defensive power up to it's slot
        defensivePowerUpIndex++;  //Need to increase so slots don't overlap [INCREMENT ONLY AFTER ADDING THE DEFENSIVE POWER UP TO THE SLOT]
    }
}