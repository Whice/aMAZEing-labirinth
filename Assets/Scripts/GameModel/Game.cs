using Assets.Scripts.GameModel.Player;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.GameModel
{
    /// <summary>
    /// Модель игры.
    /// </summary>
    public class Game
    {
        public Game(GamePlayer[] players, Int32 startPlayerNumber)
        {
            this.players = players;
            this.currentPlayerNumber = startPlayerNumber;
        }

        /// <summary>
        /// Список игроков.
        /// </summary>
        private readonly GamePlayer[] players = null;
        /// <summary>
        ///  Номер игрока, которому принадлежит текущий ход.
        /// </summary>
        private Int32 currentPlayerNumber = 0;
        /// <summary>
        ///  Игрок, которому принадлежит текущий ход.
        /// </summary>
        public GamePlayer currentPlayer
        {
            get=>this.players[currentPlayerNumber];
        }

    }
}
