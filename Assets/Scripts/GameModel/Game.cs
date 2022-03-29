using Assets.Scripts.GameModel.Player;
using Assets.Scripts.GameModel.PlayingField;
using Assets.Scripts.GameModel.PlayingField.FieldCells;
using Assets.Scripts.GameModel.TurnPhaseAndExtensions;
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
        /// Выбрать начального игрока и вернуть его номер.
        /// </summary>
        /// <returns></returns>
        private Int32 ChooseStartPlayerNumber()
        {
            Random random = new Random();
            return random.Next(players.Length);
        }
        /// <summary>
        /// Начать игру.
        /// </summary>
        public void Start()
        {
            this.currentPlayerNumber = ChooseStartPlayerNumber();
            this.currentPhasePrivate = TurnPhase.movingCell;
            this.fieldPrivate = new Field();
        }

        #region Данные игры.

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
        /// <summary>
        /// Текущая фаза хода игрока.
        /// </summary>
        private TurnPhase currentPhasePrivate = TurnPhase.unknow;
        /// <summary>
        /// Текущая фаза хода игрока.
        /// </summary>
        public TurnPhase currentPhase
        {
            get => this.currentPhasePrivate;
        }
        /// <summary>
        /// Игровое поле.
        /// </summary>
        private Field fieldPrivate = null;
        /// <summary>
        /// Игровое поле.
        /// </summary>
        public Field field
        {
            get => this.fieldPrivate;
        }
        /// <summary>
        /// Свободная ячейка.
        /// </summary>
        public FieldCell freeCell
        {
            get => this.field.freeFieldCell;
        }

        #endregion Данные игры.

        #region Действия.

        public void SetFreeCellToField(Int32 x, Int32 y)
        {
            
        }

        #endregion Действия.
    }
}
