using Assets.Scripts.GameModel;
using RxEvents;
using SummonEra.RxEvents;
using System;
using UniRx;
using UnityEngine;
using Zenject;

public abstract class GameViewOriginScript : MonoBehaviourLogger
{
    [Inject] protected GameManager gameManager;
    protected CompositeDisposable disposables;
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

    #region Подписки.

    /// <summary>
    /// Подписаться на все нужные события.
    /// Используется во время инициализации скрипта.
    /// </summary>
    protected virtual void Subscribe() { }
    /// <summary>
    /// Отписаться от ненужных событий.
    /// Используется во время инициализации скрипта.
    /// Поу молчанию используется при уничтожении объекта.
    /// </summary>
    protected virtual void Unsubscribe() { }
    /// <summary>
    /// Инициализировать элементы для новой модели.
    /// </summary>
    public virtual void Initialize()
    {
        Unsubscribe();
        Subscribe();
    }
    /// <summary>
    /// Произошло событие инициализации игры (модели).
    /// </summary>
    /// <param name="msg"></param>
    private void OnGameInitialize(GameInitializeMessage msg)
    {
        Initialize();
    }

    #endregion Подписки.

    /// <summary>
    /// Включен или отключен объект этого скрипта.
    /// </summary>
    public Boolean activeSelf
    {
        get => this.gameObject.activeSelf;
    }
    protected virtual void Awake() 
    {
        disposables= new CompositeDisposable();
        disposables.Subscribe<GameInitializeMessage>(OnGameInitialize);
        this.gameManager.AddGameViewOriginScript(this);
    }


    protected virtual void OnDestroy()
    {
        Unsubscribe();
        this.disposables?.Dispose();
    }
}
