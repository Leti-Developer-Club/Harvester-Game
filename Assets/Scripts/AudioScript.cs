using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioScript : MonoBehaviour
{
    // A centalised manager for all the audio in the game

    public AudioSource winGameClip;
    public AudioSource loseGameClip;
    public AudioSource backgroundMusicClip;
    public AudioSource fruitCollectClip;
    public AudioSource enemyCollectClip;

    public void PlayWinGame(bool isPlaying)
    {
        if (winGameClip != null)
        {
            if (isPlaying)
            {
                winGameClip.Play();
                Debug.Log("Win game sound plays");
            }
            else
            {
                winGameClip.Stop();
                Debug.Log("Stop win game the music");
            }
        } 
        else
        {
            Debug.Log("AudiSource is null");
        }
    }

    public void PlayLoseGame(bool isPlaying)
    {
        if (loseGameClip != null)
        {
            if (isPlaying)
            {
                loseGameClip.Play();
                Debug.Log("Lose game sound plays");
            }
            else
            {
                loseGameClip.Stop();
                Debug.Log("Stop lose game the music");
            }
        }
        else
        {
            Debug.Log("AudiSource is null");
        }
    }

    public void PlayBackground(bool isPlaying)
    {
        
        if (backgroundMusicClip != null )
        {
            if (isPlaying)
            {
                backgroundMusicClip.Play();
                Debug.Log("Background sound plays");
            }
            else
            {
                backgroundMusicClip.Stop();
                Debug.Log("Stop background the music");
            }  
        }
        else
        {
            Debug.Log("AudiSource is null");
        }
    }

    public void PlayFruitCollection(bool isPlaying)
    {
        if (fruitCollectClip != null)
        {
            if (isPlaying)
            {
                fruitCollectClip.Play();
                Debug.Log("Fruit Collect sound plays");
            }
            else
            {
                fruitCollectClip.Stop();
                Debug.Log("Stop fruit the music");
            }
        }
        else
        {
            Debug.Log("AudiSource is null");
        }
    }

    public void PlayEnemyCollection(bool isPlaying)
    {
        if (enemyCollectClip != null)
        {
            if (isPlaying)
            {
                enemyCollectClip.Play();
                Debug.Log("Enemy Collect sound plays");
            }
            else
            {
                enemyCollectClip.Stop();
                Debug.Log("Stop enemy the music");
            }
        }
        else
        {
            Debug.Log("AudiSource is null");
        }
    }


}
