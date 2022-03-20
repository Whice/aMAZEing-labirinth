using System;
using System.Collections.Generic;

namespace Assets.Scripts.GameModel.PlayingField.FieldCells
{
    /// <summary>
    /// Ячейка игрового поля.
    /// </summary>
    public abstract class FieldCell
    {
        /// <summary>
        /// Проходы.
        /// <br/> 0 - верхний.
        /// <br/> 1 - правый.
        /// <br/> 2 - нижний.
        /// <br/> 3 - левый.
        /// </summary>
        private Boolean[] directions = null;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="directions">Проходы.</param>
        /// <param name="CellType">Тип ячейки.
        /// Хранит информацию о том, сколько проходов у нее и как они расположены.</param>
        public FieldCell(List<Boolean> directions, CellType CellType)
        {
            this.directions = directions.ToArray();
            this.CellType = CellType;
        }

        #region Данные ячейки.

        /// <summary>
        /// Разрешено взаимодействие с этой ячейкой.
        /// </summary>
        public Boolean isInteractable { get; set; } = true;
        /// <summary>
        /// Тип ячейки.
        /// Хранит информацию о том, сколько проходов у нее и как они расположены.
        /// </summary>
        public readonly CellType CellType = CellType.unknown;
        /// <summary>
        /// Имеется ли направление вверх у этой клетки.
        /// </summary>
        public Boolean IsHaveDirectionUp
        {
            get => this.directions[0];
        }
        /// <summary>
        /// Имеется ли направление вправо у этой клетки.
        /// </summary>
        public Boolean IsHaveDirectionRight
        {
            get => this.directions[1];
        }
        /// <summary>
        /// Имеется ли направление вниз у этой клетки.
        /// </summary>
        public Boolean IsHaveDirectionDown
        {
            get => this.directions[2];
        } 
        /// <summary>
        /// Имеется ли направление влево у этой клетки.
        /// </summary>
        public Boolean IsHaveDirectionLeft
        {
            get => this.directions[3];
        }
        /// <summary>
        /// Количество направлений у этой ячейки.
        /// </summary>
        public Int32 directionCount
        {
            get
            {
                Int32 count = 0;
                foreach (Boolean direction in this.directions)
                {
                    if(direction)
                    {
                        count++;
                    }
                }
                return count;
            }
        }

        #endregion Данные ячейки.

        #region Действия с ячейкой.

        /// <summary>
        /// Повернуть по часовой стрелке.
        /// </summary>
        /// <param name="count">Количество поворотов.</param>
        public void TurnClockwise(Int32 count=1)
        {
            //Если взаимодействие с ячейкой не разрешено, то ничего не делать.
            if (!this.isInteractable)
                return;

            for (Int32 numberTurn = 0; numberTurn < count; numberTurn++)
            {
                Int32 end = this.directions.Length - 1;
                Boolean lastDirection = this.directions[end];
                for (Int32 i = end; i > 0; i--)
                {
                    this.directions[i] = this.directions[i - 1];
                }

                this.directions[0] = lastDirection;
            }
        }
        /// <summary>
        /// Повернуть против часовой стрелке.
        /// </summary>
        /// <param name="count">Количество поворотов.</param>
        public void TurnCounterClockwise(Int32 count = 1)
        {
            //Если взаимодействие с ячейкой не разрешено, то ничего не делать.
            if (!this.isInteractable)
                return;

            for (Int32 numberTurn = 0; numberTurn < count; numberTurn++)
            {
                Int32 end = this.directions.Length - 1;
                Boolean firstDirection = this.directions[0];
                for (Int32 i = 0; i < end; i++)
                {
                    this.directions[i] = this.directions[i + 1];
                }

                this.directions[end] = firstDirection;
            }
        }

        #endregion Действия с ячейкой.

        #region Сравнение.

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj is FieldCell otherCell)
            {

                if (this.IsHaveDirectionUp != otherCell.IsHaveDirectionUp)
                    return false;
                if (this.IsHaveDirectionRight != otherCell.IsHaveDirectionRight)
                    return false;
                if (this.IsHaveDirectionDown != otherCell.IsHaveDirectionDown)
                    return false;
                if (this.IsHaveDirectionLeft != otherCell.IsHaveDirectionLeft)
                    return false;

                if(this.directionCount != otherCell.directionCount)
                    return false;

                if(this.CellType != otherCell.CellType)
                    return false;

                return true;
            }

            return false;
        }
        public override Int32 GetHashCode()
        {
            Int32 hashCode = 0;
            hashCode += this.isInteractable ? 1 : 0;
            hashCode += this.IsHaveDirectionUp ? 1 : 0;
            hashCode += this.IsHaveDirectionRight ? 1 : 0;
            hashCode += this.IsHaveDirectionDown ? 1 : 0;
            hashCode += this.IsHaveDirectionLeft ? 1 : 0;
            hashCode |= this.directionCount;
            hashCode |= (Int32)this.CellType;
            return hashCode;
        }
        public static bool operator ==(FieldCell l, FieldCell r)
        {
            if(l==null || r==null)
                return false;
            else
                return l.Equals(r);
        }
        public static bool operator !=(FieldCell l, FieldCell r)
        {
            return !(l == r);
        }

        #endregion Сравнение.
    }
}
