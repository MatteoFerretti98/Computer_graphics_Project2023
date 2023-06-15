using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIWeaponSelector : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public float scaleFactor = 1.2f; // Fattore di scala per l'ingrandimento
    private Vector3 originalScale; // La scala originale dell'elemento UI
    public Button targetButton;
    private RectTransform rectTransform;
    public List<GameObject> otherWeapons;
    public bool isClicked = false;

    private Dictionary<string, int> prices = new Dictionary<string, int>();

    GameObject messageSuccess;
    GameObject messageError;

    private void Awake()
    {
        originalScale = transform.localScale;
        targetButton.gameObject.SetActive(false);
        rectTransform = GetComponent<RectTransform>();
        prices.Add("Knife", 0);
        prices.Add("Garlic", 20);
        prices.Add("FlameStream", 50);
        prices.Add("IceShard", 100);
    }

    private void Start()
    {

        messageError = GameObject.Find("/Canvas/Panels/Panel_WeaponSelect/Popup_Message/Popup_MessageError");
        messageSuccess = GameObject.Find("/Canvas/Panels/Panel_WeaponSelect/Popup_Message/Popup_MessageSuccess");
        messageError.SetActive(false);
        messageSuccess.SetActive(false);
        if (PersistenceManager.PersistenceInstance.Weapons.Contains(tag))
        {
            if (GameObject.Find("WeaponBgLock " + tag)) GameObject.Find("WeaponBgLock " + tag).SetActive(false);
        }
        if (GameObject.Find("price " + tag)) GameObject.Find("price " + tag).GetComponent<TextMeshProUGUI>().text = prices[tag].ToString();

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (PersistenceManager.PersistenceInstance.Weapons.Contains(tag))
        {
            rectTransform.localScale = originalScale * scaleFactor;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isClicked)
            rectTransform.localScale = originalScale; // Ripristina la scala originale
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (PersistenceManager.PersistenceInstance.Weapons.Contains(tag))
        {
            if (!isClicked)
            {

                isClicked = true;
                RectTransform rectTransformOther;
                UIWeaponSelector uiWeaponSelector;
                foreach (GameObject el in otherWeapons)
                {
                    rectTransformOther = el.GetComponent<RectTransform>();
                    rectTransformOther.localScale = originalScale;
                    uiWeaponSelector = el.GetComponent<UIWeaponSelector>();
                    uiWeaponSelector.isClicked = false;
                }


                CharacterSelectorController.instance.weaponSelected = this.tag;

                targetButton.gameObject.SetActive(true); // Attiva il pulsante
                rectTransform.localScale = originalScale * scaleFactor; // Mantieni l'ingrandimento

            }
            else
            {
                isClicked = false;
                targetButton.gameObject.SetActive(false);

                CharacterSelectorController.instance.weaponSelected = "";
            }
        }

    }

    public void Buy()
    {

        Debug.Log(tag);
        int price = prices[tag];
        if (PersistenceManager.PersistenceInstance.Coins < price)
        {
            Debug.Log("Non Puoi comprarlo");
            MessageManager.ShowMessage(messageError, 2f);
        }
        else
        {
            // Aggiorno Saldo
            PersistenceManager.PersistenceInstance.DecrementBalance(price);
            PersistenceManager.PersistenceInstance.AddWeapon(tag);
            PersistenceManager.PersistenceInstance.writeFile();

            Debug.Log("Puoi comprarlo");
            MessageManager.ShowMessage(messageSuccess, 2f);
            GameObject.Find("WeaponBgLock " + tag).SetActive(false);

        }
    }
}