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
        private PlayingField field;
#pragma warning restore CS8618
        /// <summary>
        /// Создать игровое поле, если оно не создано.
        /// </summary>
        private void CreateField()
        {
            if (this.field == null)
            {
                this.field = new PlayingField();
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
            PlayingField field = this.field;

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
            PlayingField field = this.field;

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
            PlayingField field = this.field;

            Assert.NotNull(field.freeFieldCell);
        }
        /// <summary>
        /// Правильное количество ячеек каждого вида.
        /// </summary>
        [Fact]
        public void TestCreate_CreateField_RightCountCellsInField()
        {
            CreateField();
            PlayingField field = this.field;
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
        [Fact]
        public void TestCreate_RotateAllNotInreationCellsField_UnsuccessfullRotate()
        {
            CreateField();
            PlayingField field = this.field;
            PlayingField field2 = this.field.Clone();

            foreach (FieldCell cell in field.fieldCells)
                cell.TurnClockwise();

            for (Int32 i = 0; i < PlayingField.fieldSize; i++)
                for (Int32 j = 0; j < PlayingField.fieldSize; j++)
                {
                    Assert.True(field.fieldCells[i, j] == field2.fieldCells[i, j]);
                }
        }

        #endregion Create
    }
}
