using Assets.Scripts.GameModel;
using Assets.Scripts.GameModel.Commands.GameCommands;
using Assets.Scripts.GameModel.Player;
using Assets.Scripts.GameModel.PlayingField;
using System;
using System.Collections.Generic;
using System.Drawing;
using Xunit;

namespace TestModel.ModelTests
{
    /// <summary>
    /// Класс тестирования команд.
    /// </summary>
    public class UnitTestGameCommands
    {
        /// <summary>
        /// Создание игры для двух игроков.
        /// </summary>
        /// <returns></returns>
        private Game CreateAndStartGameWithTwoPlayes()
        {
            PlayerInfo[] playerInfos = new PlayerInfo[]
            {
                new PlayerInfo("1", Color.Red),
                new PlayerInfo("2", Color.Yellow),
            };
            GameInfo gameInfo = new GameInfo(playerInfos);
            gameInfo.cardsShuffleSeed = 1;
            gameInfo.cellsShuffleSeed = 1;
            gameInfo.fisrtPlayerNumberSeed = 1;
            (Boolean isStart, Game game) = Game.CreateGameWithStart(gameInfo);
            return game;
        }
        /// <summary>
        /// Получить координаты ячеек, куда может пойти текущий игрок в указанной игре. 
        /// </summary>
        /// <param name="game"></param>
        /// <returns></returns>
        private Point[] GetPointsForMove(Game game)
        {
            HashSet<Point> cellsForMove = game.field.GetPointsForMove(game.currentPlayer);
            Point[] cellsForMoveArray = new Point[cellsForMove.Count];
            Int32 index = 0;
            foreach (Point point in cellsForMove)
            {
                cellsForMoveArray[index] = point;
                ++index;
            }

            return cellsForMoveArray;
        }
        /// <summary>
        /// Выполнить один ход игрока: сдвинуть клетку и сдвинуть аватара.
        /// </summary>
        /// <param name="game">Модель игры.</param>
        /// <param name="commandsCount">Количество комманд, которое увеличится при совершении хода.</param>
        private void PerformOnePlayerMove(Game game, ref Int32 commandsCount)
        {
            game.SetFreeCellToField(1, FieldSide.left);
            ++commandsCount;

            Point[] cellsForMove = GetPointsForMove(game);
            if (cellsForMove.Length > 0)
            {
                game.SetPlayerAvatarToField(cellsForMove[0].X, cellsForMove[0].Y);
                ++commandsCount;
            }
        }
        /// <summary>
        /// Выполнить один неправильный ход игрока: сдвинуть клетку и 
        /// попытатьсясдвинуть аватара туда, куда не получиться, а потому туда, куда получиться.
        /// </summary>
        /// <param name="game">Модель игры.</param>
        /// <param name="commandsCount">Количество комманд, которое увеличится при совершении хода.</param>
        private void PerformOnePlayerWrongMove(Game game, ref Int32 commandsCount)
        {
            game.SetFreeCellToField(1, FieldSide.left);
            ++commandsCount;

            Point[] cellsForMove = GetPointsForMove(game);
            Point wrongMove = new Point(0, 0);
            Boolean isBadMove = true;
            for (Int32 i = 0; i < Field.FIELD_SIZE; i++)
                for (Int32 j = 0; j < Field.FIELD_SIZE; j++)
                {
                    isBadMove = true;
                    foreach (Point point in cellsForMove)
                    {
                        if (point.X == i && point.Y == j)
                        {
                            isBadMove = false;
                        }
                        if (isBadMove)
                        {
                            wrongMove = new Point(i, j);
                            goto isBadMove;
                        }
                    }
                }

            isBadMove:
            game.SetPlayerAvatarToField(cellsForMove[0].X, cellsForMove[0].Y);
            ++commandsCount;

            if (cellsForMove.Length > 0)
            {
                game.SetPlayerAvatarToField(cellsForMove[0].X, cellsForMove[0].Y);
                ++commandsCount;
            }
        }
        /// <summary>
        /// Удачное создание нескольких команд.
        /// </summary>
        [Fact]
        public void TestCreate_CreateSeveralComand_SuccessfullCreate()
        {
            Game game = CreateAndStartGameWithTwoPlayes();
            Int32 commandsCount = 0;

            PerformOnePlayerMove(game, ref commandsCount);
            PerformOnePlayerMove(game, ref commandsCount);
            PerformOnePlayerMove(game, ref commandsCount);

            Int32 commandsCountInKeeper = game.commandKeeper.count;

            Assert.Equal(commandsCount, commandsCountInKeeper); 
        }
        /// <summary>
        /// Удачное повторение нескольких команд во второй игре.
        /// </summary>
        [Fact]
        public void TestReplay_CreateSeveralComandAndPlaySecondGameWithThem_SuccessfullPlaySecondGame()
        {
            Game firstGame = CreateAndStartGameWithTwoPlayes();
            Game secondGame = firstGame.Clone();
            Int32 commandsCount = 0;

            PerformOnePlayerMove(firstGame, ref commandsCount);
            PerformOnePlayerMove(firstGame, ref commandsCount);
            PerformOnePlayerMove(firstGame, ref commandsCount);

            GameCommandKeeper keeper = firstGame.commandKeeper.Clone();
            keeper.isStartWithFirstCommand = true;
            Int32 executedCommandCount = 0;
            while (!keeper.isEmpty)
            {
                Assert.True(secondGame.ExecuteCommand(keeper.Pop()));
                ++executedCommandCount;
                Assert.Equal(executedCommandCount, secondGame.commandKeeper.count);
            }

            Assert.Equal(firstGame.commandKeeper.count, secondGame.commandKeeper.count); 
            Assert.True(firstGame.Equals(secondGame));
        }
        /// <summary>
        /// Удачная отмена нескольких команд в игре.
        /// </summary>
        [Fact]
        public void TestUndoMoves_CreateSeveralComandAndUndoThem_SuccessfullUndo()
        {
            Game game = CreateAndStartGameWithTwoPlayes();
            Game originGame = game.Clone();
            Int32 commandsCount = 0;

            PerformOnePlayerMove(game, ref commandsCount);
            PerformOnePlayerMove(game, ref commandsCount);
            PerformOnePlayerMove(game, ref commandsCount);
            PerformOnePlayerWrongMove(game, ref commandsCount);


            GameCommandKeeper keeper = game.commandKeeper.Clone();
            while (!keeper.isEmpty)
            {
                Assert.True(game.UndoCommand(keeper.Pop()));
                --commandsCount;
            }

            Assert.Equal(0, game.commandKeeper.count);
            Assert.True(game.Equals(originGame));
        }
    }
}
