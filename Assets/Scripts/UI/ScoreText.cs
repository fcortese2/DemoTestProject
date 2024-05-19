using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class ScoreText : MonoBehaviour
{
    private TMP_Text text;

    private void Awake()
    {
        text = GetComponent<TMP_Text>();
        text.text = $"SCORE: {GameData.Instance.Score.ToString()}";
        
        GameData.Instance.OnScoreChanged.AddListener(score =>
        {
            text.text = $"SCORE: {score.ToString()}";
        });
    }
}
