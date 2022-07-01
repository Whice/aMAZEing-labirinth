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
        /// Начинать с первой команды в списке хранилища.
        /// <br/>Первой считается команда, которая была добавлена раньше других, т.е. в самом начале игры.
        /// </summary>
        private Boolean isStartWithFirstCommandPrivate = false;
        /// <summary>
        /// Начинать выдавать команды в методе <see cref="Pop"/> с первой в списке хранилища.
        /// <br/>Первой считается команда, которая была добавлена раньше других, т.е. в самом начале игры.
        /// <br/>Метод <see cref="Add(GameCommand)"/> не будет работать при значении true.
        /// </summary>
        public Boolean isStartWithFirstCommand
        {
            get
            {
                return this.isStartWithFirstCommandPrivate;
            }
            set
            {
                if (this.isStartWithFirstCommandPrivate != value)
                {
                    this.commands.Reverse();
                }
                this.isStartWithFirstCommandPrivate = value;
            }
        }

        /// <summary>
        /// Хранилище комманд.
        /// </summary>
        private List<GameCommand> commands = new List<GameCommand>();
        /// <summary>
        ///Получить клон списка команд, начиная от самой ранней и заканчивая самой поздней.
        /// </summary>
        public List<GameCommand> commandsClone
        {
            get
            {
                List<GameCommand> clone  = new List<GameCommand>(this.count);
                for (Int32 i = 0; i < this.count; i++)
                {
                    clone.Add(this.commands[i].Clone());
                }
                return clone;
            }
        }   
        /// <summary>
                 /// Количество команд в хранилище.
                 /// </summary>
        public Int32 count
        {
            get => this.commands.Count;
        }
        /// <summary>
        /// В хранилище нет команд.
        /// </summary>
        public Boolean isEmpty
        {
            get => this.count == 0;
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
        /// <br/><see cref="GameCommandKeeper.isStartWithFirstCommand"/> должна быть false, чтобы можно было добавлять новые команды.
        /// </summary>
        /// <param name="command"></param>
        public void Add(GameCommand command)
        {
            if (this.isStartWithFirstCommand == true)
            {
                throw new Exception("You cannot add new items when the list of commands is not in the right order!" +
                    $" It is required to change {this.isStartWithFirstCommand} on false.");
            }
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
        /// <summary>
        /// Получить последнюю по порядку команду передвижения ячеек.
        /// </summary>
        /// <param name="commands"></param>
        /// <returns></returns>
        public CellMoveCommand GetLastCellMoveCommand()
        {
            if (this.isStartWithFirstCommand)
            {
                for (Int32 i = 0; i< this.commands.Count; i--)
                {
                    if (this.commands[i] is CellMoveCommand cellMoveCommand)
                    {
                        return cellMoveCommand;
                    }
                }
            }
            else
            {
                for (Int32 i = this.commands.Count - 1; i > -1; i--)
                {
                    if (this.commands[i] is CellMoveCommand cellMoveCommand)
                    {
                        return cellMoveCommand;
                    }
                }
            }

            return null;
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
