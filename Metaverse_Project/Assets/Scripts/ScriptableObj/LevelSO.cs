using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BoatGame/Newlevel")]
public class LevelSO : ScriptableObject
{
    public LevelData levelData;
}

[System.Serializable]
public class LevelData
{
    [Header("Level Prefab")]
    public GameObject LevelPrefab;

    [Header("Player Settings")]
    public Vector3 PlayerSpawnPosition;
    public float PlayerSpawnDirection;

    [Header("Score Settings")]
    public TimeScore[] TimeScores;
    public ScoreData ScoreData;
}

[System.Serializable]
public class ScoreData //Scoring system for how good the player does. They will require a score higher than what is stated to get that particular rank.
{
    public int S_Rank = 50000;
    public int A_Rank = 10000;
    public int B_Rank = 5000;
    public int C_Rank = 2500;
    public int D_Rank = 1000;
}

[System.Serializable]
public class TimeScore // Gives a score based on how quick they are at the level. You can have as many or little Score gaps, but I would recommend having a score gap for each rank. (6)
{
    public int time = 5;
    public int Score = 50000;
}