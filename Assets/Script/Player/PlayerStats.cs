using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                currentHealth = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentHealthDisplay.text = "Health: " + currentHealth;
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
                    GameManager.instance.currentRecoveryDisplay.text = "Recovery: " + currentRecovery;
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
                    GameManager.instance.currentMoveSpeedDisplay.text = "Move Speed: " + currentMoveSpeed;
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
                    GameManager.instance.currentMightDisplay.text = "Might: " + currentMight;
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
                    GameManager.instance.currentProjectileSpeedDisplay.text = "Projectile Speed: " + currentProjectileSpeed;
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
                    GameManager.instance.currentMagnetDisplay.text = "Magnet: " + currentMagnet;
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
            currentCoins = value;
            if (GameManager.instance != null)
            {
                GameManager.instance.currentCoinsDisplay.text = "Coins: " + currentCoins;
            }
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

    public GameObject secondWeaponTest;
    public GameObject firstPassiveItemTest, secondPassiveItemTest;


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
        CurrentCoins = PersistenceManager.PersistenceInstance.Coins;

        //Spawn the starting weapon
       
        //SpawnWeapon(secondWeaponTest);
        //SpawnPassiveItem(firstPassiveItemTest);
        //SpawnPassiveItem(secondPassiveItemTest);
    }

    void Start()
    {
        SpawnWeapon(characterData.StartingWeapon);
        //Initialize the experience cap as the first experience cap increase
        experienceCap = levelRanges[0].experienceCapIncrease;

        //Set the current stats display
        GameManager.instance.currentHealthDisplay.text = "Health: " + currentHealth;
        GameManager.instance.currentRecoveryDisplay.text = "Recovery: " + currentRecovery;
        GameManager.instance.currentMoveSpeedDisplay.text = "Move Speed: " + currentMoveSpeed;
        GameManager.instance.currentMightDisplay.text = "Might: " + currentMight;
        GameManager.instance.currentProjectileSpeedDisplay.text = "Projectile Speed: " + currentProjectileSpeed;
        GameManager.instance.currentMagnetDisplay.text = "Magnet: " + currentMagnet;
        GameManager.instance.currentCoinsDisplay.text = "Coins: " + currentCoins;

        GameManager.instance.AssignChosenCharacterUI(characterData);
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
            GameManager.instance.StartLevelUp();
        }
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
        }
    }

    public void Kill()
    {

        Debug.Log("Game over:" + GameManager.instance.isGameOver);
        if (!GameManager.instance.isGameOver)
        {
            GameManager.instance.AssignLevelReachedUI(level);
            GameManager.instance.AssignChosenWeaponsAndPassiveItemsUI(inventory.weaponUISlots, inventory.passiveItemUISlots);
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
}