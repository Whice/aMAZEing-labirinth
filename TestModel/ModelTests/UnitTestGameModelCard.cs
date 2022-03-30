using Assets.Scripts.GameModel.Cards;
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

#pragma warning disable CS8600
            Action act = null;
            ArgumentException exception = null;
#pragma warning restore CS8600
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
        /// <summary>
        /// Удачное создание клона карты.
        /// </summary>
        [Fact]
        public void TestCreate_CreateCloneCard_SuccessfullCreate()
        {
            Card card = new Card(TreasureAndStartPointsType.treasureOnMovingCell1, true);
            Card clone = card.Clone();
            Assert.Equal(card.treasure, clone.treasure);
            Assert.Equal(card.isOpen, clone.isOpen);
            
            card.FlipCard();
            clone = card.Clone();
            Assert.Equal(card.treasure, clone.treasure);
            Assert.Equal(card.isOpen, clone.isOpen);
        }
        /// <summary>
        /// Удачное сравнение двух карт.
        /// </summary>
        [Fact]
        public void TestCompare_Compare_True()
        {
            Card card1 = new Card(TreasureAndStartPointsType.treasureOnMovingCell1, true);
            Card card2 = new Card(TreasureAndStartPointsType.treasureOnMovingCell1, true);
            Card card3 = new Card(TreasureAndStartPointsType.treasureOnMovingCell1, false);
            Card card4 = new Card(TreasureAndStartPointsType.treasureOnMovingCell12, false);

            Assert.True(card1 == card2);
            Assert.True(card3 == card2);
            Assert.False(card3 == card4);
            Assert.False(card2 == card4);

            Assert.False(card1 != card2);
            Assert.False(card3 != card2);
            Assert.True(card3 != card4);
            Assert.True(card2 != card4);
        }
    }
}
