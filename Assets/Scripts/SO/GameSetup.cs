using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Singleton SO/Game Setup", fileName = "Game Setup")]
public class GameSetup : ScriptableObject
{
    private static GameSetup instance;

    public static GameSetup Instance
    {
        get
        {
            if (!instance)
            {
                instance = Resources.Load<GameSetup>("Game Setup");
            }

            return instance;
        }
    }

    public Vector2Int TableSize;
}
