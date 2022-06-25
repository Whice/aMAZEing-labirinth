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
    }
}
