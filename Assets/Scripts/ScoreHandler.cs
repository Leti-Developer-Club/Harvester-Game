using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreHandler : MonoBehaviour
{
    //add reference to the ui text elements that displays the score
    //public TMP_Text scoreText;
    public int scorePoints = 0;

    //when basket collides with an object
    public void HandleBasketCollision(int points)
    {
        scorePoints += points;
        UpdateScoreDisplay();
        Debug.Log($"Score Updated: {scorePoints}");
    }

    //update the score display on the ui
    private void UpdateScoreDisplay()
    {
        //if (scoreText != null)
        {
            //scoreText.text = "Score: " + scorePoints;
        }
    }

    //reset score on game end
    public void ResetScore()
    {
        scorePoints = 0;
        UpdateScoreDisplay();
        Debug.Log("Score reset");
    }


}
