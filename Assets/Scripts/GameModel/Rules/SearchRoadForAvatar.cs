using Assets.Scripts.GameModel.PlayingField;
using Assets.Scripts.GameModel.PlayingField.FieldCells;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Assets.Scripts.GameModel.Rules
{
    /// <summary>
    /// Класс для поиска пути, куда может ходить игрок.
    /// </summary>
    public class SearchRoadForAvatar
    {
        /// <summary>
        /// Координаты ячеек, куда можно ходить.
        /// </summary>
        private HashSet<Point> cellsForMove = new HashSet<Point>();
        /// <summary>
        /// Ссылка поле с ячейками.
        /// </summary>
        private FieldCell[,] field;

        /// <summary>
        /// Координаты находятся на поле.
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        private Boolean IsCellInField(Int32 i, Int32 j)
        {
            if (i < 0)
                return false;
            else if (j < 0  )
                return false;
            else if (i >= Field.FIELD_SIZE)
                return false;
            else if(j >= Field.FIELD_SIZE)
                return false;

            return true;
        }
        /// <summary>
        /// Есть пути между ячейками.
        /// </summary>
        /// <param name="firstCellPosition"></param>
        /// <param name="secondCellPosition"></param>
        /// <returns></returns>
        private Boolean IsConnectionBetweenCells(Point firstCellPosition, Point secondCellPosition)
        {
            //X - направлен вниз
            //Y - направлен вправо
            //i - направлен вниз
            //j - направлен вправо
            FieldCell firstCell = this.field[firstCellPosition.X, firstCellPosition.Y];
            FieldCell secondCell = this.field[secondCellPosition.X, secondCellPosition.Y];

            if (firstCellPosition.X == secondCellPosition.X)
            {
                if (firstCellPosition.Y > secondCellPosition.Y)
                {
                    return firstCell.IsHaveDirectionLeft && secondCell.IsHaveDirectionRight;
                }

                if (firstCellPosition.Y < secondCellPosition.Y)
                {
                    return firstCell.IsHaveDirectionRight && secondCell.IsHaveDirectionLeft;
                }
            }
            else if (firstCellPosition.Y == secondCellPosition.Y)
            {
                if (firstCellPosition.X > secondCellPosition.X)
                {
                    return firstCell.IsHaveDirectionUp && secondCell.IsHaveDirectionDown;
                }

                if (firstCellPosition.X < secondCellPosition.X)
                {
                    return firstCell.IsHaveDirectionDown && secondCell.IsHaveDirectionUp;
                }
            }

            return false;
        }
        /// <summary>
        /// Найти путь между соседними ячейками.
        /// </summary>
        /// <param name="xPositionCell"></param>
        /// <param name="yPositionCell"></param>
        private void SearchForPassageInNeighboringCells(Int32 xPositionCell, Int32 yPositionCell)
        {
            Point thisCellPosition = new Point(xPositionCell, yPositionCell);

            Point otherCellPosition = new Point(xPositionCell + 1, yPositionCell);
            if (!this.cellsForMove.Contains(otherCellPosition))
            {
                if (IsCellInField(otherCellPosition.X, otherCellPosition.Y))
                {
                    if (IsConnectionBetweenCells(thisCellPosition, otherCellPosition))
                    {
                        this.cellsForMove.Add(otherCellPosition);
                        SearchForPassageInNeighboringCells(otherCellPosition.X, otherCellPosition.Y);
                    }
                }
            }

            otherCellPosition = new Point(xPositionCell - 1, yPositionCell);
            if (!this.cellsForMove.Contains(otherCellPosition))
            {
                if (IsCellInField(otherCellPosition.X, otherCellPosition.Y))
                {
                    if (IsConnectionBetweenCells(thisCellPosition, otherCellPosition))
                    {
                        this.cellsForMove.Add(otherCellPosition);
                        SearchForPassageInNeighboringCells(otherCellPosition.X, otherCellPosition.Y);
                    }
                }
            }

            otherCellPosition = new Point(xPositionCell, yPositionCell + 1);
            if (!this.cellsForMove.Contains(otherCellPosition))
            {
                if (IsCellInField(otherCellPosition.X, otherCellPosition.Y))
                {
                    if (IsConnectionBetweenCells(thisCellPosition, otherCellPosition))
                    {
                        this.cellsForMove.Add(otherCellPosition);
                        SearchForPassageInNeighboringCells(otherCellPosition.X, otherCellPosition.Y);
                    }
                }
            }

            otherCellPosition = new Point(xPositionCell, yPositionCell - 1);
            if (!this.cellsForMove.Contains(otherCellPosition))
            {
                if (IsCellInField(otherCellPosition.X, otherCellPosition.Y))
                {
                    if (IsConnectionBetweenCells(thisCellPosition, otherCellPosition))
                    {
                        this.cellsForMove.Add(otherCellPosition);
                        SearchForPassageInNeighboringCells(otherCellPosition.X, otherCellPosition.Y);
                    }
                }
            }
        }
        /// <summary>
        /// Получить координаты ячеек, на которые можно ходить.
        /// </summary>
        /// <param name="playerPosition">Позиция игрока, для которого выполнить подсчет.</param>
        /// <param name="field">Ссылка на игровое поле.</param>
        /// <returns></returns>
        public HashSet<Point> GetCellsForMove(Point playerPosition, FieldCell[,] field)
        {
            this.cellsForMove.Clear();

            this.field = field;
            this.cellsForMove.Add(playerPosition);

            SearchForPassageInNeighboringCells(playerPosition.X, playerPosition.Y);

            return new HashSet<Point>(this.cellsForMove);
        }
    }
}
