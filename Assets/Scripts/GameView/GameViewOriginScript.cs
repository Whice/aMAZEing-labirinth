using Assets.Scripts.GameModel;
using UnityEngine;

public class GameViewOriginScript : MonoBehaviourLogger
{
    /// <summary>
    /// Модель игры, реализовывает логику взаимодействия всех частей.
    /// </summary>
    public Game gameModel
    {
        get => GameManager.instance.gameModel;
    }
}
