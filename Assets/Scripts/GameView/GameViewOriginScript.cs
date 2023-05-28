using Assets.Scripts.GameModel;
using System;
using UnityEngine;
using Zenject;

public abstract class GameViewOriginScript : MonoBehaviourLogger
{
    [Inject] protected GameManager gameManager;
    /// <summary>
    /// Модель игры, реализовывает логику взаимодействия всех частей.
    /// </summary>
    public Game gameModel
    {
        get
        {
            return this.gameManager.gameModel;
        }
    }

    /// <summary>
    /// Получить клон указанного по имени префаба из провайдер.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    protected GameObject GetPrefabClone(String name)
    {
        return this.gameManager.prefabsProvider.GetPrefabClone(name);
    }

    /// <summary>
    /// Включить или отключить объект этого скрипта.
    /// </summary>
    /// <param name="isActive"></param>
    public void SetActive(Boolean isActive)
    {
        this.gameObject.SetActive(isActive);
    }
    /// <summary>
    /// Включен или отключен объект этого скрипта.
    /// </summary>
    public Boolean activeSelf
    {
        get => this.gameObject.activeSelf;
    }
    protected virtual void Start() 
    {
        this.gameManager.AddGameViewOriginScript(this);
    }
    protected virtual void OnDestroy() { }
}
