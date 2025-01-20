using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    //show start menu on game start
    public GameObject startPanel;
    public GameObject pausePanel;
    public GameObject winPanel;
    public GameObject losePanel;
    public GameObject inGameUIPanel;
    public GameObject endOfGamePanel;
    public GameObject creditsPanel;

    public LevelManager levelManager;

    public TMP_Text levelNameText;

    bool isMenuActive = true;

    public void StartGameMenuInactive()
    {
        if(startPanel != null)
        {
            startPanel.SetActive(false);
            pausePanel.SetActive(false);
            winPanel.SetActive(false);
            losePanel.SetActive(false);
            inGameUIPanel.SetActive(true);
            endOfGamePanel.SetActive(false);
            creditsPanel.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Start Panel is NULL");
        }
    }

    public void PauseGameMenu()
    {
        if (pausePanel != null)
        {
            startPanel.SetActive(false);
            pausePanel.SetActive(true);
            winPanel.SetActive(false);
            losePanel.SetActive(false);
            inGameUIPanel.SetActive(false);
            endOfGamePanel.SetActive(false);
        }
        else
        {
            Debug.LogWarning("pause Panel is NULL");
        }
    }

    public void CreditsMenuOpen()
    {
        creditsPanel.SetActive(true);
        startPanel.SetActive(false);
    }
    public void CreditsMenuClose()
    {
        creditsPanel.SetActive(false);
        startPanel.SetActive(true);
    }

    public void WinGameMenu(bool menuActive)
    {
        isMenuActive = menuActive;

        LevelPrefabs currentlevelName = levelManager.GetCurrentLevelConfig();
        {
            Debug.Log($"Current Level Name: {currentlevelName.levelName}");
        }


        if (winPanel != null)
        { 
            if (menuActive)
            {
                //show the win menu
                startPanel.SetActive(false);
                pausePanel.SetActive(false);
                winPanel.SetActive(true);
                losePanel.SetActive(false);
                inGameUIPanel.SetActive(false);
                endOfGamePanel.SetActive(false);

                if (levelNameText!= null)
                {
                    levelNameText.text = $"Level {currentlevelName.levelName} Completed ";
                }
            }
            else
            {
                //hide the win menu
                winPanel.SetActive(false);
                inGameUIPanel.SetActive(true);
                losePanel.SetActive(false);
            }            
        }
        else
        {
            Debug.LogWarning("Win Panel is NULL");
        }
    }

    public void LoseGameMenu(bool menuActive)
    {
        if (losePanel != null)
        {
            if (menuActive)
            {
                startPanel.SetActive(false);
                pausePanel.SetActive(false);
                winPanel.SetActive(false);
                losePanel.SetActive(true);
                inGameUIPanel.SetActive(false);
                endOfGamePanel.SetActive(false);
            }                
        }
        else
        {
            Debug.LogWarning("lose Panel is NULL");
        }
    }

    public void StartGameMenuActive()
    {
        if (startPanel != null)
        {
            startPanel.SetActive(true);
            pausePanel.SetActive(false);
            winPanel.SetActive(false);
            losePanel.SetActive(false);
            inGameUIPanel.SetActive(false);
            endOfGamePanel.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Start Panel is NULL");
        }
    }

    //screen that shows at the end of the game
   public void EOG()
   {
        endOfGamePanel.SetActive(true);
        winPanel.SetActive(false);
   }

}
