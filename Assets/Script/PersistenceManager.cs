using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PersistenceManager : MonoBehaviour
{
    private static object lockObject = new object();
    private class GameData
    {
        public int Coins;
        public List<string> Weapons;
    }

    private GameData gameData;

    public int Coins
    {
        get => gameData.Coins;
        set => gameData.Coins = value;
    }

    public List<string> Weapons
    {
        get => gameData.Weapons;
        set => gameData.Weapons = value;
    }

    // Create a field for the save file.
    string saveFile;

    public static PersistenceManager PersistenceInstance { get; private set; }

    void Awake()
    {
        if (PersistenceInstance == null)
        {
            PersistenceInstance = this;
            PersistenceInstance.gameData = new GameData();
            DontDestroyOnLoad(gameObject);
        } 
        else
            Destroy(gameObject);

        PersistenceInstance.saveFile = Application.persistentDataPath + "/gamedata.json";
        Debug.Log(PersistenceInstance.saveFile);
        if (!File.Exists(PersistenceInstance.saveFile))
        {
            PersistenceInstance.Coins = 0;
            List<string> weapons = new List<string>();
            weapons.Add("Knife");
            PersistenceInstance.Weapons = weapons;
            // Serialize the object into JSON and save string.
            string jsonString = JsonUtility.ToJson(gameData);

            // Write JSON to file.
            File.WriteAllText(PersistenceInstance.saveFile, jsonString);
        }
        else
        {
            // Read the entire file and save its contents.
            string fileContents = File.ReadAllText(PersistenceInstance.saveFile);

            // Deserialize the JSON data 
            //  into a pattern matching the GameData class.
            PersistenceInstance.gameData = gameData = JsonUtility.FromJson<GameData>(fileContents);
        }
        
    }

    public void readFile()
    {
        // Does the file exist?
        if (File.Exists(PersistenceInstance.saveFile))
        {
            // Read the entire file and save its contents.
            string fileContents = File.ReadAllText(PersistenceInstance.saveFile);

            // Deserialize the JSON data 
            //  into a pattern matching the GameData class.
            gameData = JsonUtility.FromJson<GameData>(fileContents);
        }
    }

    public void writeFile()
    {
        // Serialize the object into JSON and save string.
        lock (lockObject)
        {
            string jsonString = JsonUtility.ToJson(PersistenceInstance.gameData);

            // Write JSON to file.
            File.WriteAllText(PersistenceInstance.saveFile, jsonString);
        }
    }

    public void DecrementBalance(int value)
    {
        lock (lockObject)
        {
            PersistenceInstance.Coins -= value;
        }
    }

    public void AddWeapon(string weapon)
    {
        lock (lockObject)
        {
            List<string> weapons = new List<string>(PersistenceInstance.Weapons);
            weapons.Add(weapon);
            PersistenceInstance.Weapons = weapons;
        }
    }
}