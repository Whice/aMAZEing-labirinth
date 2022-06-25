using Assets.Scripts.Extensions;
using Assets.Scripts.GameModel.PlayingField;
using System;

namespace Assets.Scripts.GameModel.Commands.GameCommands
{
    /// <summary>
    /// Комманда сдвига линии ячеек свободной ячейкой.
    /// </summary>
    public class CellMoveCommand : GameCommand
    {
        /// <summary>
        /// Откуда игрок должен пойти. Содержит обе координаты, т.к. они занимают меньше 4х битов.
        /// </summary>
        private TwoBytesInOneKeeper numberLineAndSide;
        /// <summary>
        /// Откуда игрок должен пойти по горизонтали.
        /// </summary>
        private Int32 numberLine
        {
            get => this.numberLineAndSide.firstValue;
            set => this.numberLineAndSide.firstValue = (byte)value;
        }
        /// <summary>
        /// Откуда игрок должен пойти по вертикали.
        /// </summary>
        private FieldSide side
        {
            get
            {
                return (FieldSide)(this.numberLineAndSide.secondValue - 2);
            }
            set
            {
                byte numberSide = (byte)value;
                numberSide += 2;//Т.к. минимум -2, то надо немного сдвинуть, чтобы число было положительным.

                this.numberLineAndSide.secondValue = numberSide;
            }
        }

        /// <summary>
        /// Инициализировать команду.
        /// </summary>
        /// <param name="numberLine">Номер линии.</param>
        /// <param name="side">Сторона, с которой вставляется ячейка.</param>
        public void Init(Int32 numberLine, FieldSide side)
        {
            this.numberLine = numberLine;
            this.side = side;
        }

        public override void Execute(Game modelGame)
        {
            base.Execute(modelGame);

            modelGame.SetFreeCellToField(this.numberLine, this.side);
        }
        public override void Undo(Game modelGame)
        {
            base.Undo(modelGame);

            //Умножение на -1, т.к. противположные стороны имеют противоположный знак.
            FieldSide opositeSide = (FieldSide)((Int32)this.side * -1);
            modelGame.SetFreeCellToFieldWithAllowedMovesCancellation(this.numberLine, opositeSide);
        }
    }
}
