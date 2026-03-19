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
public class ScoreData
{
    public int S_Rank = 50000;
    public int A_Rank = 10000;
    public int B_Rank = 5000;
    public int C_Rank = 2500;
    public int D_Rank = 1000;
}

[System.Serializable]
public class TimeScore
{
    public int time = 5;
    public int Score = 50000;
}