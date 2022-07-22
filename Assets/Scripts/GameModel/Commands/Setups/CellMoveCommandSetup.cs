using Assets.Scripts.GameModel.PlayingField;
using System;

namespace Assets.Scripts.GameModel.Commands
{
    /// <summary>
    /// Структура передачи информации в команду передвижения ячейки.
    /// </summary>
    public struct CellMoveCommandSetup
    {
        /// <summary>
        /// Номер линии.
        /// </summary>
        public int numberLine;
        /// <summary>
        /// Сторона, с которой вставляется ячейка.
        /// </summary>
        public FieldSide side;
        /// <summary>
        /// Количество поворотов у свободной ячейки по часовой стрелке до совершения хода.
        /// </summary>
        public int turnsClockwiseCountFreeCellBefore;
        /// <summary>
        /// Количество поворотов у свободной ячейки по часовой стрелке после совершения хода.
        /// </summary>
        public int turnsClockwiseCountFreeCellAfter;

        /// <summary>
        /// Инициализировать команду.
        /// </summary>
        /// <param name="numberLine">Номер линии.</param>
        /// <param name="side">Сторона, с которой вставляется ячейка.</param>
        /// <param name="turnsClockwiseCountFreeCellBefore">Количество поворотов у свободной ячейки по часовой стрелке до совершения хода.</param>
        /// <param name="turnsClockwiseCountFreeCellAfter">Количество поворотов у свободной ячейки по часовой стрелке после совершения хода.</param>
        public CellMoveCommandSetup(Int32 numberLine, FieldSide side, Int32 turnsClockwiseCountFreeCellBefore, Int32 turnsClockwiseCountFreeCellAfter)
        {
            this.numberLine = numberLine;
            this.side = side;
            this.turnsClockwiseCountFreeCellBefore = turnsClockwiseCountFreeCellBefore;
            this.turnsClockwiseCountFreeCellAfter = turnsClockwiseCountFreeCellAfter;
        }
    }
}
