using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Singleton SO/Game Data", fileName = "GameData")]
public class GameData : SingletonScriptableObject<GameData>
{
    [SerializeField] private int score = 0;

    public int Score
    {
        get => score;
        set
        {
            Debug.Log($"UPPING SCORE TO {value}");
            score = value;

            OnScoreChanged.Invoke(score);
        }
    }

    public UnityEvent<int> OnScoreChanged = new UnityEvent<int>();

    public int MatchesStraight = 1;

    public List<string> GameCards = new List<string>();
    public List<string> PairsFound = new List<string>();

    public bool WonGame = false;
}
