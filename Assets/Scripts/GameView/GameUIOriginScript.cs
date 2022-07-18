using Assets.Scripts.GameModel;
using Assets.Scripts.GameModel.Player;
using System;

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
            GameInterfaceRectanglesDetected.instance.AddGameViewOriginScripts(this);
        }

        /// <summary>
        /// Подписаться на все нужные события.
        /// Используется во время инициализации скрипта.
        /// </summary>
        protected virtual void Subscribe()
        {
        }
        /// <summary>
        /// Отписаться от ненужных событий.
        /// Используется во время инициализации скрипта.
        /// Поу молчанию используется при уничтожении объекта.
        /// </summary>
        protected virtual void Unsubscribe()
        {
        }
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