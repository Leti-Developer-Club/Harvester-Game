using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private ScoreHandler scoreHandler;
    private ObjectPooling objectPooling;
    private GameManager gameManager;
    //array of level prefabs for each level
    public LevelPrefabs[] levelConfigs;
    public AudioScript audioManager;

    public int currentLevelIndex = 0;
    private float levelTimer;

    public bool isLevelActive = false;


    private void Start()
    {
        scoreHandler = FindObjectOfType<ScoreHandler>();
        objectPooling = FindObjectOfType<ObjectPooling>();
        gameManager = FindObjectOfType<GameManager>();

        if (objectPooling == null)
        {
            Debug.LogError("ObjectPooling component not found. Ensure the Object Pooling Manager GameObject is active and has the script attached.");
            return;
        }

        //start with the current level
        //LoadLevel(currentLevelIndex); //Giving an error revisit
    }



    public void LoadLevel(int levelIndex)
    {
        //check if the level is within the array bounds
        if (levelIndex < 0 || levelIndex >= levelConfigs.Length)
        {
            Debug.LogWarning("Invalid level index");
            return;
        }

        currentLevelIndex = levelIndex;

        //load level specific prefabs into the object pool
        objectPooling.LoadLevelPrefabs(levelConfigs[levelIndex]);

        //set the level specific properties
        levelTimer = levelConfigs[levelIndex].levelDuration;
        ///set the time target for the level
        scoreHandler.SetTimeTarget(levelTimer);

        //set the score target for the level
        scoreHandler.SetScoreTarget(levelConfigs[levelIndex].targetScore);
        scoreHandler.ResetScore();


        //reset the timer in score handler script
        scoreHandler.ResetTimer(levelTimer);

        //turn the ui off and resume the game
        gameManager.WinGame(false);
        Debug.Log("Win UI turned off"); //turn the ui off and resume the game

        //turn the lose ui off and restart game
        //gameManager.LoseGame(false);
        //Debug.Log("Lose UI turned off"); //turn the ui off and resume the game

        audioManager.PlayBackground(true);

        isLevelActive = true;
        Debug.Log($"Level {levelIndex + 1} started: Timer = {levelTimer}s, Target Score = {scoreHandler.scoreTarget}");

       

    }

    // Method to load the next level automatically
    public void LoadNextLevel()
    {
        if (currentLevelIndex + 1 < levelConfigs.Length)
        {
            LoadLevel(currentLevelIndex + 1);
        }
        else
        {
            Debug.Log("All levels completed! Game Over.");
            gameManager.EndOfGame();
            
        }
    }

    public void EndLevel(bool isWin)
    {
        isLevelActive = false; // Stop the level

        if (isWin)
        {
            Debug.Log($"Level {currentLevelIndex + 1} completed!");
            //trigger the win ui
            gameManager.WinGame(true);
        }
        else
        {
            Debug.Log($"Level {currentLevelIndex + 1} failed. Try again.");
            //trigger the lose ui
            gameManager.LoseGame(true);
            
        }
    }


    // Call this to reset the current level manually
    public void ResetLevel()
    {
        Debug.Log($"Resetting Level {currentLevelIndex + 1}...");
        LoadLevel(currentLevelIndex);
    }


    //get the current level prefab
    public LevelPrefabs GetCurrentLevelConfig()
    {
        if (currentLevelIndex >= 0 && currentLevelIndex < levelConfigs.Length)
        {
            return levelConfigs[currentLevelIndex];
        }
        return null;
    }
}
