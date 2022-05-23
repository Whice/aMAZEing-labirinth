using Assets.Scripts.GameModel.PlayingField.Treasures;
using System;

namespace Assets.Scripts.GameModel.PlayingField.FieldCells
{
    /// <summary>
    /// Ячейка игрового поля.
    /// </summary>
    public class FieldCell
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
        /// Задать проходы для ячейки по ее типу.
        /// </summary>
        /// <param name="сellType">Тип ячейки.</param>
        private void CreateDirections(CellType сellType)
        {
            this.directions = new Boolean[4];

            switch (сellType)
            {
                case CellType.corner:
                    {
                        this.directions = new Boolean[] { true, true, false, false };
                        break;
                    }
                case CellType.line:
                    {
                        this.directions = new Boolean[] { true, false, true, false };
                        break;
                    }
                case CellType.threeDirection:
                    {
                        this.directions = new Boolean[] { true, true, true, false };
                        break;
                    }
                default:
                    {
                        //corner
                        this.directions = new Boolean[] { true, true, false, false };
                        break;
                    }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="directions">Проходы.</param>
        /// <param name="сellType">Тип ячейки.
        /// <param name="treasureOrStartPoints">Сокровище или стратовая точка, которая есть в этой ячейке.
        /// Хранит информацию о том, сколько проходов у нее и как они расположены.</param>
        public FieldCell(CellType сellType,
            TreasureAndStartPointsType treasureOrStartPoints = TreasureAndStartPointsType.empty)
        {
            this.CellType = сellType;
            CreateDirections(сellType);

            this.treasureOrStartPoints = treasureOrStartPoints;
        }

        #region Публичные данные ячейки.

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
        /// Сокровище или стратовая точка, которая есть в этой ячейке.
        /// </summary>
        public readonly TreasureAndStartPointsType treasureOrStartPoints;
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
                    if (direction)
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
        /// Количество поворотов по часовой стрелке.
        /// <br/>По сути определяет направление.
        /// <br/>Значение может быть с 0 по 3.
        /// </summary>
        private Int32 turnsClockwiseCountPrivate = 0;
        /// <summary>
        /// Количество поворотов по часовой стрелке.
        /// <br/>По сути определяет направление.
        /// <br/>Значение может быть с 0 по 3.
        /// </summary>
        public Int32 turnsClockwiseCount
        {
            get=>this.turnsClockwiseCountPrivate;
        }
        /// <summary>
        /// Происходит поворот по часовой стрелке.
        /// </summary>
        public event Action<Int32> OnTurnedClockwise;
        /// <summary>
        /// Повернуть по часовой стрелке.
        /// </summary>
        /// <param name="count">Количество поворотов.</param>
        public void TurnClockwise(Int32 count = 1)
        {
            //Если взаимодействие с ячейкой не разрешено, то ничего не делать.
            if (!this.isInteractable)
                return;

            count = count % 4;
            this.turnsClockwiseCountPrivate = (this.turnsClockwiseCount + count) % 4;

            this.OnTurnedClockwise?.Invoke(count);

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
        /// Происходит поворот против часовой стрелки.
        /// </summary>
        public event Action<Int32> OnTurnedCountclockwise;
        /// <summary>
        /// Повернуть против часовой стрелке.
        /// </summary>
        /// <param name="count">Количество поворотов.</param>
        public void TurnCounterClockwise(Int32 count = 1)
        {
            //Если взаимодействие с ячейкой не разрешено, то ничего не делать.
            if (!this.isInteractable)
                return;

            count = count % 4;
            this.turnsClockwiseCountPrivate = 4 - ((this.turnsClockwiseCount + count) % 4);

            this.OnTurnedCountclockwise?.Invoke(count);

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
                if (this.CellType != otherCell.CellType)
                    return false;

                if (this.directionCount != otherCell.directionCount)
                    return false;

                if (this.IsHaveDirectionUp != otherCell.IsHaveDirectionUp)
                    return false;
                if (this.IsHaveDirectionRight != otherCell.IsHaveDirectionRight)
                    return false;
                if (this.IsHaveDirectionDown != otherCell.IsHaveDirectionDown)
                    return false;
                if (this.IsHaveDirectionLeft != otherCell.IsHaveDirectionLeft)
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
            if (l is null && r is null)
                return true;
            else if (l is null)
                return false;
            else
                return l.Equals(r);
        }
        public static bool operator !=(FieldCell l, FieldCell r)
        {
            return !(l == r);
        }

        #endregion Сравнение.

        #region Клонирование.

        /// <summary>
        /// Создание глубокого клона ячейки.
        /// </summary>
        /// <returns></returns>
        public FieldCell Clone()
        {
            FieldCell cell = new FieldCell(this.CellType, this.treasureOrStartPoints);
            cell.directions = this.CopyDirections();
            cell.isInteractable = this.isInteractable;
            return cell;
        }
        /// <summary>
        /// Копировать данные массива направлений.
        /// </summary>
        /// <returns></returns>
        protected Boolean[] CopyDirections()
        {
            Boolean[] directionsClone = new Boolean[this.directions.Length];
            for (Int32 i = 0; i < this.directions.Length; i++)
            {
                directionsClone[i] = this.directions[i];
            }
            return directionsClone;
        }

        #endregion Клонирование.

        public override string ToString()
        {
            return "Field cell rotate count: " + this.turnsClockwiseCount.ToString()
                + "; type: " + this.CellType.ToString();
        }
    }
}
