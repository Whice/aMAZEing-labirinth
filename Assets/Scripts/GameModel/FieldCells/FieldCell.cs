﻿using System;
using System.Collections.Generic;

namespace Assets.Scripts.GameModel.FieldCells
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
        public FieldCell(List<Boolean> directions)
        {
            this.directions = directions.ToArray();
        }

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

        /// <summary>
        /// Повернуть по часовой стрелке.
        /// </summary>
        public void TurnClockwise()
        {
            Int32 end = this.directions.Length - 1;
            Boolean lastDirection = this.directions[end];
            for (Int32 i = end; i > 0; i--)
            {
                this.directions[i] = this.directions[i - 1];
            }

            this.directions[0] = lastDirection;
        }
        /// <summary>
        /// Повернуть против часовой стрелке.
        /// </summary>
        public void TurnCounterClockwise()
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
}
