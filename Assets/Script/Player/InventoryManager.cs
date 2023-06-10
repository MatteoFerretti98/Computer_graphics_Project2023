using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class InventoryManager : MonoBehaviour
{
    public List<WeaponController> weaponSlots = new List<WeaponController>(6);
    public int[] weaponLevels = new int[6];
    public List<Image> weaponUISlots = new List<Image>(6);
    public List<PassiveItem> passiveItemSlots = new List<PassiveItem>(6);
    public int[] passiveItemLevels = new int[6];
    public List<Image> passiveItemUISlots = new List<Image>(6);
    public List<DefensivePowerUpController> defensivePowerUpSlots = new List<DefensivePowerUpController>(6);
    public int[] defensivePowerUpLevels = new int[6];
    public List<Image> defensivePowerUpUISlots = new List<Image>(6);


    [System.Serializable]
    public class WeaponUpgrade
    {
        public int weaponUpgradeIndex;
        public GameObject initialWeapon;
        public WeaponScriptableObject weaponData;
    }

    [System.Serializable]
    public class PassiveItemUpgrade
    {
        public int passiveItemUpgradeIndex;
        public GameObject initialPassiveItem;
        public PassiveItemScriptableObject passiveItemData;
    }

    [System.Serializable]
    public class DefensivePowerUpUpgrade
    {
        public int defensivePowerUpIndex;
        public GameObject initialDefensivePowerUp;
        public DefensivePowerUpScriptableObject defensivePowerUpData;
    }

    [System.Serializable]
    public class UpgradeUI
    {
        public TextMeshProUGUI upgradeNameDisplay;
        public TextMeshProUGUI upgradeDescriptionDisplay;
        public Image upgradeIcon;
        public Button upgradeButton;
    }

    public List<WeaponUpgrade> weaponUpgradeOptions = new List<WeaponUpgrade>();    //List of upgrade options for weapons
    public List<PassiveItemUpgrade> passiveItemUpgradeOptions = new List<PassiveItemUpgrade>(); //List of upgrade options for passive items
    public List<DefensivePowerUpUpgrade> defensivePowerUpUpgradeOptions = new List<DefensivePowerUpUpgrade>(); //List of upgrade options for defensive power up
    public List<UpgradeUI> upgradeUIOptions = new List<UpgradeUI>();    //List of ui for upgrade options present in the scene


    PlayerStats player;


    void Start()
    {
        player = GetComponent<PlayerStats>();
    }

    public void AddWeapon(int slotIndex, WeaponController weapon)   //Add a weapon to a specific slot
    {
        weaponSlots[slotIndex] = weapon;
        weaponLevels[slotIndex] = weapon.weaponData.Level;
        weaponUISlots[slotIndex].enabled = true;   //Enable the image component
        weaponUISlots[slotIndex].sprite = weapon.weaponData.Icon;

        if (GameManager.instance != null && GameManager.instance.choosingUpgrade)
        {
            GameManager.instance.EndLevelUp();
        }
    }

    public void AddPassiveItem(int slotIndex, PassiveItem passiveItem)  //Add a passive item to a specific slot
    {
        passiveItemSlots[slotIndex] = passiveItem;
        passiveItemLevels[slotIndex] = passiveItem.passiveItemData.Level;
        passiveItemUISlots[slotIndex].enabled = true; //Enable the image component
        passiveItemUISlots[slotIndex].sprite = passiveItem.passiveItemData.Icon;

        if (GameManager.instance != null && GameManager.instance.choosingUpgrade)
        {
            GameManager.instance.EndLevelUp();
        }
    }

    public void AddDefensivePowerUp(int slotIndex, DefensivePowerUpController defensivePowerUp)   //Add a defensive power up to a specific slot
    {
        defensivePowerUpSlots[slotIndex] = defensivePowerUp;
        defensivePowerUpLevels[slotIndex] = defensivePowerUp.defensivePowerUpData.Level;
        defensivePowerUpUISlots[slotIndex].enabled = true;   //Enable the image component
        defensivePowerUpUISlots[slotIndex].sprite = defensivePowerUp.defensivePowerUpData.Icon;

        if (GameManager.instance != null && GameManager.instance.choosingUpgrade)
        {
            GameManager.instance.EndLevelUp();
        }
    }

    public void LevelUpWeapon(int slotIndex, int upgradeIndex)
    {
        if (weaponSlots.Count > slotIndex)
        {
            WeaponController weapon = weaponSlots[slotIndex];
            if (!weapon.weaponData.NextLevelPrefab)  //Checks if there is a next level
            {
                Debug.LogError("NO NEXT LEVEL FOR " + weapon.name);
                return;
            }
            GameObject upgradedWeapon = Instantiate(weapon.weaponData.NextLevelPrefab, transform.position, Quaternion.identity);
            upgradedWeapon.transform.SetParent(transform);    //Set the weapon to be a child of the player
            AddWeapon(slotIndex, upgradedWeapon.GetComponent<WeaponController>());
            Destroy(weapon.gameObject);
            weaponLevels[slotIndex] = upgradedWeapon.GetComponent<WeaponController>().weaponData.Level;  //To make sure we have the correct weapon level
            weaponUpgradeOptions[upgradeIndex].weaponData = upgradedWeapon.GetComponent<WeaponController>().weaponData;
        }

        if (GameManager.instance != null && GameManager.instance.choosingUpgrade)
        {
            GameManager.instance.EndLevelUp();
        }
    }

    public void LevelUpPassiveItem(int slotIndex, int upgradeIndex)
    {
        if (passiveItemSlots.Count > slotIndex)
        {
            PassiveItem passiveItem = passiveItemSlots[slotIndex];
            if (!passiveItem.passiveItemData.NextLevelPrefab)  //Checks if there is a next level
            {
                Debug.LogError("NO NEXT LEVEL FOR " + passiveItem.name);
                return;
            }
            GameObject upgradedPassiveItem = Instantiate(passiveItem.passiveItemData.NextLevelPrefab, transform.position, Quaternion.identity);
            upgradedPassiveItem.transform.SetParent(transform);    //Set the passive item to be a child of the player
            AddPassiveItem(slotIndex, upgradedPassiveItem.GetComponent<PassiveItem>());
            Destroy(passiveItem.gameObject);
            passiveItemLevels[slotIndex] = upgradedPassiveItem.GetComponent<PassiveItem>().passiveItemData.Level;  //To make sure we have the correct passive item level
            passiveItemUpgradeOptions[upgradeIndex].passiveItemData = upgradedPassiveItem.GetComponent<PassiveItem>().passiveItemData;
        }
        if (GameManager.instance != null && GameManager.instance.choosingUpgrade)
        {
            GameManager.instance.EndLevelUp();
        }
    }

    public void LevelUpDefensivePowerUp(int slotIndex, int upgradeIndex)
    {
        if (defensivePowerUpSlots.Count > slotIndex)
        {
            DefensivePowerUpController defensivePowerUp = defensivePowerUpSlots[slotIndex];
            if (!defensivePowerUp.defensivePowerUpData.NextLevelPrefab)  //Checks if there is a next level
            {
                Debug.LogError("NO NEXT LEVEL FOR " + defensivePowerUp.name);
                return;
            }
            GameObject upgradedDefensivePowerUp = Instantiate(defensivePowerUp.defensivePowerUpData.NextLevelPrefab, transform.position, Quaternion.identity);
            upgradedDefensivePowerUp.transform.SetParent(transform);    //Set the defensive power up to be a child of the player
            AddDefensivePowerUp(slotIndex, upgradedDefensivePowerUp.GetComponent<DefensivePowerUpController>());
            Destroy(defensivePowerUp.gameObject);
            defensivePowerUpLevels[slotIndex] = upgradedDefensivePowerUp.GetComponent<DefensivePowerUpController>().defensivePowerUpData.Level;  //To make sure we have the correct defensive level up level
            defensivePowerUpUpgradeOptions[upgradeIndex].defensivePowerUpData = upgradedDefensivePowerUp.GetComponent<DefensivePowerUpController>().defensivePowerUpData;
        }

        if (GameManager.instance != null && GameManager.instance.choosingUpgrade)
        {
            GameManager.instance.EndLevelUp();
        }
    }

    void ApplyUpgradeOptions()
    {
   
        List<WeaponUpgrade> availableWeaponUpgrades = new List<WeaponUpgrade>(weaponUpgradeOptions);
        List<PassiveItemUpgrade> availablePassiveItemUpgrades = new List<PassiveItemUpgrade>(passiveItemUpgradeOptions);
        List<DefensivePowerUpUpgrade> availableDefensivePowerUpUpgrades = new List<DefensivePowerUpUpgrade>(defensivePowerUpUpgradeOptions);

        foreach (var upgradeOption in upgradeUIOptions)
        {
           

            if (availableWeaponUpgrades.Count == 0 && availablePassiveItemUpgrades.Count == 0 && availableDefensivePowerUpUpgrades.Count == 0)
            {
                return;
            }

            int upgradeType;

            if (availableWeaponUpgrades.Count == 0)
            {
                // upgradeType = 2;
                upgradeType = Random.Range(2, 4); // chose 2 or 3
            }
            else if (availablePassiveItemUpgrades.Count == 0)
            {
                // upgradeType = 1;
                 
                upgradeType = Random.Range(1, 3); // chose 1 or 3
                if (upgradeType >= 2)
                {
                    upgradeType += 1;
                }
            }
            else if (availableDefensivePowerUpUpgrades.Count == 0)
            {
                upgradeType = 3;
                upgradeType = Random.Range(1, 2); // chose 1 or 2
            }
            else
            {
                
                upgradeType = Random.Range(1, 4);    //Choose between weapon and passive items

            }

            Debug.Log("upgradeType: " + upgradeType);

            if (upgradeType == 1)
            {
                WeaponUpgrade chosenWeaponUpgrade = availableWeaponUpgrades[Random.Range(0, availableWeaponUpgrades.Count)];
                availableWeaponUpgrades.Remove(chosenWeaponUpgrade);

                if (chosenWeaponUpgrade != null)
                {
                    EnableUpgradeUI(upgradeOption);
                    bool newWeapon = false;
                    for (int i = 0; i < weaponSlots.Count; i++)
                    {
                        if (weaponSlots[i] != null && weaponSlots[i].weaponData == chosenWeaponUpgrade.weaponData)
                        {
                            newWeapon = false;
                            if (!newWeapon)
                            {
                                if (!chosenWeaponUpgrade.weaponData.NextLevelPrefab)
                                {
                                    DisableUpgradeUI(upgradeOption);
                                    break;
                                }
                                upgradeOption.upgradeButton.onClick.AddListener(() => LevelUpWeapon(i, chosenWeaponUpgrade.weaponUpgradeIndex)); //Apply button functionality;

                                //Set the description and description to be that of the next level
                                upgradeOption.upgradeDescriptionDisplay.text = chosenWeaponUpgrade.weaponData.NextLevelPrefab.GetComponent<WeaponController>().weaponData.Description;
                                upgradeOption.upgradeNameDisplay.text = chosenWeaponUpgrade.weaponData.NextLevelPrefab.GetComponent<WeaponController>().weaponData.Name;
                            }
                            break;
                        }
                        else
                        {
                            newWeapon = true;
                        }
                    }
                    if (newWeapon)  //Spawn a new weapon
                    {
                        upgradeOption.upgradeButton.onClick.AddListener(() => player.SpawnWeapon(chosenWeaponUpgrade.initialWeapon)); //Apply button functionality
                        upgradeOption.upgradeDescriptionDisplay.text = chosenWeaponUpgrade.weaponData.Description;  //Apply initial description
                        upgradeOption.upgradeNameDisplay.text = chosenWeaponUpgrade.weaponData.Name; // Apply initial name
                    }
                    upgradeOption.upgradeIcon.sprite = chosenWeaponUpgrade.weaponData.Icon;
                }
            }
            else if (upgradeType == 2)
            {
                PassiveItemUpgrade chosenPassiveItemUpgrade = availablePassiveItemUpgrades[Random.Range(0, availablePassiveItemUpgrades.Count)];
                availablePassiveItemUpgrades.Remove(chosenPassiveItemUpgrade);

                if (chosenPassiveItemUpgrade != null)
                {
                    EnableUpgradeUI(upgradeOption);
                    bool newPassiveItem = false;
                    for (int i = 0; i < passiveItemSlots.Count; i++)
                    {
                        if (passiveItemSlots[i] != null && passiveItemSlots[i].passiveItemData == chosenPassiveItemUpgrade.passiveItemData)
                        {
                            newPassiveItem = false;

                            if (!newPassiveItem)
                            {
                                if (!chosenPassiveItemUpgrade.passiveItemData.NextLevelPrefab)
                                {
                                    DisableUpgradeUI(upgradeOption);
                                    break;
                                }
                                upgradeOption.upgradeButton.onClick.AddListener(() => LevelUpPassiveItem(i, chosenPassiveItemUpgrade.passiveItemUpgradeIndex)); //Apply button functionality
                                //Set the description and description to be that of the next level
                                upgradeOption.upgradeDescriptionDisplay.text = chosenPassiveItemUpgrade.passiveItemData.NextLevelPrefab.GetComponent<PassiveItem>().passiveItemData.Description;
                                upgradeOption.upgradeNameDisplay.text = chosenPassiveItemUpgrade.passiveItemData.NextLevelPrefab.GetComponent<PassiveItem>().passiveItemData.Name;
                            }
                            break;
                        }
                        else
                        {
                            newPassiveItem = true;
                        }
                    }
                    if (newPassiveItem) //Spawn a new passive item
                    {
                        upgradeOption.upgradeButton.onClick.AddListener(() => player.SpawnPassiveItem(chosenPassiveItemUpgrade.initialPassiveItem)); //Apply button functionality
                        upgradeOption.upgradeDescriptionDisplay.text = chosenPassiveItemUpgrade.passiveItemData.Description;  //Apply initial description
                        upgradeOption.upgradeNameDisplay.text = chosenPassiveItemUpgrade.passiveItemData.Name;  //Apply initial name
                    }
                    upgradeOption.upgradeIcon.sprite = chosenPassiveItemUpgrade.passiveItemData.Icon;
                }
            }
            else if (upgradeType == 3)
            {
                Debug.Log("upgradeType == 3");

                
                DefensivePowerUpUpgrade chosenDefensivePowerUpUpgrade = availableDefensivePowerUpUpgrades[Random.Range(0, availableDefensivePowerUpUpgrades.Count)];
                availableDefensivePowerUpUpgrades.Remove(chosenDefensivePowerUpUpgrade);

                if (chosenDefensivePowerUpUpgrade != null)
                {
                    EnableUpgradeUI(upgradeOption);
                    bool newDefensivePowerUp = false;
                    for (int i = 0; i < defensivePowerUpSlots.Count; i++)
                    {
                        if (defensivePowerUpSlots[i] != null && defensivePowerUpSlots[i].defensivePowerUpData == chosenDefensivePowerUpUpgrade.defensivePowerUpData)
                        {
                            newDefensivePowerUp = false;

                            if (!newDefensivePowerUp)
                            {
                                if (!chosenDefensivePowerUpUpgrade.defensivePowerUpData.NextLevelPrefab)
                                {
                                    DisableUpgradeUI(upgradeOption);
                                    break;
                                }
                                upgradeOption.upgradeButton.onClick.AddListener(() => LevelUpDefensivePowerUp(i, chosenDefensivePowerUpUpgrade.defensivePowerUpIndex)); //Apply button functionality
                                //Set the description and description to be that of the next level
                                upgradeOption.upgradeDescriptionDisplay.text = chosenDefensivePowerUpUpgrade.defensivePowerUpData.NextLevelPrefab.GetComponent<DefensivePowerUpController>().defensivePowerUpData.Description;
                                upgradeOption.upgradeNameDisplay.text = chosenDefensivePowerUpUpgrade.defensivePowerUpData.NextLevelPrefab.GetComponent<DefensivePowerUpController>().defensivePowerUpData.Name;
                                
                            }
                            break;
                        }
                        else
                        {
                            newDefensivePowerUp = true;
                        }
                    }
                    if (newDefensivePowerUp) //Spawn a new defensive power up
                    {
                        upgradeOption.upgradeButton.onClick.AddListener(() => player.SpawnDefensivePowerUp(chosenDefensivePowerUpUpgrade.initialDefensivePowerUp)); //Apply button functionality
                        upgradeOption.upgradeDescriptionDisplay.text = chosenDefensivePowerUpUpgrade.defensivePowerUpData.Description;  //Apply initial description
                        upgradeOption.upgradeNameDisplay.text = chosenDefensivePowerUpUpgrade.defensivePowerUpData.Name;  //Apply initial name
                    }
                    upgradeOption.upgradeIcon.sprite = chosenDefensivePowerUpUpgrade.defensivePowerUpData.Icon;
                }
                
            }
        }
    }

    void RemoveUpgradeOptions()
    {
        foreach (var upgradeOption in upgradeUIOptions)
        {
            upgradeOption.upgradeButton.onClick.RemoveAllListeners();
        }
    }

    public void RemoveAndApplyUpgrades()
    {
        RemoveUpgradeOptions();
        ApplyUpgradeOptions();
    }

    void DisableUpgradeUI(UpgradeUI ui)
    {
        ui.upgradeNameDisplay.transform.parent.gameObject.SetActive(false);
    }

    void EnableUpgradeUI(UpgradeUI ui)
    {
        ui.upgradeNameDisplay.transform.parent.gameObject.SetActive(true);
    }
}
