using Assets.Scripts.GameModel.CardDeck;
using Assets.Scripts.GameModel.PlayingField.Treasures;
using System;
using Xunit;

namespace TestModel.ModelTests
{
    /// <summary>
    ///  Класс тестирования игровой карты.
    /// </summary>
    public class UnitTestGameModelCard
    {
        /// <summary>
        /// Неудачное создание карты.
        /// </summary>
        [Fact]
        public void TestCreate_CreateCard_UnsuccessfullCreate()
        {
            Action act = null;
            ArgumentException exception = null;
            for (Int32 i = 0; i < 6; i++)
            {
                act = () => new Card((TreasureAndStartPointsType)i);

                exception = Assert.Throws<ArgumentException>(act);

                Assert.Equal("Карте должен быть задан тип сокровища!\nThe card must be set to a treasure type!", exception.Message);
            }

            act = () => new Card((TreasureAndStartPointsType)99);

            exception = Assert.Throws<ArgumentException>(act);

            Assert.Equal("Карте должен быть задан тип сокровища!\nThe card must be set to a treasure type!", exception.Message);
        }
    }
}
