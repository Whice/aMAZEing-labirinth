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
        private List<PlayerMoveCommand> playerCommands = new List<PlayerMoveCommand>(INITIAL_CAPASITY);
        /// <summary>
        ///  Список команд для передвижения ячеек.
        /// </summary>
        private List<CellMoveCommand> cellCommands = new List<CellMoveCommand>(INITIAL_CAPASITY);

        /// <summary>
        /// Создает пул и заполняет начальным количеством команд.
        /// </summary>
        public GameCommandPool()
        {
            for (Int32 i = 0; i < INITIAL_CAPASITY; i++)
            {
                this.playerCommands.Add(new PlayerMoveCommand());
                this.cellCommands.Add(new CellMoveCommand());
            }
        }

        /// <summary>
        /// Вернуть команду в пул.
        /// </summary>
        /// <param name="command"></param>
        public void PutInPool(GameCommand command)
        {
            if (command is PlayerMoveCommand playerMoveCommand)
            {
                this.playerCommands.Add(playerMoveCommand);
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
        public PlayerMoveCommand GetPlayerMoveCommand(Int32 playerMoveToX, Int32 playerMoveToY,
            Int32 playerMoveFromX, Int32 playerMoveFromY, Int32 playerNumber)
        {
            Int32 lastListIndex = this.playerCommands.Count - 1;
            PlayerMoveCommand command = null;
            if (lastListIndex < 0)
            {
                command = new PlayerMoveCommand();
            }
            else
            {
                command = this.playerCommands[lastListIndex];
            }

            command.Init(playerMoveToX, playerMoveToY, playerMoveFromX, playerMoveFromY, playerNumber);

            return command;
        }
        /// <summary>
        ///  Получить команду для перемещения ячейки.
        /// </summary>
        /// <param name="numberLine">Номер линии.</param>
        /// <param name="side">Сторона, с которой вставляется ячейка.</param>
        /// <returns></returns>
        public CellMoveCommand GetCellMoveCommand(Int32 numberLine, FieldSide side)
        {
            Int32 lastListIndex = this.playerCommands.Count - 1;
            CellMoveCommand command = null;
            if (lastListIndex < 0)
            {
                command = new CellMoveCommand();
            }
            else
            {
                command = this.cellCommands[lastListIndex];
            }

            command.Init(numberLine, side);

            return command;
        }
    }
}
