using Assets.Scripts.GameModel.PlayingField;
using Assets.Scripts.GameModel.PlayingField.FieldCells;
using Assets.Scripts.GameModel.PlayingField.FieldCells.SpecificFieldCells;
using System;
using Xunit;

namespace TestModel
{
    /// <summary>
    ///  Класс тестирования ячеек.
    /// </summary>
    public class UnitTestGameModelCells
    {
        #region Create

        [Fact]
        public void TestCreateCornerTwoDirectionFieldCell_Create_SuccessfullCreate()
        {
            FieldCell cell = new CornerTwoDirectionFieldCell();

            Assert.Equal(2, cell.directionCount);

            Assert.True(cell.IsHaveDirectionUp);
            Assert.True(cell.IsHaveDirectionRight);
            Assert.False(cell.IsHaveDirectionDown);
            Assert.False(cell.IsHaveDirectionLeft);
        }
        [Fact]
        public void TestCreateLineTwoDirectionFieldCell_Create_SuccessfullCreate()
        {
            FieldCell cell = new LineTwoDirectionFieldCell();

            Assert.Equal(2, cell.directionCount);

            Assert.True(cell.IsHaveDirectionUp);
            Assert.True(cell.IsHaveDirectionDown);
            Assert.False(cell.IsHaveDirectionRight);
            Assert.False(cell.IsHaveDirectionLeft);
        }
        [Fact]
        public void TestCreateThreeDirectionFieldCell_Create_SuccessfullCreate()
        {
            FieldCell cell = new ThreeDirectionFieldCell();

            Assert.Equal(3, cell.directionCount);

            Assert.True(cell.IsHaveDirectionUp);
            Assert.True(cell.IsHaveDirectionRight);
            Assert.True(cell.IsHaveDirectionDown);
            Assert.False(cell.IsHaveDirectionLeft);
        }

        #endregion Create

        #region Rotate

        [Fact]
        public void TestRotateCornerTwoDirectionFieldCell_TurnClockwise_SuccessfullRotate()
        {
            FieldCell cell = new CornerTwoDirectionFieldCell();

            cell.TurnClockwise();

            Assert.False(cell.IsHaveDirectionUp);
            Assert.True(cell.IsHaveDirectionRight);
            Assert.True(cell.IsHaveDirectionDown);
            Assert.False(cell.IsHaveDirectionLeft);
        }
        [Fact]
        public void TestRotateCornerTwoDirectionFieldCell_TurnCounterClockwise_SuccessfullRotate()
        {
            FieldCell cell = new CornerTwoDirectionFieldCell();

            cell.TurnCounterClockwise();

            Assert.True(cell.IsHaveDirectionUp);
            Assert.False(cell.IsHaveDirectionRight);
            Assert.False(cell.IsHaveDirectionDown);
            Assert.True(cell.IsHaveDirectionLeft);
        }
        [Fact]
        public void TestRotateLineTwoDirectionFieldCell_TurnClockwise_SuccessfullRotate()
        {
            FieldCell cell = new LineTwoDirectionFieldCell();

            cell.TurnClockwise();

            Assert.False(cell.IsHaveDirectionUp);
            Assert.False(cell.IsHaveDirectionDown);
            Assert.True(cell.IsHaveDirectionRight);
            Assert.True(cell.IsHaveDirectionLeft);
        }
        [Fact]
        public void TestRotateLineTwoDirectionFieldCell_TurnCounterClockwise_SuccessfullRotate()
        {
            FieldCell cell = new LineTwoDirectionFieldCell();

            cell.TurnClockwise();

            Assert.False(cell.IsHaveDirectionUp);
            Assert.False(cell.IsHaveDirectionDown);
            Assert.True(cell.IsHaveDirectionRight);
            Assert.True(cell.IsHaveDirectionLeft);
        }
        [Fact]
        public void TestRotateThreeDirectionFieldCell_TurnClockwise_SuccessfullRotate()
        {
            FieldCell cell = new ThreeDirectionFieldCell();

            cell.TurnClockwise();

            Assert.False(cell.IsHaveDirectionUp);
            Assert.True(cell.IsHaveDirectionRight);
            Assert.True(cell.IsHaveDirectionDown);
            Assert.True(cell.IsHaveDirectionLeft);
        }
        [Fact]
        public void TestRotateThreeDirectionFieldCell_TurnCounterClockwise_SuccessfullRotate()
        {
            FieldCell cell = new ThreeDirectionFieldCell();

            cell.TurnCounterClockwise();

            Assert.True(cell.IsHaveDirectionUp);
            Assert.True(cell.IsHaveDirectionRight);
            Assert.False(cell.IsHaveDirectionDown);
            Assert.True(cell.IsHaveDirectionLeft);
        }

        #endregion Rotate
    }
    /// <summary>
    ///  Класс тестировани¤ игрового поля.
    /// </summary>
    public class UnitTestGameModelPlaingField
    {
        #region Create

