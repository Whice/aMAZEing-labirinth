using Assets.Scripts.GameModel;
using System;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameViewOriginScript : MonoBehaviourLogger
{
    /// <summary>
    /// Модель игры, реализовывает логику взаимодействия всех частей.
    /// </summary>
    public Game gameModel
    {
        get => GameManager.instance.gameModel;
    }


    /// <summary>
    /// Получить клон указанного по имени префаба из провайдер.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    protected GameObject GetPrefabClone(String name)
    {
        return GameManager.instance.prefabsProvider.GetPrefabClone(name);
    }

    #region Симуляция/считывание клика по объекту.

    /// <summary>
    /// Событие клика на стрелочку.
    /// </summary>
    public event Action OnSlotClicked;
    /// <summary>
    /// Симулировать клик по объекту.
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

    #endregion Симуляция/считывание клика по объекту.
}
