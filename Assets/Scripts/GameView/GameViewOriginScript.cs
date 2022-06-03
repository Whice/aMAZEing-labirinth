using Assets.Scripts.GameModel;
using System;
using UnityEngine;

public abstract class GameViewOriginScript : MonoBehaviourLogger
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

    /// <summary>
    /// Включить или отключить объект этого скрипта.
    /// </summary>
    /// <param name="isEnable"></param>
    public void SetEnableObject(Boolean isEnable)
    {
        this.gameObject.SetActive(isEnable);
    }
    /// <summary>
    /// Включен или отключен объект этого скрипта.
    /// </summary>
    public Boolean activeSelf
    {
        get => this.gameObject.activeSelf;
    }
    protected virtual void Awake() { }
    protected virtual void OnDestroy() { }
}
