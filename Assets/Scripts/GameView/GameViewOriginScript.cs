using Assets.Scripts.GameModel;
using System;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;

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

    #region ���������/���������� ����� �� �������.

    /// <summary>
    /// ������� ����� �� ���������.
    /// </summary>
    public event Action OnSlotClicked;
    /// <summary>
    /// ������������ ���� �� �������.
    /// </summary>
    public virtual void SimulateOnClick()
    {
        this.OnSlotClicked?.Invoke();
    }
    private void OnMouseUp()
    {
        if (!GameInterfaceRectanglesDetected.instance.isPointerOnUIElement)
        {
            SimulateOnClick();
        }
    }
    private void OnMouseDown()
    {
    }

    #endregion ���������/���������� ����� �� �������.
}
