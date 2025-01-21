using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    bool isGameStart = false;
    public UIManager uiManager;
    public ObjectPooling objectPooling;
    public ScoreHandler scoreHandler;
    public LevelManager levelManager;
    public AudioScript audioManager;

    // Input Action reference for the pause action
    private PlayerControls playerControls;
    private bool isPaused = false;
    private bool isGameOver = false;

    public GameObject fireworksEffect;


    private void Start()
    {
        //ensure game is paused on start
        Time.timeScale = 0f;
        Debug.Log("Game has been paused");

    }

    #region Player controls (Pause)

    private void OnEnable()
    {
        // Enable input actions
        playerControls.Enable();

        // Bind the pause action to the TogglePause method
        playerControls.GamePlayActions.Pause.performed += _ => TogglePause();
    }

    private void OnDisable()
    {
        // Disable input actions when the GameManager is disabled
        playerControls.Disable();
    }

    private void TogglePause()
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            //Time.timeScale = 0f;  // Pause the game
            PauseGame();
        }
        else
        {
            //Time.timeScale = 1f;  // Resume the game
            ResumeGame();
        }
    }

    #endregion

    private void Awake()
    {
        playerControls = new PlayerControls();
    }

    public void StartGame()
    {
        if (!isGameStart)
        {
            isGameStart = true;
            Debug.Log("Game Started");
            
            //hide the start panel
            uiManager.StartGameMenuInactive();
            objectPooling.InitializePools();

            //start the game logic
            Time.timeScale = 1.0f;

            //play the background music
            audioManager.PlayBackground(true);

            //load level 0 on start
            levelManager.LoadLevel(0);

        }   

    }

    private void Update()
    {
        if (!isGameOver)
        {
            scoreHandler.TimerCountdown();

            //check if the timer has reached zero
            if(scoreHandler.currentTime <= 0)
            {
                isGameOver = true;
                //call the lose game logic
                LoseGame(true);
            }
        }
        
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        uiManager.PauseGameMenu();
    }

    public void ResumeGame()
    {
        Time.timeScale = 1.0f;
        uiManager.StartGameMenuInactive();
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 0f;
        uiManager.StartGameMenuActive();
        isGameStart = false;
        //Call reset game method here (this resets the game instead of the level) BUG: CHECK ON THIS.. AFTER TESTING CONSIDER RESTARTING THE ENTIRE GAME INSTEAD OF THE CURRENT LEVEL
        InitializedGame();

        //stop Background music from playing 
        audioManager.PlayBackground(false);

    }

    public void OpenCredits()
    {
        uiManager.CreditsMenuOpen();
    }
    public void CloseCredits()
    {
        uiManager.CreditsMenuClose();
    }


    public void WinGame(bool isUiActive)
    {
        if (isUiActive)
        {
            //pause the game and show the win UI
            Time.timeScale = 0f;
            uiManager.WinGameMenu(true);

            //play the win game sound
            audioManager.PlayWinGame(true);
            //stop Background music from playing 
            audioManager.PlayBackground(false);

            if(fireworksEffect != null)
            {
                fireworksEffect.SetActive(true);
                Debug.Log("Firework play;");
            }
        }
        else
        {
            //resume the game and hide the win UI
            Time.timeScale = 1f;
            uiManager.WinGameMenu(false);

            if (fireworksEffect != null)
            {
                fireworksEffect.SetActive(false);
            }
        }
        
        
    }

    public void LoseGame(bool isUiActive)
    {
        if(isUiActive)
        {
            Time.timeScale = 0f;
            uiManager.LoseGameMenu(true);
            //play the lose game sound
            audioManager.PlayLoseGame(true);
            //stop Background music from playing 
            audioManager.PlayBackground(false);
        }
        
        else
        {
            Time.timeScale = 1f;
            uiManager.LoseGameMenu(false);
        }

    }

    //code to reset the game components
    public void InitializedGame()
    {
        //reset the score
        scoreHandler.ResetScore();

        //Reset the object pool
        objectPooling.ResetPools();
    }

    public void LoadNextLevelButton()
    {
        if (levelManager != null)
        {
            levelManager.LoadNextLevel(); // Calls LoadNextLevel from LevelManager
            audioManager.PlayBackground(true);
        }
    }

    public void LoadSpecificLevelButton(int levelIndex)
    {
        if (levelManager != null)
        {
            levelManager.LoadLevel(levelIndex); // Calls LoadLevel with a specific index
        }
    }

    //TODO: Add functionality
    public void RestartLevel()
    {        
        InitializedGame();
        
    }

    public void ResetGameOverState()
    {
        //this to fix level timer not working on level replay
        isGameOver = false; // Reset game over state
    }

    public void QuitGame()
    {
        //leave the game
        Application.Quit();
    }

    public void EndOfGame()
    {
        uiManager.EOG();
    }

   
}
