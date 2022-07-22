using Assets.Scripts.Extensions;
using Assets.Scripts.GameModel.PlayingField;
using System;

namespace Assets.Scripts.GameModel.Commands
{
    /// <summary>
    /// Комманда сдвига линии ячеек свободной ячейкой.
    /// </summary>

    [Serializable]
    public class CellMoveCommand : GameCommand
    {
        /// <summary>
        /// Откуда игрок должен пойти. Содержит обе координаты, т.к. они занимают меньше 4х битов.
        /// </summary>
        private TwoBytesInOneKeeper numberLineAndSide;
        /// <summary>
        /// Линия, куда вставлялась свободная ячейка.
        /// </summary>
        public Int32 numberLine
        {
            get => this.numberLineAndSide.firstValue;
            private set => this.numberLineAndSide.firstValue = (byte)value;
        }
        /// <summary>
        /// Сторона, куда вставлялась свободная ячейка.
        /// </summary>
        public FieldSide side
        {
            get
            {
                return (FieldSide)(this.numberLineAndSide.secondValue - 2);
            }
            private set
            {
                byte numberSide = (byte)value;
                numberSide += 2;//Т.к. минимум -2, то надо немного сдвинуть, чтобы число было положительным.

                this.numberLineAndSide.secondValue = numberSide;
            }
        }
        /// <summary>
        /// Количество поворотов у свободной ячейки по часовой стрелке до и после хода.
        /// <br/>По сути определяет направление.
        /// <br/>Значение может быть с 0 по 3.
        /// </summary>
        private TwoBytesInOneKeeper turnsClockwiseCountBeforeAndAfter;
        /// <summary>
        /// Количество поворотов у свободной ячейки по часовой стрелке до совершения хода.
        /// <br/>По сути определяет направление.
        /// <br/>Значение может быть с 0 по 3.
        /// </summary>
        public Int32 turnsClockwiseCountFreeCellBefore
        {
            get => this.turnsClockwiseCountBeforeAndAfter.firstValue;
            private set => this.turnsClockwiseCountBeforeAndAfter.firstValue = (byte)value;
        }
        /// <summary>
        /// Количество поворотов у свободной ячейки по часовой стрелке после совершения хода.
        /// <br/>По сути определяет направление.
        /// <br/>Значение может быть с 0 по 3.
        /// </summary>
        public Int32 turnsClockwiseCountFreeCellAfter
        {
            get => this.turnsClockwiseCountBeforeAndAfter.secondValue;
            private set => this.turnsClockwiseCountBeforeAndAfter.secondValue = (byte)value;
        }

        /// <summary>
        /// Инициализировать команду.
        /// </summary>
        public void Init(in CellMoveCommandSetup setup)
        {
            this.numberLine = setup.numberLine;
            this.side = setup.side;
            this.turnsClockwiseCountFreeCellBefore = setup.turnsClockwiseCountFreeCellBefore;
            this.turnsClockwiseCountFreeCellAfter = setup.turnsClockwiseCountFreeCellAfter;
        }

        public override bool Execute(Game modelGame)
        {
            Boolean result = base.Execute(modelGame);

            modelGame.freeCell.SetClockwise(this.turnsClockwiseCountFreeCellBefore);
            result &= modelGame.SetFreeCellToField(this.numberLine, this.side);
            modelGame.freeCell.SetClockwise(this.turnsClockwiseCountFreeCellAfter);

            return result;
        }
        public override bool Undo(Game modelGame)
        {
            Boolean result = base.Undo(modelGame);

            //Умножение на -1, т.к. противположные стороны имеют противоположный знак.
            FieldSide opositeSide = (FieldSide)((Int32)this.side * -1);
            modelGame.freeCell.SetClockwise(this.turnsClockwiseCountFreeCellAfter);
            result &= modelGame.SetFreeCellToFieldWithAllowedMovesCancellation(this.numberLine, opositeSide, false);
            modelGame.freeCell.SetClockwise(this.turnsClockwiseCountFreeCellBefore);

            return result;
        }

        #region Клонирование.

        public override GameCommand Clone()
        {
            return GetCellMoveCommandClone();
        }
        /// <summary>
        /// Выполнить глубокое клонирование команды перемещения аватара и получить клон.
        /// </summary>
        /// <returns></returns>
        public CellMoveCommand GetCellMoveCommandClone()
        {
            CellMoveCommand clone = new CellMoveCommand();
            clone.numberLineAndSide = this.numberLineAndSide;
            clone.turnsClockwiseCountBeforeAndAfter = this.turnsClockwiseCountBeforeAndAfter;

            return clone;
        }

        #endregion Клонирование.

        #region Сравнение.

        public override Boolean Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj is CellMoveCommand cellMoveCommand)
            {
                if (this.numberLineAndSide != cellMoveCommand.numberLineAndSide)
                {
                    return false;
                }

                return true;
            }

            return false;
        }
        public override Int32 GetHashCode()
        {
            return this.numberLineAndSide.GetHashCode();
        }
        public static bool operator ==(CellMoveCommand l, CellMoveCommand r)
        {
            if (l is null && r is null)
                return true;
            else if (l is null)
                return false;
            else
                return l.Equals(r);
        }
        public static bool operator !=(CellMoveCommand l, CellMoveCommand r)
        {
            return !(l == r);
        }

        #endregion Сравнение.
    }
}
