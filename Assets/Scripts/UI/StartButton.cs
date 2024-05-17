using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartButton : Button
{
    public Vector2Int mapSize = Vector2Int.one;

    protected override void Awake()
    {
        base.Awake();

        onClick.AddListener(() =>
        {
            GameSetup.Instance.TableSize = mapSize;
            SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
        });

        GetComponentInChildren<TMP_Text>().text = $"Play {mapSize.x}x{mapSize.y}";
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(StartButton))]
[CanEditMultipleObjects]
public class StartButtonEditor:Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
}
#endif