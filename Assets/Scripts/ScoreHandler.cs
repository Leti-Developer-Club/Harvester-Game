using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ScoreHandler : MonoBehaviour
{
    //add reference to the ui text elements that displays the score
    public TMP_Text scoreText;
    public TMP_Text currentTimeText;
   
    public int scorePoints;
    public int scoreTarget;
    public float currentTime; // total time in seconds

    public LevelManager levelManager;
    public GameManager gameManager;


    //when basket collides with an object
    public void HandleBasketCollision(int points)
    {
        LevelPrefabs scoreTarget = levelManager.GetCurrentLevelConfig();

        scorePoints += points;
        //scorePoints = Mathf.Max(scorePoints, 0);
        UpdateScoreDisplay();
        Debug.Log($"Score Updated: {scorePoints}");

        //update the level manager when score is updated NEW CODE!!
        //if(scorePoints >= scoreTarget)
        if (scorePoints >= scoreTarget.targetScore)
        {
            //update level manager (Win state)
            //FindObjectOfType<LevelManager>().EndLevel(true);
            levelManager.EndLevel(true);
        }
    }

    //update the score display on the ui
    private void UpdateScoreDisplay()
    {
        if (scoreText != null)
        {
            LevelPrefabs scoreTarget = levelManager.GetCurrentLevelConfig();
            scorePoints = Mathf.Max(scorePoints, 0);
            //scoreText.text = "Score: " + scorePoints;
            scoreText.text = $"Score: {scorePoints} / {scoreTarget.targetScore}";
        }
    }

    private void UpdateTimerDisplay()
    {
        //update the timer ui
        if (currentTimeText != null)
        {
            int minutes = Mathf.FloorToInt(currentTime / 60); // calculate minutes
            int seconds = Mathf.FloorToInt(currentTime % 60);
            currentTimeText.text = $"{minutes: 00}:{seconds:00}";
        }
    }

    //timer logic
    public void TimerCountdown()
    {
        if(currentTime > 0)
        {
            //decrease time based on real time seconds
            currentTime -= Time.deltaTime;
            UpdateTimerDisplay();            
        }
        else
        {
            //set lose state  when timer runs  out
            if (currentTime <= 0)
            {
                currentTime = 0;
                Debug.Log("Time is up");

                //Update the LevelManager that the level has ended because the timer ran out (Lose state)
                FindObjectOfType<LevelManager>().EndLevel(false);
            }
            
        }

    }


    //reset score on game end
    public void ResetScore()
    {
        scorePoints = 0;
        UpdateScoreDisplay();
        Debug.Log("Score reset");
    }

    //ADD RESET TIMER Function
    public void ResetTimer(float newTime)
    {
        currentTime = newTime;
        UpdateTimerDisplay();
        gameManager.ResetGameOverState();
        Debug.Log("Timer reset");
    }

    //NEW CODE
    public void SetScoreTarget(int target)
    {
        scoreTarget = target;
    }

    // Set the timer for the current level
    public void SetTimeTarget(float time)
    {
        currentTime = time;
    }

}
