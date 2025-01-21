using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Scriptable Object to store level-specific prefabs
[CreateAssetMenu(fileName = "LevelPrefabs", menuName = "Game/Level Prefabs")]
public class LevelPrefabs : ScriptableObject
{
    public GameObject[] fruitPrefabs;
    public GameObject[] enemyPrefabs;
    public int targetScore; // set score target for this level
    public float levelDuration; //set the duration for the level
    public string levelName;


    // Add these fields for spawn intervals (New CODE!)
    public float fruitSpawnInterval;
    public float enemySpawnInterval;

}
