using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Debug = UnityEngine.Debug;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour

{
    public static GameManager instance;
    public GameObject canvas;
    public GameObject BossHealthBar;
    

    // Define the different states of the game
    public enum GameState
    {
        Gameplay,
        Paused,
        GameOver,
        LevelUp,
        Win
        //Tutorial
    }

    // Store the current state of the game
    public GameState currentState;

    // Store the previous state of the game before it was paused
    public GameState previousState;

    [Header("Screens")]
    public GameObject pauseScreen;
    public GameObject resultsScreen;
    public GameObject confirmScreen;
    public GameObject gameScreen;
    public GameObject levelUpScreen;
    public GameObject winScreen;
    public GameObject bossWarningScreen;
    public GameObject tutorialScreen;

    //Current stat displays
    [Header("Current Stat Displays")]
    public TextMeshProUGUI currentTimeDisplay;
    public TextMeshProUGUI currentHealthDisplay;
    public TextMeshProUGUI currentRecoveryDisplay;
    public TextMeshProUGUI currentMoveSpeedDisplay;
    public TextMeshProUGUI currentMightDisplay;
    public TextMeshProUGUI currentProjectileSpeedDisplay;
    public TextMeshProUGUI currentMagnetDisplay;
    public TextMeshProUGUI currentCoinsDisplay;

    [Header("Results Screen Displays")]
    public Image chosenCharacterImage;
    public TextMeshProUGUI chosenCharacterName;
    public TextMeshProUGUI levelReachedDisplay;
    public TextMeshProUGUI timeSurvivedDisplay;
    public List<Image> chosenWeaponsUI = new List<Image>(6);
    public List<Image> chosenPassiveItemsUI = new List<Image>(6);
    public List<Image> chosenDefensivePowerUpUI = new List<Image>(6);

    [Header("Win Screen Displays")]
    public Image winCharacterImage;

    [Header("Stopwatch")]
    public float timeLimit; // The time limit in seconds
    float stopwatchTime; // The current time elapsed since the stopwatch started
    public TextMeshProUGUI stopwatchDisplay;

    [Header("Tutorial Mode")]
    public bool tutorialMode = true;

    // Flag to check if the game is over
    public bool isGameOver = false;

    // Flag to check if the player is choosing their upgrades
    public bool choosingUpgrade = false;

    // Reference to the player's game object
    public GameObject playerObject;

    public bool BossFightTime = false;

    public bool isWin = false;


    [SerializeField]
    private PlayerStats player;

    void Awake()
    {
        //Warning check to see if there is another singleton of this kind already in the game
        if (instance == null)
        {
            instance = this;
            Debug.Log("Game instance is created");
        }
        else
        {
            Debug.LogWarning("EXTRA " + this + " DELETED");
            Destroy(gameObject);
        }

        DisableScreens();
    }

    private void Start()
    {
        AudioManager.instance.PlayMusicLoop("InGameMusic");
        ChangeState(GameState.Gameplay); // Set the initial state to gameplay
        Time.timeScale = 1f; // Resume the game
        Debug.Log("Game is started");

        // code to initialize status and powers?
    }

    void Update()
    {
        //TestSwitchState(); // test game over pressing key G
        // manage boss bar visualization
        // Check if the current scene is the target scene
        if (SceneManager.GetActiveScene().name == "Scene1")
        {
            HideBossHealthBar();
        }
        else if (SceneManager.GetActiveScene().name == "BossArena")
        {
            ShowBossHealthBar();
        }

        // Define the behavior for each state
        switch (currentState)
        {
            case GameState.Gameplay:
                // Code for the gameplay state
                if (PersistenceManager.PersistenceInstance.FirstTime)
                {
                    Time.timeScale = 0f;
                    tutorialScreen.SetActive(true);
                }
                else 
                {
                    CheckForPauseAndResume();
                    UpdateStopwatch();
                }
                
                break;
            case GameState.Paused:
                // Code for the paused state
                CheckForPauseAndResume();
                break;
            case GameState.GameOver:
                // Code for the game over state
                
                if (!isGameOver)
                {
                    isGameOver = true;
                    AudioManager.instance.StopMusic();
                    AudioManager.instance.PlaySFX("GameOver");
                    Time.timeScale = 0f;

                    if (previousState == GameState.Gameplay)
                    {
                        Debug.Log("prevoius state: Gameplay");
                        //aggiunta monete accomulate durante il gioco
                        player = FindObjectOfType<PlayerStats>();
                        PersistenceManager.PersistenceInstance.Coins += player.CurrentCoins;
                        PersistenceManager.PersistenceInstance.writeFile();

                        Debug.Log("Game is over");
                        gameScreen.SetActive(false); //
                        DisplayResults();
                    }
                    else if (previousState == GameState.Paused)
                    {
                        Debug.Log("prevoius state: Pause");
                        Debug.Log("Game is over");
                        gameScreen.SetActive(false); //
                    }
                }
                break;
            case GameState.LevelUp:
                if (!choosingUpgrade)
                {
                    choosingUpgrade = true;
                    Time.timeScale = 0f; //Pause the game for now
                    Debug.Log("Upgrades shown");
                    levelUpScreen.SetActive(true);
                }
                break;
            /*case GameState.Tutorial:
                if(PersistenceManager.PersistenceInstance.FirstTime)
                ChangeState(GameState.Gameplay);
                break;*/
            case GameState.Win:
                if (!isWin)
                {
                    AudioManager.instance.StopMusic();
                    isWin = true;
                    Time.timeScale = 0f;

                    //aggiunta monete accomulate durante il gioco
                    player = FindObjectOfType<PlayerStats>();
                    PersistenceManager.PersistenceInstance.Coins += player.CurrentCoins;
                    PersistenceManager.PersistenceInstance.writeFile();

                    Debug.Log("Game is in Win state: congratulation!");
                    gameScreen.SetActive(false); //
                    winScreen.SetActive(true);
                    AudioManager.instance.PlaySFX("Win");
                }

                break;

            default:
                Debug.LogWarning("STATE DOES NOT EXIST");
                break;
        }
    }

    // test game over

    void TestSwitchState()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            ChangeState(GameState.GameOver);
        }
    }


    // Define the method to change the state of the game
    public void ChangeState(GameState newState)
    {
        currentState = newState;
    }



    public void PauseGame()
    {
        if (currentState != GameState.Paused)
        {
            previousState = currentState;
            ChangeState(GameState.Paused); // do this only if game not yet paused
            Time.timeScale = 0f; // Stop the game
            gameScreen.SetActive(false);
            pauseScreen.SetActive(true); // Enable the pause screen
            ShowStopwatchDisplayPauseScreen();
            Debug.Log("Game is paused");
        }

    }

    public bool IsGamePaused()
    {

        return (currentState == GameState.Paused || currentState == GameState.LevelUp || currentState == GameState.GameOver);
    }

    public bool IsGameOver()
    {

        return (currentState == GameState.GameOver);
    }

    public void ResumeGame()
    {
        if (currentState == GameState.Paused)
        {
            ChangeState(previousState);
            Time.timeScale = 1f; // Resume the game
            pauseScreen.SetActive(false); // Disable the pause screen
            gameScreen.SetActive(true);
            Debug.Log("Game is resumed");
        }
    }

    public void HideTutorial()
    {
        PersistenceManager.PersistenceInstance.FirstTime = false;
        PersistenceManager.PersistenceInstance.writeFile();
        Time.timeScale = 1f;
    }

    // Define the method to check for pause and resume input
    void CheckForPauseAndResume()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentState == GameState.Paused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void DisplayPauseScreen()
    {
        confirmScreen.SetActive(false);
        pauseScreen.SetActive(true);

    }

    public void DisplayQuitConfirmScreen()
    {
        confirmScreen.SetActive(true);

    }

    public void DisableScreens()
    {
        pauseScreen.SetActive(false);
        resultsScreen.SetActive(false);
        confirmScreen.SetActive(false);
        levelUpScreen.SetActive(false);
        winScreen.SetActive(false);
        bossWarningScreen.SetActive(false);
        tutorialScreen.SetActive(false);
    }

    public void GameOver()
    {
        timeSurvivedDisplay.text = stopwatchDisplay.text;
        previousState = currentState;
        ChangeState(GameState.GameOver);

    }

    public void StopGame()
    {
        Time.timeScale = 0f;
    }

    void DisplayResults()
    {
        resultsScreen.SetActive(true);
    }

    public void AssignChosenCharacterUI(string playerName, Sprite playerImage)
    {
        chosenCharacterImage.sprite = playerImage;
        chosenCharacterName.text = playerName;
        winCharacterImage.sprite = playerImage;
    }

    public void AssignLevelReachedUI(int levelReachedData)
    {
        levelReachedDisplay.text = levelReachedData.ToString();
    }


    public void AssignChosenWeaponsAndPassiveItemsUI(List<Image> chosenWeaponsData, List<Image> chosenPassiveItemsData, List<Image> chosenDefensivePowerUpData)
    {
        // Check that lists have the same length
        if (chosenWeaponsData.Count != chosenWeaponsUI.Count || chosenPassiveItemsData.Count != chosenPassiveItemsUI.Count || chosenDefensivePowerUpData.Count != chosenDefensivePowerUpUI.Count)
        {
            Debug.LogError("Chosen weapons and passive items data and defensive power up lists have different lengths");
            return;
        }

        // Assign chosen weapons data to chosenWeaponsUI
        for (int i = 0; i < chosenWeaponsUI.Count; i++)
        {
            // Check that the sprite of the corresponding element in chosenWeaponsData is not null
            if (chosenWeaponsData[i].sprite)
            {
                // Enable the corresponding element in chosenWeaponsUI and set its sprite to the corresponding sprite in chosenWeaponsData
                chosenWeaponsUI[i].enabled = true;
                chosenWeaponsUI[i].sprite = chosenWeaponsData[i].sprite;
            }
            else
            {
                // If the sprite is null, disable the corresponding element in chosenWeaponsUI
                chosenWeaponsUI[i].enabled = false;
            }
        }

        // Assign chosen passive items data to chosenPassiveItemsUI
        for (int i = 0; i < chosenPassiveItemsUI.Count; i++)
        {
            // Check that the sprite of the corresponding element in chosenPassiveItemsData is not null
            if (chosenPassiveItemsData[i].sprite)
            {
                // Enable the corresponding element in chosenPassiveItemsUI and set its sprite to the corresponding sprite in chosenPassiveItemsData
                chosenPassiveItemsUI[i].enabled = true;
                chosenPassiveItemsUI[i].sprite = chosenPassiveItemsData[i].sprite;
            }
            else
            {
                // If the sprite is null, disable the corresponding element in chosenPassiveItemsUI
                chosenPassiveItemsUI[i].enabled = false;
            }
        }

        // Assign chosen defensive power up data to chosenDefensivePowerUpUI
        for (int i = 0; i < chosenDefensivePowerUpUI.Count; i++)
        {
            // Check that the sprite of the corresponding element in chosenDefensivePowerUpData is not null
            if (chosenDefensivePowerUpData[i].sprite)
            {
                // Enable the corresponding element in chosenDefensivePowerUpUI and set its sprite to the corresponding sprite in chosenDefensivePowerUpData
                chosenDefensivePowerUpUI[i].enabled = true;
                chosenDefensivePowerUpUI[i].sprite = chosenDefensivePowerUpData[i].sprite;
            }
            else
            {
                // If the sprite is null, disable the corresponding element in chosenDefensivePowerUpUI
                chosenDefensivePowerUpUI[i].enabled = false;
            }
        }
    }


    void UpdateStopwatch()
    {
        stopwatchTime += Time.deltaTime;

        UpdateStopwatchDisplay();

        if (stopwatchTime >= timeLimit && !BossFightTime)
        {
            //GameOver(); // change: call here function to start game with boss
            //Mostra schermata della boss warning
            
            AudioManager.instance.StopMusic();


            BossFightTime = true;
            player = FindAnyObjectByType<PlayerStats>();

            DontDestroyOnLoad(player);
            DontDestroyOnLoad(instance);
            DontDestroyOnLoad(canvas);
        }
    }

    public void DestroyElements()
    {
        Debug.Log("I'm trying to destroy the player to go back to menu");
        player = FindAnyObjectByType<PlayerStats>();
        HealthBarBossFight healthBar = FindAnyObjectByType<HealthBarBossFight>();
        if (healthBar)
        {
            Destroy(healthBar.gameObject);
            healthBar = null;
        }
        Destroy(player.gameObject);
        player = null;
        Destroy(canvas);
        canvas = null;
        Destroy(gameObject);

    }

    void UpdateStopwatchDisplay()
    {
        // Calculate the number of minutes and seconds that have elapsed
        int minutes = Mathf.FloorToInt(stopwatchTime / 60);
        int seconds = Mathf.FloorToInt(stopwatchTime % 60);

        // Update the stopwatch text to display the elapsed time
        stopwatchDisplay.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void ShowStopwatchDisplayPauseScreen()
    {
        // Calculate the number of minutes and seconds that have elapsed
        int minutes = Mathf.FloorToInt(stopwatchTime / 60);
        int seconds = Mathf.FloorToInt(stopwatchTime % 60);

        // Update the stopwatch text to display the elapsed time
        currentTimeDisplay.text = "" + string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void StartLevelUp()
    {
        ChangeState(GameState.LevelUp);
        playerObject.SendMessage("RemoveAndApplyUpgrades");
    }

    public void EndLevelUp()
    {
        choosingUpgrade = false;
        Time.timeScale = 1f;    // Resume the game
        levelUpScreen.SetActive(false);
        ChangeState(GameState.Gameplay);
    }

    public void ShowBossHealthBar()
    {
        BossHealthBar.SetActive(true);
    }

    // Call this method to hide the canvas
    public void HideBossHealthBar()
    {
        BossHealthBar.SetActive(false);
    }



}