using Assets.Scripts.GameModel.PlayingField;
using Assets.Scripts.GameModel.PlayingField.FieldCells;
using System;
using Xunit;

namespace TestModel.ModelTests
{
    /// <summary>
    ///  Класс тестирования игрового поля.
    /// </summary>
    public class UnitTestGameModelPlaingField
    {
        #region Дополнительные поля и методы.

#pragma warning disable CS8618
        /// <summary>
        /// Игровое поле для всех тестов создания.
        /// </summary>
        private Field field;
#pragma warning restore CS8618
        /// <summary>
        /// Создать игровое поле, если оно не создано.
        /// </summary>
        private void CreateField()
        {
            if (this.field == null)
            {
                this.field = new Field();
            }
        }

        #endregion Дополнительные поля и методы.

        #region Create

        /// <summary>
        /// Правильное заполнение закрепленных ячеек при создании.
        /// </summary>
        [Fact]
        public void TestCreate_CreateField_SuccessfullCreatePinnedCells()
        {
            CreateField();
            Field field = this.field;

            #region  углы

            FieldCell currentCell = field.fieldCells[0, 0];
            Assert.Equal(CellType.corner, currentCell.CellType);
            Assert.True(currentCell.IsHaveDirectionRight);
            Assert.True(currentCell.IsHaveDirectionDown);
            currentCell = field.fieldCells[0, 6];
            Assert.Equal(CellType.corner, currentCell.CellType);
            Assert.True(currentCell.IsHaveDirectionDown);
            Assert.True(currentCell.IsHaveDirectionLeft);
            currentCell = field.fieldCells[6, 0];
            Assert.Equal(CellType.corner, currentCell.CellType);
            Assert.True(currentCell.IsHaveDirectionUp);
            Assert.True(currentCell.IsHaveDirectionRight);
            currentCell = field.fieldCells[6, 6];
            Assert.Equal(CellType.corner, currentCell.CellType);
            Assert.True(currentCell.IsHaveDirectionLeft);
            Assert.True(currentCell.IsHaveDirectionUp);

            #endregion  углы

            #region центр

            currentCell = field.fieldCells[2, 2];
            Assert.Equal(CellType.threeDirection, currentCell.CellType);
            Assert.True(currentCell.IsHaveDirectionUp);
            Assert.True(currentCell.IsHaveDirectionRight);
            Assert.True(currentCell.IsHaveDirectionDown);
            currentCell = field.fieldCells[2, 4];
            Assert.Equal(CellType.threeDirection, currentCell.CellType);
            Assert.True(currentCell.IsHaveDirectionRight);
            Assert.True(currentCell.IsHaveDirectionDown);
            Assert.True(currentCell.IsHaveDirectionLeft);
            currentCell = field.fieldCells[4, 4];
            Assert.Equal(CellType.threeDirection, currentCell.CellType);
            Assert.True(currentCell.IsHaveDirectionDown);
            Assert.True(currentCell.IsHaveDirectionLeft);
            Assert.True(currentCell.IsHaveDirectionUp);
            currentCell = field.fieldCells[4, 2];
            Assert.Equal(CellType.threeDirection, currentCell.CellType);
            Assert.True(currentCell.IsHaveDirectionLeft);
            Assert.True(currentCell.IsHaveDirectionUp);
            Assert.True(currentCell.IsHaveDirectionRight);

            #endregion центр

            #region границы

            currentCell = field.fieldCells[0, 2];
            Assert.Equal(CellType.threeDirection, currentCell.CellType);
            Assert.True(currentCell.IsHaveDirectionRight);
            Assert.True(currentCell.IsHaveDirectionDown);
            Assert.True(currentCell.IsHaveDirectionLeft);
            currentCell = field.fieldCells[0, 4];
            Assert.Equal(CellType.threeDirection, currentCell.CellType);
            Assert.True(currentCell.IsHaveDirectionRight);
            Assert.True(currentCell.IsHaveDirectionDown);
            Assert.True(currentCell.IsHaveDirectionLeft);
            currentCell = field.fieldCells[2, 6];
            Assert.Equal(CellType.threeDirection, currentCell.CellType);
            Assert.True(currentCell.IsHaveDirectionDown);
            Assert.True(currentCell.IsHaveDirectionLeft);
            Assert.True(currentCell.IsHaveDirectionUp);
            currentCell = field.fieldCells[4, 6];
            Assert.Equal(CellType.threeDirection, currentCell.CellType);
            Assert.True(currentCell.IsHaveDirectionDown);
            Assert.True(currentCell.IsHaveDirectionLeft);
            Assert.True(currentCell.IsHaveDirectionUp);
            currentCell = field.fieldCells[6, 2];
            Assert.Equal(CellType.threeDirection, currentCell.CellType);
            Assert.True(currentCell.IsHaveDirectionLeft);
            Assert.True(currentCell.IsHaveDirectionUp);
            Assert.True(currentCell.IsHaveDirectionRight);
            currentCell = field.fieldCells[6, 4];
            Assert.Equal(CellType.threeDirection, currentCell.CellType);
            Assert.True(currentCell.IsHaveDirectionLeft);
            Assert.True(currentCell.IsHaveDirectionUp);
            Assert.True(currentCell.IsHaveDirectionRight);
            currentCell = field.fieldCells[2, 0];
            Assert.Equal(CellType.threeDirection, currentCell.CellType);
            Assert.True(currentCell.IsHaveDirectionUp);
            Assert.True(currentCell.IsHaveDirectionRight);
            Assert.True(currentCell.IsHaveDirectionDown);
            currentCell = field.fieldCells[4, 0];
            Assert.Equal(CellType.threeDirection, currentCell.CellType);
            Assert.True(currentCell.IsHaveDirectionUp);
            Assert.True(currentCell.IsHaveDirectionRight);
            Assert.True(currentCell.IsHaveDirectionDown);

            #endregion границы
        }
        /// <summary>
        /// Отсутсвуют пустые ячейки на поле. Они должны быть все определены.
        /// </summary>
        [Fact]
        public void TestCreate_CreateField_ThereAreNoNullValuesI​nField()
        {
            CreateField();
            Field field = this.field;

            foreach (FieldCell cell in field.fieldCells)
            {
                Assert.NotNull(cell);
            }
        }
        /// <summary>
        /// Заполнение свободной ячейки присоздании.
        /// </summary>
        [Fact]
        public void TestCreate_CreateField_SuccessfullCreateFreeCell()
        {
            CreateField();
            Field field = this.field;

            Assert.NotNull(field.freeFieldCell);
        }
        /// <summary>
        /// Правильное количество ячеек каждого вида.
        /// </summary>
        [Fact]
        public void TestCreate_CreateField_RightCountCellsInField()
        {
            CreateField();
            Field field = this.field;
            Int32 countCoreners = 0;
            Int32 countLines = 0;
            Int32 countThreeDirections = 0;
            foreach (FieldCell cell in field.fieldCells)
            {
                if (cell.CellType == CellType.threeDirection)
                    ++countThreeDirections;
                if (cell.CellType == CellType.line)
                    ++countLines;
                if (cell.CellType == CellType.corner)
                    ++countCoreners;
            }

            if (field.freeFieldCell.CellType == CellType.corner)
                ++countCoreners;
            else if (field.freeFieldCell.CellType == CellType.line)
                ++countLines;
            else if (field.freeFieldCell.CellType == CellType.threeDirection)
                ++countThreeDirections;

            Assert.Equal(20, countCoreners);
            Assert.Equal(12, countLines);
            Assert.Equal(18, countThreeDirections);
        }
        /// <summary>
        /// Правильное заполнение закрепленных ячеек при создании.
        /// </summary>
        public void TestCreate_RotateAllNotInreationCellsField_UnsuccessfullRotate()
        {
            CreateField();
            Field field = this.field;
            Field field2 = this.field.Clone();

            foreach (FieldCell cell in field.fieldCells)
                cell.TurnClockwise();

            for (Int32 i = 0; i < Field.fieldSize; i++)
                for (Int32 j = 0; j < Field.fieldSize; j++)
                {
                    Assert.True(field.fieldCells[i, j] == field2.fieldCells[i, j]);
                }
        }
        /// <summary>
        /// Правильное количество сокровищ каждого типа при создании.
        /// </summary>
        [Fact]
        public void TestCreate_CreateAllTresures_RightCountTReasuresEachType()
        {

            CreateField();
            Field field = this.field;
            Int32 treasureType;
            Int32 countStartPoints = 0;
            Int32 countPinnedTreasures = 0;
            Int32 countMovingTreasures = 0;
            foreach (FieldCell cell in field.fieldCells)
            {
                treasureType = (Int32)cell.treasureOrStartPoints;
                if (treasureType >= 2 && treasureType <= 5)
                    ++countStartPoints;
                else if (treasureType >= 6 && treasureType <= 17)
                    ++countPinnedTreasures;
                else if (treasureType >= 18 && treasureType <= 29)
                    ++countMovingTreasures;
            }

            treasureType = (Int32)field.freeFieldCell.treasureOrStartPoints;
            if (treasureType >= 18 && treasureType <= 29)
                ++countMovingTreasures;

            Assert.Equal(4, countStartPoints);
            Assert.Equal(12, countPinnedTreasures);
            Assert.Equal(12, countMovingTreasures);
        }

