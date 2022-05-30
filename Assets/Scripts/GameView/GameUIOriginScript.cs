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
    }
}