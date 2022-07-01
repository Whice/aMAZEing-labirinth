using Assets.Scripts.GameModel.PlayingField;
using Assets.Scripts.GameModel.PlayingField.FieldCells;
using Assets.Scripts.GameModel.Rules;
using System;
using System.Collections.Generic;
using System.Drawing;
using Xunit;

namespace TestModel.ModelTests
{
    /// <summary>
    /// Класс для проверки ходов аватара по ячекам.
    /// </summary>
    public class UnitTestsSearchRoad
    {
        /// <summary>
        /// Класс поиска пути.
        /// </summary>
        private SearchRoadForAvatar search = new SearchRoadForAvatar();
        /// <summary>
        /// Можно ли совершить указаный ход.
        /// </summary>
        /// <param name="from">Откуда.</param>
        /// <param name="to">Куда.</param>
        /// <param name="field">Поле.</param>
        /// <returns></returns>
        private Boolean CanMakeMove(Point from, Point to, FieldCell[,] field)
        {
            HashSet<Point> poinstForMove = this.search.GetCellsForMove(from, field);

            return poinstForMove.Contains(to);
        }
        /// <summary>
        /// Создать поле из двух ячеек.
        /// </summary>
        /// <param name="leftCellType"></param>
        /// <param name="leftCellClockwiseRotateCount"></param>
        /// <param name="rightCellType"></param>
        /// <param name="rightCellClockwiseRotateCount"></param>
        /// <returns></returns>
        private FieldCell[,] CreateFieldwithTwoCell(
            CellType leftCellType, Int32 leftCellClockwiseRotateCount, 
            CellType rightCellType, Int32 rightCellClockwiseRotateCount)
        {
            FieldCell[,] field = new FieldCell[Field.FIELD_SIZE, Field.FIELD_SIZE];

            //Создание заглушек для поля.
            {
                for (Int32 i = 0; i < Field.FIELD_SIZE; i++)
                    for (Int32 j = 0; j < Field.FIELD_SIZE; j++)
                    {
                        field[i, j] = new FieldCell(CellType.corner);
                    }
            }

            field[0, 0] = new FieldCell(leftCellType);
            field[0, 0].TurnClockwise(leftCellClockwiseRotateCount);
            field[0, 1] = new FieldCell(rightCellType);
            field[0, 1].TurnClockwise(rightCellClockwiseRotateCount);

           

            return field;
        }
        /// <summary>
        /// Создать поле из двух ячеек и проверить возможность хода в обе стороны.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="leftCellType"></param>
        /// <param name="leftCellClockwiseRotateCount"></param>
        /// <param name="rightCellType"></param>
        /// <param name="rightCellClockwiseRotateCount"></param>
        /// <returns></returns>
        private Boolean CreateFieldwithTwoCellAndCanMakeMoveBothWays(
            CellType leftCellType, Int32 leftCellClockwiseRotateCount,
            CellType rightCellType, Int32 rightCellClockwiseRotateCount)
        {
            Point pointLeft = new Point(0, 0);
            Point pointRight = new Point(0, 1);
            FieldCell[,] field = CreateFieldwithTwoCell(leftCellType, leftCellClockwiseRotateCount, rightCellType, rightCellClockwiseRotateCount);

            Boolean leftToRight = CanMakeMove(pointLeft, pointRight, field);
            Boolean rightToLeft = CanMakeMove(pointRight, pointLeft, field);

            return leftToRight && rightToLeft;
        }


