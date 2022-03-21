using Assets.Scripts.GameModel.PlayingField.FieldCells;
using Assets.Scripts.GameModel.PlayingField.FieldCells.SpecificFieldCells;
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

        #region Interaction

        [Fact]
        public void TestRotateCornerTwoDirectionNotInteractionFieldCell_TurnClockwise_UnsuccessfullRotate()
        {
            FieldCell cell = new CornerTwoDirectionFieldCell();

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
            FieldCell cell = new CornerTwoDirectionFieldCell();
            FieldCell cloneCell = cell.Clone();

            Assert.Equal(cell, cloneCell);

            cell = new LineTwoDirectionFieldCell();
            cloneCell = cell.Clone();

            Assert.Equal(cell, cloneCell);

            cell = new ThreeDirectionFieldCell();
            cloneCell = cell.Clone();

            Assert.Equal(cell, cloneCell);
        }

        #endregion Клонирование.
    }
}
