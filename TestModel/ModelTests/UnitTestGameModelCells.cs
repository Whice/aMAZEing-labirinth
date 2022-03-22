using Assets.Scripts.GameModel.PlayingField.FieldCells;
using Xunit;

namespace TestModel.ModelTests
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
            FieldCell cell = new FieldCell(CellType.corner);

            Assert.Equal(2, cell.directionCount);

            Assert.True(cell.IsHaveDirectionUp);
            Assert.True(cell.IsHaveDirectionRight);
            Assert.False(cell.IsHaveDirectionDown);
            Assert.False(cell.IsHaveDirectionLeft);
        }
        [Fact]
        public void TestCreateLineTwoDirectionFieldCell_Create_SuccessfullCreate()
        {
            FieldCell cell = new FieldCell(CellType.line);

            Assert.Equal(2, cell.directionCount);

            Assert.True(cell.IsHaveDirectionUp);
            Assert.True(cell.IsHaveDirectionDown);
            Assert.False(cell.IsHaveDirectionRight);
            Assert.False(cell.IsHaveDirectionLeft);
        }
        [Fact]
        public void TestCreateThreeDirectionFieldCell_Create_SuccessfullCreate()
        {
            FieldCell cell = new FieldCell(CellType.threeDirection);

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
            FieldCell cell = new FieldCell(CellType.corner);

            cell.TurnClockwise();

            Assert.False(cell.IsHaveDirectionUp);
            Assert.True(cell.IsHaveDirectionRight);
            Assert.True(cell.IsHaveDirectionDown);
            Assert.False(cell.IsHaveDirectionLeft);
        }
        [Fact]
        public void TestRotateCornerTwoDirectionFieldCell_TurnCounterClockwise_SuccessfullRotate()
        {
            FieldCell cell = new FieldCell(CellType.corner);

            cell.TurnCounterClockwise();

            Assert.True(cell.IsHaveDirectionUp);
            Assert.False(cell.IsHaveDirectionRight);
            Assert.False(cell.IsHaveDirectionDown);
            Assert.True(cell.IsHaveDirectionLeft);
        }
        [Fact]
        public void TestRotateLineTwoDirectionFieldCell_TurnClockwise_SuccessfullRotate()
        {
            FieldCell cell = new FieldCell(CellType.line);

            cell.TurnClockwise();

            Assert.False(cell.IsHaveDirectionUp);
            Assert.False(cell.IsHaveDirectionDown);
            Assert.True(cell.IsHaveDirectionRight);
            Assert.True(cell.IsHaveDirectionLeft);
        }
        [Fact]
        public void TestRotateLineTwoDirectionFieldCell_TurnCounterClockwise_SuccessfullRotate()
        {
            FieldCell cell = new FieldCell(CellType.line);

            cell.TurnClockwise();

            Assert.False(cell.IsHaveDirectionUp);
            Assert.False(cell.IsHaveDirectionDown);
            Assert.True(cell.IsHaveDirectionRight);
            Assert.True(cell.IsHaveDirectionLeft);
        }
        [Fact]
        public void TestRotateThreeDirectionFieldCell_TurnClockwise_SuccessfullRotate()
        {
            FieldCell cell = new FieldCell(CellType.threeDirection);

            cell.TurnClockwise();

            Assert.False(cell.IsHaveDirectionUp);
            Assert.True(cell.IsHaveDirectionRight);
            Assert.True(cell.IsHaveDirectionDown);
            Assert.True(cell.IsHaveDirectionLeft);
        }
        [Fact]
        public void TestRotateThreeDirectionFieldCell_TurnCounterClockwise_SuccessfullRotate()
        {
            FieldCell cell = new FieldCell(CellType.threeDirection);

            cell.TurnCounterClockwise();

            Assert.True(cell.IsHaveDirectionUp);
            Assert.True(cell.IsHaveDirectionRight);
            Assert.False(cell.IsHaveDirectionDown);
            Assert.True(cell.IsHaveDirectionLeft);
        }

        #endregion Rotate

        #region Interaction

        [Fact]
        public void TestRotateCornerTwoDirectionNotInteractionFieldCell_TurnClockwise_UnsuccessfullRotate()
        {
            FieldCell cell = new FieldCell(CellType.corner);

            //По умолчанию взаимодействие включено

            cell.TurnClockwise();

            Assert.False(cell.IsHaveDirectionUp);
            Assert.True(cell.IsHaveDirectionRight);
            Assert.True(cell.IsHaveDirectionDown);
            Assert.False(cell.IsHaveDirectionLeft);

            cell.TurnCounterClockwise();

            Assert.True(cell.IsHaveDirectionUp);
            Assert.True(cell.IsHaveDirectionRight);
            Assert.False(cell.IsHaveDirectionDown);
            Assert.False(cell.IsHaveDirectionLeft);

            //при отключении взаимодействия, повороты не должны работать.
            cell.isInteractable = false;

            cell.TurnClockwise();

            Assert.True(cell.IsHaveDirectionUp);
            Assert.True(cell.IsHaveDirectionRight);
            Assert.False(cell.IsHaveDirectionDown);
            Assert.False(cell.IsHaveDirectionLeft);

            cell.TurnCounterClockwise();

            Assert.True(cell.IsHaveDirectionUp);
            Assert.True(cell.IsHaveDirectionRight);
            Assert.False(cell.IsHaveDirectionDown);
            Assert.False(cell.IsHaveDirectionLeft);
        }

        #endregion Interactions

        #region Клонирование.

        /// <summary>
        /// Тест клонирования.
        /// <br/>Действие: клонирование.
        /// <br/>Результат: соответсвие экземпляров.
        /// </summary>
        [Fact]
        public void TestCloneFieldCell_Cloning_InstanceMatching()
        {
            FieldCell cell = new FieldCell(CellType.corner);
            FieldCell cloneCell = cell.Clone();

            Assert.Equal(cell, cloneCell);

            cell = new FieldCell(CellType.line);
            cloneCell = cell.Clone();

            Assert.Equal(cell, cloneCell);

            cell = new FieldCell(CellType.threeDirection);
            cloneCell = cell.Clone();

            Assert.Equal(cell, cloneCell);
        }

        #endregion Клонирование.
    }
}
