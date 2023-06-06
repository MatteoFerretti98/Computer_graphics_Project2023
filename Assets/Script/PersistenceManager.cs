using System.IO;
using UnityEngine;

public class PersistenceManager : MonoBehaviour
{
    private class GameData
    {
        public int Coins;
    }

    private GameData gameData;

    public int Coins
    {
        get => gameData.Coins;
        set => gameData.Coins = value;
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
            GameData firstGameData = new GameData();
            firstGameData.Coins = 0;
            // Serialize the object into JSON and save string.
            string jsonString = JsonUtility.ToJson(firstGameData);

            PersistenceInstance.Coins = 0;
            // Write JSON to file.
            File.WriteAllText(PersistenceInstance.saveFile, jsonString);
        }
        else
        {
            // Read the entire file and save its contents.
            string fileContents = File.ReadAllText(PersistenceInstance.saveFile);

            // Deserialize the JSON data 
            //  into a pattern matching the GameData class.
            gameData = JsonUtility.FromJson<GameData>(fileContents);
            
            PersistenceInstance.gameData.Coins = gameData.Coins;
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
        string jsonString = JsonUtility.ToJson(PersistenceInstance.gameData);

        // Write JSON to file.
        File.WriteAllText(PersistenceInstance.saveFile, jsonString);
    }
}