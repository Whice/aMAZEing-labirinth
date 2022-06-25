﻿using System;
using Assets.Scripts.Extensions;
using Assets.Scripts.GameModel.Logging;
using Assets.Scripts.GameModel.TurnPhaseAndExtensions;

namespace Assets.Scripts.GameModel.Commands.GameCommands
{
    /// <summary>
    /// Команда для перемещения аватара игрока.
    /// </summary>
    public class PlayerMoveCommand : GameCommand
    {
        #region Откуда игрок идет.

        /// <summary>
        /// Откуда игрок должен пойти. Содержит обе координаты, т.к. они занимают меньше 4х битов.
        /// </summary>
        private TwoBytesInOneKeeper playerMoveFrom;
        /// <summary>
        /// Откуда игрок должен пойти по горизонтали.
        /// </summary>
        private byte playerMoveFromX
        {
            get => this.playerMoveFrom.firstValue;
            set => this.playerMoveFrom.firstValue = value;
        }
        /// <summary>
        /// Откуда игрок должен пойти по вертикали.
        /// </summary>
        private byte playerMoveFromY
        {
            get => this.playerMoveFrom.secondValue;
            set => this.playerMoveFrom.secondValue = value;
        }

        #endregion Откуда игрок идет.

        #region Куда игрок идет.

        /// <summary>
        /// Куда игрок должен пойти. Содержит обе координаты, т.к. они занимают меньше 4х битов.
        /// </summary>
        private TwoBytesInOneKeeper playerMoveTo;
        /// <summary>
        /// Куда игрок должен пойти по горизонтали.
        /// </summary>
        private byte playerMoveToX
        {
            get => this.playerMoveTo.firstValue;
            set => this.playerMoveTo.firstValue = value;
        }
        /// <summary>
        /// Куда игрок должен пойти по вертикали.
        /// </summary>
        private byte playerMoveToY
        {
            get => this.playerMoveTo.secondValue;
            set => this.playerMoveTo.secondValue = value;
        }

        #endregion Куда игрок идет.

        /// <summary>
        /// Номер игрока, нужен для проверки правильности игрока.
        /// </summary>
        private byte playerNumber;

        /// <summary>
        /// Инициализировать команду.
        /// </summary>
        /// <param name="playerMoveToX">Куда игрок должен пойти по горизонтали.</param>
        /// <param name="playerMoveToY">Куда игрок должен пойти по вертикали.</param>
        /// <param name="playerMoveFromX">Откуда игрок должен пойти по горизонтали.</param>
        /// <param name="playerMoveFromY">Откуда игрок должен пойти по вертикали.</param>
        /// <param name="playerNumber">Номер игрока, нужен для проверки правильности игрока.</param>
        public void Init(Int32 playerMoveToX, Int32 playerMoveToY,
            Int32 playerMoveFromX, Int32 playerMoveFromY, Int32 playerNumber)
        {
            this.playerMoveToX = (byte)playerMoveToX;
            this.playerMoveToY = (byte)playerMoveToY;
            this.playerMoveFromX = (byte)playerMoveFromX;
            this.playerMoveFromY = (byte)playerMoveFromY;
            this.playerNumber = (byte)playerNumber;
        }

        public override void Execute(Game modelGame)
        {
            base.Execute(modelGame);

            if (modelGame.currentPhase != TurnPhase.movingAvatar)
            {
                GameModelLogger.LogError("The expected phase is " + nameof(TurnPhase.movingAvatar)
                    + ", but phase in model is " + modelGame.currentPhase.ToString());
            }

            if (modelGame.currentPlayer.playerNumer != this.playerNumber)
            {
                GameModelLogger.LogError("When you execute a command, the number of the player " +
                    "in the model does not equal the number of the player in the command!");
                return;
            }

            modelGame.SetPlayerAvatarToField(this.playerMoveToX, this.playerMoveToY);
        }
        public override void Undo(Game modelGame)
        {
            base.Undo(modelGame);

            modelGame.SetPlayerAvatarToField(this.playerMoveToX, this.playerMoveToY);
        }
    }
}
