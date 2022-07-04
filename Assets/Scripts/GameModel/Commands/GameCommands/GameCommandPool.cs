using Assets.Scripts.GameModel.PlayingField;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.GameModel.Commands.GameCommands
{
    /// <summary>
    /// Пул игровых команд.
    /// </summary>
    public class GameCommandPool
    {
        /// <summary>
        /// Вместимость при создании пула.
        /// </summary>
        public const Int32 INITIAL_CAPASITY = 128;
        /// <summary>
        /// Список команд для передвижения аватара.
        /// </summary>
        private List<AvatarMoveCommand> avatarMoveCommands = new List<AvatarMoveCommand>(INITIAL_CAPASITY);
        /// <summary>
        ///  Список команд для передвижения ячеек.
        /// </summary>
        private List<CellMoveCommand> cellCommands = new List<CellMoveCommand>(INITIAL_CAPASITY);

        /// <summary>
        /// Создает пул и заполняет начальным количеством команд.
        /// </summary>
        public GameCommandPool(Boolean isZeroCapasity = false)
        {
            if (!isZeroCapasity)
            {
                for (Int32 i = 0; i < INITIAL_CAPASITY; i++)
                {
                    this.avatarMoveCommands.Add(new AvatarMoveCommand());
                    this.cellCommands.Add(new CellMoveCommand());
                }
            }
        }

        /// <summary>
        /// Вернуть команду в пул.
        /// </summary>
        /// <param name="command"></param>
        public void PutInPool(GameCommand command)
        {
            if (command is AvatarMoveCommand playerMoveCommand)
            {
                this.avatarMoveCommands.Add(playerMoveCommand);
            }
            else if (command is CellMoveCommand cellMoveCommand)
            {
                this.cellCommands.Add(cellMoveCommand);
            }
        }

        /// <summary>
        /// Получить команду для хода аватаром по заданным параметрам.
        /// </summary>
        /// <param name="playerMoveToX">Куда игрок должен пойти по горизонтали.</param>
        /// <param name="playerMoveToY">Куда игрок должен пойти по вертикали.</param>
        /// <param name="playerMoveFromX">Откуда игрок должен пойти по горизонтали.</param>
        /// <param name="playerMoveFromY">Откуда игрок должен пойти по вертикали.</param>
        /// <param name="playerNumber">Номер игрока, нужен для проверки правильности игрока.</param>
        /// <returns></returns>
        public AvatarMoveCommand GetPlayerMoveCommand(Int32 playerMoveToX, Int32 playerMoveToY,
            Int32 playerMoveFromX, Int32 playerMoveFromY, Int32 playerNumber)
        {
            Int32 lastListIndex = this.avatarMoveCommands.Count - 1;
            AvatarMoveCommand command = null;
            if (lastListIndex < 0)
            {
                command = new AvatarMoveCommand();
            }
            else
            {
                command = this.avatarMoveCommands[lastListIndex];
                this.avatarMoveCommands.RemoveAt(lastListIndex);
            }

            command.Init(playerMoveToX, playerMoveToY, playerMoveFromX, playerMoveFromY, playerNumber);

            return command;
        }
        /// <summary>
        ///  Получить команду для перемещения ячейки.
        /// </summary>
        /// <param name="numberLine">Номер линии.</param>
        /// <param name="side">Сторона, с которой вставляется ячейка.</param>
        /// <param name="turnsClockwiseCountBefore">Количество поворотов по часовой стрелке до совершения хода.</param>
        /// <param name="turnsClockwiseCountAfter">Количество поворотов по часовой стрелке после совершения хода.</param>
        /// <returns></returns>
        public CellMoveCommand GetCellMoveCommand(Int32 numberLine, FieldSide side, Int32 turnsClockwiseCountBefore, Int32 turnsClockwiseCountAfter)
        {
            Int32 lastListIndex = this.cellCommands.Count - 1;
            CellMoveCommand command = null;
            if (lastListIndex < 0)
            {
                command = new CellMoveCommand();
            }
            else
            {
                command = this.cellCommands[lastListIndex];
                this.cellCommands.RemoveAt(lastListIndex);
            }

            command.Init(numberLine, side, turnsClockwiseCountBefore, turnsClockwiseCountAfter);

            return command;
        }

        /// <summary>
        /// Выполнить глубокое клонирование пула и получить клон.
        /// </summary>
        /// <returns></returns>
        public GameCommandPool Clone()
        {
            GameCommandPool clone = new GameCommandPool(true);

            clone.cellCommands = new List<CellMoveCommand>(this.cellCommands.Capacity);
            for (Int32 index = 0; index < this.cellCommands.Count; index++)
            {
                clone.cellCommands.Add(this.cellCommands[index].GetCellMoveCommandClone());
            }
            clone.avatarMoveCommands = new List<AvatarMoveCommand>(this.avatarMoveCommands.Capacity);
            for (Int32 index = 0; index < this.avatarMoveCommands.Count; index++)
            {
                clone.avatarMoveCommands.Add(this.avatarMoveCommands[index].GetAvatarMoveCommandClone());
            }

            return clone;
        }

        #region Сравнение.

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj is GameCommandPool gameCommandPool)
            {
                for (Int32 index = 0; index < gameCommandPool.cellCommands.Count; index++)
                {
                    if (this.cellCommands[index] != gameCommandPool.cellCommands[index])
                    {
                        return false;
                    }
                }
                for (Int32 index = 0; index < gameCommandPool.avatarMoveCommands.Count; index++)
                {
                    if (this.avatarMoveCommands[index] != gameCommandPool.avatarMoveCommands[index])
                    {
                        return false;
                    }
                }

                return true;
            }

            return false;
        }
        public override Int32 GetHashCode()
        {
            Int32 hashCode = 0;
            for (Int32 index = 0; index < this.cellCommands.Count; index++)
            {
                hashCode ^= this.cellCommands[index].GetHashCode();
            }
            for (Int32 index = 0; index < this.avatarMoveCommands.Count; index++)
            {
                hashCode ^= this.avatarMoveCommands[index].GetHashCode();
            }

            return hashCode;
        }
        public static bool operator ==(GameCommandPool l, GameCommandPool r)
        {
            if (l is null && r is null)
                return true;
            else if (l is null)
                return false;
            else
                return l.Equals(r);
        }
        public static bool operator !=(GameCommandPool l, GameCommandPool r)
        {
            return !(l == r);
        }

        #endregion Сравнение.
    }
}
