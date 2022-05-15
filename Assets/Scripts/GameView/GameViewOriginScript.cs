using Assets.Scripts.GameModel;
using System;
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


    /// <summary>
    /// �������� ���� ���������� �� ����� ������� �� ���������.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    protected GameObject GetPrefabClone(String name)
    {
        return GameManager.instance.prefabsProvider.GetPrefabClone(name);
    }
}
