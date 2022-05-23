using Assets.Scripts.GameModel.Cards;
using Assets.Scripts.GameModel.Player;
using Assets.Scripts.GameModel.PlayingField.Treasures;
using System;
using System.Collections.Generic;
using System.Drawing;
using Xunit;

namespace TestModel.ModelTests
{
    /// <summary>
    /// Класс тестирования класса игрока.
    /// </summary>
    public class UnitTestGameModelGamePlayer
    {
        /// <summary>
        /// Удачное создание клона игрока.
        /// </summary>
        [Fact]
        public void TestCreate_CreateCloneCard_SuccessfullCreate()
        {
            GamePlayer gamePlayer = new GamePlayer("q", Color.Red, new CardDeck(), 0, 0, 0);
            GamePlayer gamePlayerClone = gamePlayer.Clone();

            Assert.Equal(gamePlayer.name, gamePlayerClone.name);
            Assert.Equal(gamePlayer.color, gamePlayerClone.color);
            Assert.Equal(gamePlayer.position, gamePlayerClone.position);
            for(Int32 i = 0; i<gamePlayer.countCardInDeck;i++)
            {
                Assert.Equal(gamePlayer.PopCurrentCardForSearch(), gamePlayerClone.PopCurrentCardForSearch());
            }
        }
        /// <summary>
        /// Удачное сравнение двух игроков.
        /// </summary>
        [Fact]
        public void TestCompare_Compare_True()
        {

            GamePlayer gamePlayer1 = new GamePlayer("q", Color.Red, new CardDeck(), 0, 0, 0);
            GamePlayer gamePlayer2 = new GamePlayer("q", Color.Red, new CardDeck(), 0, 0, 1);
            GamePlayer gamePlayer3 = new GamePlayer("w", Color.Red, new CardDeck(), 0, 0, 2);
            GamePlayer gamePlayer4 = new GamePlayer("q", Color.Green, new CardDeck(), 0, 0, 3);
            GamePlayer gamePlayer5 = new GamePlayer("q", Color.Green, 
                new CardDeck(new List<Card>(){ new Card(TreasureAndStartPointsType.treasureOnMovingCell1)}), 0, 0, 4);
            GamePlayer gamePlayer6 = new GamePlayer("q", Color.Red, new CardDeck(), 1, 0, 5);

            Assert.True(gamePlayer1 == gamePlayer2);
            Assert.False(gamePlayer1 == gamePlayer3);
            Assert.False(gamePlayer1 == gamePlayer4);
            Assert.False(gamePlayer1 == gamePlayer5);
            Assert.True(gamePlayer1 == gamePlayer6);
        }
    }
}