        [Fact]
        public void TestCreate_CreateField_SuccessfullCreatePinnedCells()
        {
            PlayingField field = new PlayingField();

            #region  углы

            FieldCell currentCell = field.fieldCells[0, 0];
            Assert.Equal(currentCell.CellType, CellType.corner);
            Assert.True(currentCell.IsHaveDirectionRight);
            Assert.True(currentCell.IsHaveDirectionDown);
            currentCell = field.fieldCells[0, 6];
            Assert.Equal(currentCell.CellType, CellType.corner);
            Assert.True(currentCell.IsHaveDirectionDown);
            Assert.True(currentCell.IsHaveDirectionLeft);
            currentCell = field.fieldCells[6, 0];
            Assert.Equal(currentCell.CellType, CellType.corner);
            Assert.True(currentCell.IsHaveDirectionUp);
            Assert.True(currentCell.IsHaveDirectionRight);
            currentCell = field.fieldCells[6, 6];
            Assert.Equal(currentCell.CellType, CellType.corner);
            Assert.True(currentCell.IsHaveDirectionLeft);
            Assert.True(currentCell.IsHaveDirectionUp);

            #endregion  углы

            #region центр

            currentCell = field.fieldCells[2, 2];
            Assert.Equal(currentCell.CellType, CellType.treeDirection);
            Assert.True(currentCell.IsHaveDirectionUp);
            Assert.True(currentCell.IsHaveDirectionRight);
            Assert.True(currentCell.IsHaveDirectionDown);
            currentCell = field.fieldCells[2, 4];
            Assert.Equal(currentCell.CellType, CellType.treeDirection);
            Assert.True(currentCell.IsHaveDirectionRight);
            Assert.True(currentCell.IsHaveDirectionDown);
            Assert.True(currentCell.IsHaveDirectionLeft);
            currentCell = field.fieldCells[4, 4];
            Assert.Equal(currentCell.CellType, CellType.treeDirection);
            Assert.True(currentCell.IsHaveDirectionDown);
            Assert.True(currentCell.IsHaveDirectionLeft);
            Assert.True(currentCell.IsHaveDirectionUp);
            currentCell = field.fieldCells[4, 2];
            Assert.Equal(currentCell.CellType, CellType.treeDirection);
            Assert.True(currentCell.IsHaveDirectionLeft);
            Assert.True(currentCell.IsHaveDirectionUp);
            Assert.True(currentCell.IsHaveDirectionRight);

            #endregion центр

            #region границы

            currentCell = field.fieldCells[0, 2];
            Assert.Equal(currentCell.CellType, CellType.treeDirection);
            Assert.True(currentCell.IsHaveDirectionRight);
            Assert.True(currentCell.IsHaveDirectionDown);
            Assert.True(currentCell.IsHaveDirectionLeft);
            currentCell = field.fieldCells[0, 4];
            Assert.Equal(currentCell.CellType, CellType.treeDirection);
            Assert.True(currentCell.IsHaveDirectionRight);
            Assert.True(currentCell.IsHaveDirectionDown);
            Assert.True(currentCell.IsHaveDirectionLeft);
            currentCell = field.fieldCells[2, 6];
            Assert.Equal(currentCell.CellType, CellType.treeDirection);
            Assert.True(currentCell.IsHaveDirectionDown);
            Assert.True(currentCell.IsHaveDirectionLeft);
            Assert.True(currentCell.IsHaveDirectionUp);
            currentCell = field.fieldCells[4, 6];
            Assert.Equal(currentCell.CellType, CellType.treeDirection);
            Assert.True(currentCell.IsHaveDirectionDown);
            Assert.True(currentCell.IsHaveDirectionLeft);
            Assert.True(currentCell.IsHaveDirectionUp);
            currentCell = field.fieldCells[6, 2];
            Assert.Equal(currentCell.CellType, CellType.treeDirection);
            Assert.True(currentCell.IsHaveDirectionLeft);
            Assert.True(currentCell.IsHaveDirectionUp);
            Assert.True(currentCell.IsHaveDirectionRight);
            currentCell = field.fieldCells[6, 4];
            Assert.Equal(currentCell.CellType, CellType.treeDirection);
            Assert.True(currentCell.IsHaveDirectionLeft);
            Assert.True(currentCell.IsHaveDirectionUp);
            Assert.True(currentCell.IsHaveDirectionRight);
            currentCell = field.fieldCells[2, 0];
            Assert.Equal(currentCell.CellType, CellType.treeDirection);
            Assert.True(currentCell.IsHaveDirectionUp);
            Assert.True(currentCell.IsHaveDirectionRight);
            Assert.True(currentCell.IsHaveDirectionDown);
            currentCell = field.fieldCells[4, 0];
            Assert.Equal(currentCell.CellType, CellType.treeDirection);
            Assert.True(currentCell.IsHaveDirectionUp);
            Assert.True(currentCell.IsHaveDirectionRight);
            Assert.True(currentCell.IsHaveDirectionDown);

            #endregion границы
        }

        #endregion Create
    }

}