        #endregion Create

        #region Движение линий.

        /// <summary>
        /// Тест на удачный сдвиг вверх второй по порядку линии.
        /// </summary>
        [Fact]
        public void TestMovement_MoveLineUp_SuccessfullMovement()
        {
            CreateField();
            Field field = this.field;
            FieldCell[,] cells = field.fieldCells;

            //Предполагаемая новая свободная ячейка
            FieldCell estimatedFreeCell = cells[1, 0];
            //Предполагаемая новая линия.
            FieldCell[] estimatedNewLine = new FieldCell[Field.fieldSize];
            for (Int32 i = 0, j = i + 1; i < estimatedNewLine.Length - 1; i++, j++)
            {
                estimatedNewLine[i] = cells[1, j];
            }
            estimatedNewLine[estimatedNewLine.Length - 1] = field.freeFieldCell;

            //Подвинуть вверх вторую по порядку линию
            field.MoveLineUp(1);

            for (Int32 i = 0; i < estimatedNewLine.Length; i++)
            {
                Assert.Equal(estimatedNewLine[i], cells[1, i]);
            }

            Assert.Equal(estimatedFreeCell, field.freeFieldCell);
        }
        /// <summary>
        /// Тест на удачный сдвиг влево второй по порядку линии.
        /// </summary>
        [Fact]
        public void TestMovement_MoveLineLeft_SuccessfullMovement()
        {
            CreateField();
            Field field = this.field;
            FieldCell[,] cells = field.fieldCells;

            //Предполагаемая новая свободная ячейка
            FieldCell estimatedFreeCell = cells[0, 1];
            //Предполагаемая новая линия.
            FieldCell[] estimatedNewLine = new FieldCell[Field.fieldSize];
            for (Int32 i = 0, j = i + 1; i < estimatedNewLine.Length - 1; i++, j++)
            {
                estimatedNewLine[i] = cells[j, 1];
            }
            estimatedNewLine[estimatedNewLine.Length - 1] = field.freeFieldCell;

            //Подвинуть вверх вторую по порядку линию
            field.MoveLineLeft(1);

            for (Int32 i = 0; i < estimatedNewLine.Length; i++)
            {
                Assert.Equal(estimatedNewLine[i], cells[i, 1]);
            }

            Assert.Equal(estimatedFreeCell, field.freeFieldCell);
        }
        /// <summary>
        /// Тест на удачный сдвиг вниз второй по порядку линии.
        /// </summary>
        [Fact]
        public void TestMovement_MoveLineDown_SuccessfullMovement()
        {
            CreateField();
            Field field = this.field;
            FieldCell[,] cells = field.fieldCells;

            //Предполагаемая новая свободная ячейка
            FieldCell estimatedFreeCell = cells[1, Field.fieldSize - 1];
            //Предполагаемая новая линия.
            FieldCell[] estimatedNewLine = new FieldCell[Field.fieldSize];
            for (Int32 i = estimatedNewLine.Length - 1, j = i - 1; i > 0; i--, j--)
            {
                estimatedNewLine[i] = cells[1, j];
            }
            estimatedNewLine[0] = field.freeFieldCell;

            //Подвинуть вниз вторую по порядку линию
            field.MoveLineDown(1);

            for (Int32 i = 0; i < estimatedNewLine.Length; i++)
            {
                Assert.Equal(estimatedNewLine[i], cells[1, i]);
            }

            Assert.Equal(estimatedFreeCell, field.freeFieldCell);
        }
        /// <summary>
        /// Тест на удачный сдвиг вправо второй по порядку линии.
        /// </summary>
        [Fact]
        public void TestMovement_MoveLineRight_SuccessfullMovement()
        {
            CreateField();
            Field field = this.field;
            FieldCell[,] cells = field.fieldCells;

            //Предполагаемая новая свободная ячейка
            FieldCell estimatedFreeCell = cells[Field.fieldSize - 1, 1];
            //Предполагаемая новая линия.
            FieldCell[] estimatedNewLine = new FieldCell[Field.fieldSize];
            for (Int32 i = estimatedNewLine.Length - 1, j = i - 1; i > 0; i--, j--)
            {
                estimatedNewLine[i] = cells[j, 1];
            }
            estimatedNewLine[0] = field.freeFieldCell;

            //Подвинуть вправо вторую по порядку линию
            field.MoveLineRight(1);

            for (Int32 i = 0; i < estimatedNewLine.Length; i++)
            {
                Assert.Equal(estimatedNewLine[i], cells[i, 1]);
            }

            Assert.Equal(estimatedFreeCell, field.freeFieldCell);
        }
        /// <summary>
        /// Тест на неудачный сдвиг всех нечетных по порядку линий.
        /// Т.к. нумерация с нуля, то все-таки четных.
        /// </summary>
        [Fact]
        public void TestMovement_SuccessfullMovement_SuccessfullMovement()
        {
            CreateField();
            Field field = this.field;

            for (Int32 i = 0; i < Field.fieldSize; i++)
            {
                if (i % 2 == 0)
                {
                    Assert.False(field.MoveLineUp(i));
                    Assert.False(field.MoveLineRight(i));
                    Assert.False(field.MoveLineDown(i));
                    Assert.False(field.MoveLineLeft(i));
                }
                else
                {
                    Assert.True(field.MoveLineUp(i));
                    Assert.True(field.MoveLineRight(i));
                    Assert.True(field.MoveLineDown(i));
                    Assert.True(field.MoveLineLeft(i));
                }
            }

        }

        #endregion Движение линий.
    }
}
