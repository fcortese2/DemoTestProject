using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();

        if (GameData.Instance.WonGame)
        {
            button.interactable = false;
        }
        else
        {
            button.interactable = true;
        }
    }

    public void LoadGameScene()
    {
        if (GameData.Instance.WonGame)
        {
            return;
        }
        
        GameSetup.Instance.IsLoadingSave = true;
        SceneManager.LoadScene(1);
    }
}