        /// <summary>
        /// Проверить возможность ходов между двумя ячейками.
        /// </summary>
        [Fact]
        public void TestMoving_Moving_SuccessfullMoving()
        {
            //Два уголка
            {
                for (Int32 i = 0; i < 4; i++)
                    Assert.False(CreateFieldwithTwoCellAndCanMakeMoveBothWays(CellType.corner, i, CellType.corner, 0));
                for (Int32 i = 0; i < 4; i++)
                    Assert.False(CreateFieldwithTwoCellAndCanMakeMoveBothWays(CellType.corner, i, CellType.corner, 1));

                Assert.True(CreateFieldwithTwoCellAndCanMakeMoveBothWays(CellType.corner, 0, CellType.corner, 2));
                Assert.True(CreateFieldwithTwoCellAndCanMakeMoveBothWays(CellType.corner, 1, CellType.corner, 2));
                Assert.False(CreateFieldwithTwoCellAndCanMakeMoveBothWays(CellType.corner, 2, CellType.corner, 2));
                Assert.False(CreateFieldwithTwoCellAndCanMakeMoveBothWays(CellType.corner, 3, CellType.corner, 2));

                Assert.True(CreateFieldwithTwoCellAndCanMakeMoveBothWays(CellType.corner, 0, CellType.corner, 3));
                Assert.True(CreateFieldwithTwoCellAndCanMakeMoveBothWays(CellType.corner, 1, CellType.corner, 3));
                Assert.False(CreateFieldwithTwoCellAndCanMakeMoveBothWays(CellType.corner, 2, CellType.corner, 3));
                Assert.False(CreateFieldwithTwoCellAndCanMakeMoveBothWays(CellType.corner, 3, CellType.corner, 3));
            }
            //уголок и линия
            {
                for (Int32 i = 0; i < 4; i++)
                    Assert.False(CreateFieldwithTwoCellAndCanMakeMoveBothWays(CellType.corner, i, CellType.line, 0));
                for (Int32 i = 0; i < 4; i++)
                    Assert.False(CreateFieldwithTwoCellAndCanMakeMoveBothWays(CellType.corner, i, CellType.line, 2));

                Assert.True(CreateFieldwithTwoCellAndCanMakeMoveBothWays(CellType.corner, 0, CellType.line, 1));
                Assert.True(CreateFieldwithTwoCellAndCanMakeMoveBothWays(CellType.corner, 1, CellType.line, 1));
                Assert.False(CreateFieldwithTwoCellAndCanMakeMoveBothWays(CellType.corner, 2, CellType.line, 1));
                Assert.False(CreateFieldwithTwoCellAndCanMakeMoveBothWays(CellType.corner, 3, CellType.line, 1));

                Assert.True(CreateFieldwithTwoCellAndCanMakeMoveBothWays(CellType.corner, 0, CellType.line, 3));
                Assert.True(CreateFieldwithTwoCellAndCanMakeMoveBothWays(CellType.corner, 1, CellType.line, 3));
                Assert.False(CreateFieldwithTwoCellAndCanMakeMoveBothWays(CellType.corner, 2, CellType.line, 3));
                Assert.False(CreateFieldwithTwoCellAndCanMakeMoveBothWays(CellType.corner, 3, CellType.line, 3));
            }
            //две линии
            {
                
                for (Int32 i = 0; i < 4; i++)
                for (Int32 j = 0; j < 4; j++)
                    {
                        if (i % 2 != 0 && j % 2 != 0)
                        {
                            Assert.True(CreateFieldwithTwoCellAndCanMakeMoveBothWays(CellType.line, i, CellType.line, j));
                        }
                        else
                        {
                            Assert.False(CreateFieldwithTwoCellAndCanMakeMoveBothWays(CellType.line, i, CellType.line, j));
                        }
                    }
            }
            //три пути и линия
            {
                Assert.False(CreateFieldwithTwoCellAndCanMakeMoveBothWays(CellType.threeDirection, 0, CellType.line, 0));
                Assert.False(CreateFieldwithTwoCellAndCanMakeMoveBothWays(CellType.threeDirection, 1, CellType.line, 0));
                Assert.False(CreateFieldwithTwoCellAndCanMakeMoveBothWays(CellType.threeDirection, 2, CellType.line, 0));
                Assert.False(CreateFieldwithTwoCellAndCanMakeMoveBothWays(CellType.threeDirection, 3, CellType.line, 0));


                Assert.True(CreateFieldwithTwoCellAndCanMakeMoveBothWays(CellType.threeDirection, 0, CellType.line, 1));
                Assert.True(CreateFieldwithTwoCellAndCanMakeMoveBothWays(CellType.threeDirection, 1, CellType.line, 1));
                Assert.False(CreateFieldwithTwoCellAndCanMakeMoveBothWays(CellType.threeDirection, 2, CellType.line, 1));
                Assert.True(CreateFieldwithTwoCellAndCanMakeMoveBothWays(CellType.threeDirection, 3, CellType.line, 1));


                Assert.False(CreateFieldwithTwoCellAndCanMakeMoveBothWays(CellType.threeDirection, 0, CellType.line, 2));
                Assert.False(CreateFieldwithTwoCellAndCanMakeMoveBothWays(CellType.threeDirection, 1, CellType.line, 2));
                Assert.False(CreateFieldwithTwoCellAndCanMakeMoveBothWays(CellType.threeDirection, 2, CellType.line, 2));
                Assert.False(CreateFieldwithTwoCellAndCanMakeMoveBothWays(CellType.threeDirection, 3, CellType.line, 2));


                Assert.True(CreateFieldwithTwoCellAndCanMakeMoveBothWays(CellType.threeDirection, 0, CellType.line, 3));
                Assert.True(CreateFieldwithTwoCellAndCanMakeMoveBothWays(CellType.threeDirection, 1, CellType.line, 3));
                Assert.False(CreateFieldwithTwoCellAndCanMakeMoveBothWays(CellType.threeDirection, 2, CellType.line, 3));
                Assert.True(CreateFieldwithTwoCellAndCanMakeMoveBothWays(CellType.threeDirection, 3, CellType.line, 3));
            }
            //три пути и уголок
            {
                Assert.False(CreateFieldwithTwoCellAndCanMakeMoveBothWays(CellType.threeDirection, 0, CellType.corner, 0));
                Assert.False(CreateFieldwithTwoCellAndCanMakeMoveBothWays(CellType.threeDirection, 1, CellType.corner, 0));
                Assert.False(CreateFieldwithTwoCellAndCanMakeMoveBothWays(CellType.threeDirection, 2, CellType.corner, 0));
                Assert.False(CreateFieldwithTwoCellAndCanMakeMoveBothWays(CellType.threeDirection, 3, CellType.corner, 0));


                Assert.False(CreateFieldwithTwoCellAndCanMakeMoveBothWays(CellType.threeDirection, 0, CellType.corner, 1));
                Assert.False(CreateFieldwithTwoCellAndCanMakeMoveBothWays(CellType.threeDirection, 1, CellType.corner, 1));
                Assert.False(CreateFieldwithTwoCellAndCanMakeMoveBothWays(CellType.threeDirection, 2, CellType.corner, 1));
                Assert.False(CreateFieldwithTwoCellAndCanMakeMoveBothWays(CellType.threeDirection, 3, CellType.corner, 1));


                Assert.True(CreateFieldwithTwoCellAndCanMakeMoveBothWays(CellType.threeDirection, 0, CellType.corner, 2));
                Assert.True(CreateFieldwithTwoCellAndCanMakeMoveBothWays(CellType.threeDirection, 1, CellType.corner, 2));
                Assert.False(CreateFieldwithTwoCellAndCanMakeMoveBothWays(CellType.threeDirection, 2, CellType.corner, 2));
                Assert.True(CreateFieldwithTwoCellAndCanMakeMoveBothWays(CellType.threeDirection, 3, CellType.corner, 2));


                Assert.True(CreateFieldwithTwoCellAndCanMakeMoveBothWays(CellType.threeDirection, 0, CellType.corner, 3));
                Assert.True(CreateFieldwithTwoCellAndCanMakeMoveBothWays(CellType.threeDirection, 1, CellType.corner, 3));
                Assert.False(CreateFieldwithTwoCellAndCanMakeMoveBothWays(CellType.threeDirection, 2, CellType.corner, 3));
                Assert.True(CreateFieldwithTwoCellAndCanMakeMoveBothWays(CellType.threeDirection, 3, CellType.corner, 3));
            }
            //три пути и три пути
            {
                Assert.False(CreateFieldwithTwoCellAndCanMakeMoveBothWays(CellType.threeDirection, 0, CellType.threeDirection, 0));
                Assert.False(CreateFieldwithTwoCellAndCanMakeMoveBothWays(CellType.threeDirection, 1, CellType.threeDirection, 0));
                Assert.False(CreateFieldwithTwoCellAndCanMakeMoveBothWays(CellType.threeDirection, 2, CellType.threeDirection, 0));
                Assert.False(CreateFieldwithTwoCellAndCanMakeMoveBothWays(CellType.threeDirection, 3, CellType.threeDirection, 0));


                Assert.True(CreateFieldwithTwoCellAndCanMakeMoveBothWays(CellType.threeDirection, 0, CellType.threeDirection, 1));
                Assert.True(CreateFieldwithTwoCellAndCanMakeMoveBothWays(CellType.threeDirection, 1, CellType.threeDirection, 1));
                Assert.False(CreateFieldwithTwoCellAndCanMakeMoveBothWays(CellType.threeDirection, 2, CellType.threeDirection, 1));
                Assert.True(CreateFieldwithTwoCellAndCanMakeMoveBothWays(CellType.threeDirection, 3, CellType.threeDirection, 1));


                Assert.True(CreateFieldwithTwoCellAndCanMakeMoveBothWays(CellType.threeDirection, 0, CellType.threeDirection, 2));
                Assert.True(CreateFieldwithTwoCellAndCanMakeMoveBothWays(CellType.threeDirection, 1, CellType.threeDirection, 2));
                Assert.False(CreateFieldwithTwoCellAndCanMakeMoveBothWays(CellType.threeDirection, 2, CellType.threeDirection, 2));
                Assert.True(CreateFieldwithTwoCellAndCanMakeMoveBothWays(CellType.threeDirection, 3, CellType.threeDirection, 2));


                Assert.True(CreateFieldwithTwoCellAndCanMakeMoveBothWays(CellType.threeDirection, 0, CellType.threeDirection, 3));
                Assert.True(CreateFieldwithTwoCellAndCanMakeMoveBothWays(CellType.threeDirection, 1, CellType.threeDirection, 3));
                Assert.False(CreateFieldwithTwoCellAndCanMakeMoveBothWays(CellType.threeDirection, 2, CellType.threeDirection, 3));
                Assert.True(CreateFieldwithTwoCellAndCanMakeMoveBothWays(CellType.threeDirection, 3, CellType.threeDirection, 3));
            }
        }
    }
}
