using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectorController : MonoBehaviour
{
    public static CharacterSelectorController instance;
    public CharacterScriptableObject characterData;
    public string characterSelected = "";
    public string weaponSelected = "";
    public string nameCharacter = "";
    public Sprite imageCharacter;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogWarning("EXTRA " + this + " DELETED");
            Destroy(gameObject);
        }
    }

    public static CharacterScriptableObject GetData()
    {
        return instance.characterData;
    }

    public void SelectCharacter(CharacterScriptableObject character)
    {
        characterData = character;
    }

    public void DestroySingleton()
    {
        instance = null;
        Destroy(gameObject);
    }

}
