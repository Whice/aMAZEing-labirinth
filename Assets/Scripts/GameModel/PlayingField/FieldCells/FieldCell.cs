using Assets.Scripts.GameModel.PlayingField.Treasures;
using System;

namespace Assets.Scripts.GameModel.PlayingField.FieldCells
{
    /// <summary>
    /// Ячейка игрового поля.
    /// <br/>Положения клеток по умолчанию:
    /// <br/>У уголка пути: верх и право.
    /// <br/>У линии пути: верх и низ.
    /// <br/>У клетки с тремя направлениями пути: верх, право и низ.
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
            LineRotationContract();
        }
        /// <summary>
        /// Максимальное количество эффективных поворотов, 
        /// т.к. превышая это значение мы начинаем идти "по кругу".
        /// </summary>
        private const Int32 MAX_TURNS_COUNT = 4;
        /// <summary>
        /// Установить количество поворотов ячейки на указанное.
        /// <br/>Число повротов должно быть положительным.
        /// </summary>
        /// <param name="turnsClockwiseCount">Количество поворотов по часовой стрелке.</param>
        public void SetClockwise(Int32 turnsClockwiseCount)
        {
            //Если взаимодействие с ячейкой не разрешено, то ничего не делать.
            if (!this.isInteractable) return;
            //Незачем вертеть, если поворот тот же.
            if (this.turnsClockwiseCount == turnsClockwiseCount) return;

            if (turnsClockwiseCount < 0)
                throw new Exception("An attempt to set the " + nameof(FieldCell) +
                    " to a negative turn in " + nameof(SetClockwise));

            turnsClockwiseCount = turnsClockwiseCount % 4;

            Int32 additionalTurnClockwiseCount = 0;

            if (this.turnsClockwiseCount > turnsClockwiseCount)
            {
                additionalTurnClockwiseCount = MAX_TURNS_COUNT - (this.turnsClockwiseCount - turnsClockwiseCount);
            }
            else if (this.turnsClockwiseCount < turnsClockwiseCount)
            {
                additionalTurnClockwiseCount = turnsClockwiseCount - this.turnsClockwiseCount;
            }

            if (additionalTurnClockwiseCount < 0)
                throw new Exception("An attempt to set the " + nameof(FieldCell) +
                    " to a negative turn in " + nameof(SetClockwise));

            TurnClockwise(additionalTurnClockwiseCount);
            this.OnTurnedClockwise?.Invoke(additionalTurnClockwiseCount);

            this.turnsClockwiseCountPrivate = turnsClockwiseCount;
            LineRotationContract();
        }
        private void LineRotationContract()
        {
            if (this.CellType == CellType.line)
            {
                //Изначально узнать положение ячейки: проходы вертикальные илди горизонтальные.
                bool directionMatching = turnsClockwiseCount % 2 == 0;

                if (directionMatching)
                {
                    directionMatching &= this.IsHaveDirectionUp && this.IsHaveDirectionDown;
                    directionMatching &= !(this.IsHaveDirectionLeft && this.IsHaveDirectionRight);
                }
                else
                {
                    directionMatching = !directionMatching;
                    directionMatching &= !(this.IsHaveDirectionUp && this.IsHaveDirectionDown);
                    directionMatching &= this.IsHaveDirectionLeft && this.IsHaveDirectionRight;
                }
                if (!directionMatching)
                {
                    throw new Exception("Какая-то хрень!");
                }
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
            LineRotationContract();
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
            hashCode ^= this.isInteractable ? 1 : 0;
            hashCode ^= this.IsHaveDirectionUp ? 1 : 0;
            hashCode ^= this.IsHaveDirectionRight ? 1 : 0;
            hashCode ^= this.IsHaveDirectionDown ? 1 : 0;
            hashCode ^= this.IsHaveDirectionLeft ? 1 : 0;
            hashCode ^= this.directionCount;
            hashCode ^= (Int32)this.CellType;
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
        /// <summary>
        /// Создание глубокого клона ячейки.
        /// </summary>
        /// <returns></returns>
        public FieldCell Clone()
        {
            FieldCell cell = new FieldCell(this.CellType, this.treasureOrStartPoints);
            cell.directions = this.CopyDirections();
            cell.isInteractable = this.isInteractable;
            cell.turnsClockwiseCountPrivate = this.turnsClockwiseCountPrivate;
            return cell;
        }

        #endregion Клонирование.

        public override string ToString()
        {
            return "Field cell rotate count: " + this.turnsClockwiseCount.ToString()
                + "; type: " + this.CellType.ToString();
        }
    }
}
