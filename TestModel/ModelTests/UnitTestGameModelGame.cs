using Assets.Scripts.GameModel;
using Assets.Scripts.GameModel.Player;
using Assets.Scripts.GameModel.TurnPhaseAndExtensions;
using System;
using System.Drawing;
using Xunit;

namespace TestModel.ModelTests
{
    /// <summary>
    /// Тесты для класса игры.
    /// </summary>
    public class UnitTestGameModelGame
    {
        [Fact]
        public void TestCreate_CreateGameWithTwoPlayers_SuccessfullCreate()
        {
            PlayerInfo[] playerInfos = new PlayerInfo[]
            {
                new PlayerInfo("1", Color.Red),
                new PlayerInfo("2", Color.Yellow),
            };
            GameInfo gameInfo = new GameInfo(playerInfos);
            (Boolean isStart, Game game) = Game.CreateGameWithStart(gameInfo);

            Assert.True(isStart);

            Assert.NotNull(game.field);
            Assert.NotNull(game.freeCell);
            Assert.NotNull(game.deck);
            Assert.NotNull(game.players);
            Assert.NotNull(game.currentPlayer);
            Assert.Equal(TurnPhase.movingCell, game.currentPhase);
            Assert.Equal(playerInfos.Length, game.countOfPlayersPlaying);
            Assert.True(game.currentPlayerNumber > -1 && game.currentPlayerNumber < playerInfos.Length);
            Assert.True(game.deck.isEmpty);
        }
        [Fact]
        public void TestCreate_CreateGameWithFourPlayers_SuccessfullCreate()
        {
            PlayerInfo[] playerInfos = new PlayerInfo[]
            {
                new PlayerInfo("1", Color.Red),
                new PlayerInfo("2", Color.Yellow),
                new PlayerInfo("3", Color.Green),
                new PlayerInfo("4", Color.Purple),
            };
            GameInfo gameInfo = new GameInfo(playerInfos);
            (Boolean isStart, Game game) = Game.CreateGameWithStart(gameInfo);

            Assert.True(isStart);

            Assert.NotNull(game.field);
            Assert.NotNull(game.freeCell);
            Assert.NotNull(game.deck);
            Assert.NotNull(game.players);
            Assert.NotNull(game.currentPlayer);
            Assert.Equal(TurnPhase.movingCell, game.currentPhase);
            Assert.Equal(playerInfos.Length, game.countOfPlayersPlaying);
            Assert.True(game.currentPlayerNumber > -1 && game.currentPlayerNumber < playerInfos.Length);
            Assert.True(game.deck.isEmpty);
        }
        [Fact]
        public void TestCreate_CreateGameWithFivePlayers_UnsuccessfullCreate()
        {
            PlayerInfo[] playerInfos = new PlayerInfo[]
            {
                new PlayerInfo("1", Color.Red),
                new PlayerInfo("2", Color.Yellow),
                new PlayerInfo("3", Color.Green),
                new PlayerInfo("4", Color.Purple),
                new PlayerInfo("5", Color.Firebrick),
            };
            Boolean isStart = true;

            GameInfo gameInfo = new GameInfo(playerInfos);
            (isStart, _) = Game.CreateGameWithStart(gameInfo);

            Assert.False(isStart);
        }
    }
}
