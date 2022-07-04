using System;
using Assets.Scripts.Extensions;
using Assets.Scripts.GameModel.Logging;
using Assets.Scripts.GameModel.TurnPhaseAndExtensions;

namespace Assets.Scripts.GameModel.Commands.GameCommands
{
    /// <summary>
    /// Команда для перемещения аватара игрока.
    /// </summary>
    [Serializable]
    public class AvatarMoveCommand : GameCommand
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
        private byte playerNumberPrivate;
        /// <summary>
        /// Номер игрока, нужен для проверки правильности игрока.
        /// </summary>
        public byte playerNumber
        {
            get => playerNumberPrivate;
        }
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
            this.playerNumberPrivate = (byte)playerNumber;
        }

        public override bool Execute(Game modelGame)
        {
            Boolean result = base.Execute(modelGame);

            if (modelGame.currentPhase != TurnPhase.movingAvatar)
            {
                GameModelLogger.LogError("The expected phase is " + nameof(TurnPhase.movingAvatar)
                    + ", but phase in model is " + modelGame.currentPhase.ToString());
                return false;
            }

            if (modelGame.currentPlayer.number != this.playerNumberPrivate)
            {
                GameModelLogger.LogError("When you execute a command, the number of the player " +
                    "in the model does not equal the number of the player in the command!");
                return false;
            }

            result &= modelGame.SetPlayerAvatarToField(this.playerMoveToX, this.playerMoveToY);

            return result;
        }
        public override bool Undo(Game modelGame)
        {
            Boolean result = base.Undo(modelGame);

            result &= modelGame.SetPlayerAvatarToField(this.playerMoveFromX, this.playerMoveFromY, false);

            return result;
        }

        #region Клонирование.

        public override GameCommand Clone()
        {
            return GetAvatarMoveCommandClone();
        }
        /// <summary>
        /// Выполнить глубокое клонирование команды перемещения аватара и получить клон.
        /// </summary>
        /// <returns></returns>
        public AvatarMoveCommand GetAvatarMoveCommandClone()
        {
            AvatarMoveCommand clone = new AvatarMoveCommand();
            clone.playerMoveFrom = this.playerMoveFrom;
            clone.playerMoveTo = this.playerMoveTo;
            clone.playerNumberPrivate = this.playerNumberPrivate;

            return clone;
        }

        #endregion Клонирование.

        #region Сравнение.

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj is AvatarMoveCommand avatarMoveCommand)
            {
                if (this.playerMoveTo != avatarMoveCommand.playerMoveTo)
                {
                    return false;
                }
                if (this.playerMoveFrom != avatarMoveCommand.playerMoveFrom)
                {
                    return false;
                }
                if (this.playerNumberPrivate != avatarMoveCommand.playerNumberPrivate)
                {
                    return false;
                }

                return true;
            }

            return false;
        }
        public override Int32 GetHashCode()
        {
            Int32 hashCode = this.playerMoveFrom.GetHashCode();
            hashCode ^= this.playerMoveTo.GetHashCode();
            hashCode ^= this.playerNumberPrivate;

            return hashCode;
        }
        public static bool operator ==(AvatarMoveCommand l, AvatarMoveCommand r)
        {
            if (l is null && r is null)
                return true;
            else if (l is null)
                return false;
            else
                return l.Equals(r);
        }
        public static bool operator !=(AvatarMoveCommand l, AvatarMoveCommand r)
        {
            return !(l == r);
        }

        #endregion Сравнение.
    }
}
