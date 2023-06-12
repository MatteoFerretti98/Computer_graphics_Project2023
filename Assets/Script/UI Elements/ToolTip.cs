using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ToolTip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string text = "Hey there";
    public string currentToolTipText = "";
    public TextMeshProUGUI toolTipText;

    void Start()
    {
        toolTipText = GetComponentInChildren<TextMeshProUGUI>();
        toolTipText.enabled = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        currentToolTipText = text;
        toolTipText.text = currentToolTipText;
        toolTipText.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        currentToolTipText = "";
        toolTipText.enabled = false;
    }
}
