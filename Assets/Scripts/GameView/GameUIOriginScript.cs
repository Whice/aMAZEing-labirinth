﻿using Assets.Scripts.GameModel.Player;
using SummonEra.RxEvents;
using System;
using Zenject;

namespace UI
{
    public class GameUIOriginScript : GameViewOriginScript
    {
        /// <summary>
        /// Элемент интерфейса не должен позволять нажимать на объекты в мире.
        /// </summary>
        public Boolean isShouldInterruptPressesInWorld = true;
        /// <summary>
        ///  Игрок, которому принадлежит текущий ход.
        /// </summary>
        protected GamePlayer currentPlayer
        {
            get => this.gameModel.currentPlayer;
        }
        protected override void Awake()
        {
            base.Awake();
            new TestRxEvent { message = "UI script created!" }.Publish();
        }

        [Inject] private GameInterfaceRectanglesDetector rectanglesUIDetector;
        protected void OnEnable()
        {
            rectanglesUIDetector.AddGameViewOriginScripts(this);
        }
        protected void OnDisable()
        {
            rectanglesUIDetector.RemoveGameViewOriginScripts(this);
        }
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
        /// Инициализировать элементы интерфейса для новой модели.
        /// </summary>
        public virtual void Initialized()
        {
            Unsubscribe();
            Subscribe();
        }

        protected override void OnDestroy()
        {
            Unsubscribe();
            base.OnDestroy();
        }
    }
}