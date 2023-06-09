using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UICharacterSelector : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public float scaleFactor = 1.2f; // Fattore di scala per l'ingrandimento
    private Vector3 originalScale; // La scala originale dell'elemento UI
    public Button targetButton;
    private RectTransform rectTransform;
    public List<GameObject> otherCharacters;
    public bool isClicked = false;

    private void Awake()
    {
        originalScale = transform.localScale;
        targetButton.gameObject.SetActive(false);
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        rectTransform.localScale = originalScale * scaleFactor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {   
        if(!isClicked)
            rectTransform.localScale = originalScale; // Ripristina la scala originale
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isClicked)
        {
            isClicked = true;
            RectTransform rectTransformOther;
            UICharacterSelector uiCharacterSelector;
            foreach (GameObject el in otherCharacters)
            {
                rectTransformOther = el.GetComponent<RectTransform>();
                rectTransformOther.localScale = originalScale;
                uiCharacterSelector = el.GetComponent<UICharacterSelector>();
                uiCharacterSelector.isClicked = false;
            }
            
            CharacterSelectorController.instance.characterSelected = this.tag;

            targetButton.gameObject.SetActive(true); // Attiva il pulsante
            rectTransform.localScale = originalScale * scaleFactor; // Mantieni l'ingrandimento
        }
        else
        {
            isClicked = false;
            targetButton.gameObject.SetActive(false);
            CharacterSelectorController.instance.characterSelected = "";
        }
        
    }
}