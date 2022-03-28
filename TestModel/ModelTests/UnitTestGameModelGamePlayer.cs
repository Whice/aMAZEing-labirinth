using Assets.Scripts.GameModel.CardDeck;
using Assets.Scripts.GameModel.Player;
using Assets.Scripts.GameModel.PlayingField.Treasures;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            GamePlayer gamePlayer = new GamePlayer("q", Color.Red, new List<Assets.Scripts.GameModel.CardDeck.Card>(), 0, 0);
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

            GamePlayer gamePlayer1 = new GamePlayer("q", Color.Red, new List<Card>(), 0, 0);
            GamePlayer gamePlayer2 = new GamePlayer("q", Color.Red, new List<Card>(), 0, 0);
            GamePlayer gamePlayer3 = new GamePlayer("w", Color.Red, new List<Card>(), 0, 0);
            GamePlayer gamePlayer4 = new GamePlayer("q", Color.Green, new List<Card>(), 0, 0);
            GamePlayer gamePlayer5 = new GamePlayer("q", Color.Green, 
                new List<Card>() { new Card(TreasureAndStartPointsType.treasureOnMovingCell1)}, 0, 0);
            GamePlayer gamePlayer6 = new GamePlayer("q", Color.Red, new List<Card>(), 1, 0);

            Assert.True(gamePlayer1 == gamePlayer2);
            Assert.False(gamePlayer1 == gamePlayer3);
            Assert.False(gamePlayer1 == gamePlayer4);
            Assert.False(gamePlayer1 == gamePlayer5);
            Assert.True(gamePlayer1 == gamePlayer6);
        }
    }
}
