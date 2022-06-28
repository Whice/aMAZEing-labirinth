using System;
using System.Collections.Generic;

namespace Assets.Scripts.GameModel.Commands.GameCommands
{
    /// <summary>
    /// Хранит команды для сохранения.
    /// </summary>
    [Serializable]
    public class GameCommandKeeper
    {
        /// <summary>
        /// Создать хранителя через информацию об игре.
        /// </summary>
        /// <param name="gameInfo"></param>
        public GameCommandKeeper(GameInfo gameInfo)
        {
            this.gameInfo = gameInfo;
        }

        /// <summary>
        /// Информация о начале игры.
        /// </summary>
        public readonly GameInfo gameInfo;

        /// <summary>
        /// Хранилище комманд.
        /// </summary>
        private List<GameCommand> commands = new List<GameCommand>();
        /// <summary>
        /// Количество команд в хранилище.
        /// </summary>
        public Int32 count
        {
            get => this.commands.Count;
        }
        /// <summary>
        /// Индекс полседней команды.
        /// </summary>
        private Int32 lastIndex
        {
            get => this.count - 1;
        }
        /// <summary>
        /// Добавить команду в хранилище.
        /// </summary>
        /// <param name="command"></param>
        public void Add(GameCommand command)
        {
            this.commands.Add(command);
        }
        /// <summary>
        /// Взять команду из хранилища.
        /// </summary>
        /// <returns></returns>
        public GameCommand Pop()
        {
            GameCommand command = this.commands[lastIndex];
            this.commands.RemoveAt(lastIndex);
            return command;
        }

        /// <summary>
        /// Выполнить глубокое клонирование хранителя и получить клон.
        /// </summary>
        /// <returns></returns>
        public GameCommandKeeper Clone()
        {
            GameCommandKeeper clone = new GameCommandKeeper(this.gameInfo.Clone());
            clone.commands = new List<GameCommand>(this.commands.Capacity);
            for (int i = 0; i < this.commands.Count; i++)
            {
                clone.commands.Add(this.commands[i].Clone());
            }

            return clone;
        }

        #region Сравнение.

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj is GameCommandKeeper gameCommandKeeper)
            {
                if (this.gameInfo != gameCommandKeeper.gameInfo)
                {
                    return false;
                }
                if (this.commands.Count != gameCommandKeeper.commands.Count)
                {
                    return false;
                }
                if (this.lastIndex != gameCommandKeeper.lastIndex)
                {
                    return false;
                }

                for (Int32 index = 0; index < this.commands.Count; index++)
                {
                    if (this.commands[index] != gameCommandKeeper.commands[index])
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
            Int32 hashCode = this.gameInfo.GetHashCode();
            hashCode ^= this.lastIndex.GetHashCode();
            for (Int32 index = 0; index < this.commands.Count; index++)
            {
                hashCode ^= this.commands[index].GetHashCode();
            }

            return hashCode;
        }
        public static bool operator ==(GameCommandKeeper l, GameCommandKeeper r)
        {
            if (l is null && r is null)
                return true;
            else if (l is null)
                return false;
            else
                return l.Equals(r);
        }
        public static bool operator !=(GameCommandKeeper l, GameCommandKeeper r)
        {
            return !(l == r);
        }

        #endregion Сравнение.
    }
}
