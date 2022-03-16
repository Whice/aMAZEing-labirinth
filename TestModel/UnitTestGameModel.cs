using Assets.Scripts.GameModel.PlayingField.FieldCells;
using Assets.Scripts.GameModel.PlayingField.FieldCells.SpecificFieldCells;
using System;
using Xunit;

namespace TestModel
{
    public class UnitTestGameModel
    {
        #region FieldCells

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

        #endregion FieldCells
    }
}
