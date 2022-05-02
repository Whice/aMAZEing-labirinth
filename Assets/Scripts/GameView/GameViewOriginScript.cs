using Assets.Scripts.GameModel;
using UnityEngine;

public class GameViewOriginScript : MonoBehaviourLogger
{
    /// <summary>
    /// ������ ����, ������������� ������ �������������� ���� ������.
    /// </summary>
    public Game gameModel
    {
        get => GameManager.instance.gameModel;
    }
}
