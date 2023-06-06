using Assets.Scripts.GameModel.Player;
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
    }
}