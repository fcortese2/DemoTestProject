using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Singleton SO/Game Setup", fileName = "GameSetup")]
public class GameSetup : SingletonScriptableObject<GameSetup>
{
    public Vector2Int TableSize;
}