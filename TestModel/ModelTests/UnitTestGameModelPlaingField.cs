using Assets.Scripts.GameModel.Cards;
using Assets.Scripts.GameModel.Player;
using Assets.Scripts.GameModel.PlayingField;
using Assets.Scripts.GameModel.PlayingField.FieldCells;
using System;
using System.Drawing;
using Xunit;

namespace TestModel.ModelTests
{
    /// <summary>
    ///  Класс тестирования игрового поля.
    /// </summary>
    public class UnitTestGameModelPlaingField
    {
        #region Дополнительные поля и методы.

#pragma warning disable CS8618
        /// <summary>
        /// Игровое поле для всех тестов создания.
        /// </summary>
        private Field field;
#pragma warning restore CS8618
        /// <summary>
        /// Создать игровое поле, если оно не создано.
        /// </summary>
        private void CreateField()
        {
            if (this.field == null)
            {
                this.field = new Field();
            }
        }

        #endregion Дополнительные поля и методы.

        #region Create

        /// <summary>
        /// Правильное заполнение закрепленных ячеек при создании.
        /// </summary>
        [Fact]
        public void TestCreate_CreateField_SuccessfullCreatePinnedCells()
        {
            CreateField();
            Field field = this.field.Clone();

            #region  углы

            FieldCell currentCell = field[0, 0];
            Assert.Equal(CellType.corner, currentCell.CellType);
            Assert.True(currentCell.IsHaveDirectionRight);
            Assert.True(currentCell.IsHaveDirectionDown);
            currentCell = field[0, 6];
            Assert.Equal(CellType.corner, currentCell.CellType);
            Assert.True(currentCell.IsHaveDirectionDown);
            Assert.True(currentCell.IsHaveDirectionLeft);
            currentCell = field[6, 0];
            Assert.Equal(CellType.corner, currentCell.CellType);
            Assert.True(currentCell.IsHaveDirectionUp);
            Assert.True(currentCell.IsHaveDirectionRight);
            currentCell = field[6, 6];
            Assert.Equal(CellType.corner, currentCell.CellType);
            Assert.True(currentCell.IsHaveDirectionLeft);
            Assert.True(currentCell.IsHaveDirectionUp);

            #endregion  углы

            #region центр

            currentCell = field[2, 2];
            Assert.Equal(CellType.threeDirection, currentCell.CellType);
            Assert.True(currentCell.IsHaveDirectionUp);
            Assert.True(currentCell.IsHaveDirectionRight);
            Assert.True(currentCell.IsHaveDirectionDown);
            currentCell = field[2, 4];
            Assert.Equal(CellType.threeDirection, currentCell.CellType);
            Assert.True(currentCell.IsHaveDirectionRight);
            Assert.True(currentCell.IsHaveDirectionDown);
            Assert.True(currentCell.IsHaveDirectionLeft);
            currentCell = field[4, 4];
            Assert.Equal(CellType.threeDirection, currentCell.CellType);
            Assert.True(currentCell.IsHaveDirectionDown);
            Assert.True(currentCell.IsHaveDirectionLeft);
            Assert.True(currentCell.IsHaveDirectionUp);
            currentCell = field[4, 2];
            Assert.Equal(CellType.threeDirection, currentCell.CellType);
            Assert.True(currentCell.IsHaveDirectionLeft);
            Assert.True(currentCell.IsHaveDirectionUp);
            Assert.True(currentCell.IsHaveDirectionRight);

            #endregion центр

            #region границы

            currentCell = field[0, 2];
            Assert.Equal(CellType.threeDirection, currentCell.CellType);
            Assert.True(currentCell.IsHaveDirectionRight);
            Assert.True(currentCell.IsHaveDirectionDown);
            Assert.True(currentCell.IsHaveDirectionLeft);
            currentCell = field[0, 4];
            Assert.Equal(CellType.threeDirection, currentCell.CellType);
            Assert.True(currentCell.IsHaveDirectionRight);
            Assert.True(currentCell.IsHaveDirectionDown);
            Assert.True(currentCell.IsHaveDirectionLeft);
            currentCell = field[2, 6];
            Assert.Equal(CellType.threeDirection, currentCell.CellType);
            Assert.True(currentCell.IsHaveDirectionDown);
            Assert.True(currentCell.IsHaveDirectionLeft);
            Assert.True(currentCell.IsHaveDirectionUp);
            currentCell = field[4, 6];
            Assert.Equal(CellType.threeDirection, currentCell.CellType);
            Assert.True(currentCell.IsHaveDirectionDown);
            Assert.True(currentCell.IsHaveDirectionLeft);
            Assert.True(currentCell.IsHaveDirectionUp);
            currentCell = field[6, 2];
            Assert.Equal(CellType.threeDirection, currentCell.CellType);
            Assert.True(currentCell.IsHaveDirectionLeft);
            Assert.True(currentCell.IsHaveDirectionUp);
            Assert.True(currentCell.IsHaveDirectionRight);
            currentCell = field[6, 4];
            Assert.Equal(CellType.threeDirection, currentCell.CellType);
            Assert.True(currentCell.IsHaveDirectionLeft);
            Assert.True(currentCell.IsHaveDirectionUp);
            Assert.True(currentCell.IsHaveDirectionRight);
            currentCell = field[2, 0];
            Assert.Equal(CellType.threeDirection, currentCell.CellType);
            Assert.True(currentCell.IsHaveDirectionUp);
            Assert.True(currentCell.IsHaveDirectionRight);
            Assert.True(currentCell.IsHaveDirectionDown);
            currentCell = field[4, 0];
            Assert.Equal(CellType.threeDirection, currentCell.CellType);
            Assert.True(currentCell.IsHaveDirectionUp);
            Assert.True(currentCell.IsHaveDirectionRight);
            Assert.True(currentCell.IsHaveDirectionDown);

            #endregion границы
        }
        /// <summary>
        /// Отсутсвуют пустые ячейки на поле. Они должны быть все определены.
        /// </summary>
        [Fact]
        public void TestCreate_CreateField_ThereAreNoNullValuesI​nField()
        {
            CreateField();
            Field field = this.field.Clone();

            foreach (FieldCell cell in field)
            {
                Assert.NotNull(cell);
            }
        }
        /// <summary>
        /// Заполнение свободной ячейки присоздании.
        /// </summary>
        [Fact]
        public void TestCreate_CreateField_SuccessfullCreateFreeCell()
        {
            CreateField();
            Field field = this.field.Clone();

            Assert.NotNull(field.freeFieldCell);
        }
        /// <summary>
        /// Правильное количество ячеек каждого вида.
        /// </summary>
        [Fact]
        public void TestCreate_CreateField_RightCountCellsInField()
        {
            CreateField();
            Field field = this.field.Clone();
            Int32 countCoreners = 0;
            Int32 countLines = 0;
            Int32 countThreeDirections = 0;
            foreach (FieldCell cell in field)
            {
                if (cell.CellType == CellType.threeDirection)
                    ++countThreeDirections;
                if (cell.CellType == CellType.line)
                    ++countLines;
                if (cell.CellType == CellType.corner)
                    ++countCoreners;
            }

            if (field.freeFieldCell.CellType == CellType.corner)
                ++countCoreners;
            else if (field.freeFieldCell.CellType == CellType.line)
                ++countLines;
            else if (field.freeFieldCell.CellType == CellType.threeDirection)
                ++countThreeDirections;

            Assert.Equal(20, countCoreners);
            Assert.Equal(12, countLines);
            Assert.Equal(18, countThreeDirections);
        }
        /// <summary>
        /// Правильное заполнение закрепленных ячеек при создании.
        /// </summary>
        [Fact]
        public void TestCreate_RotateAllNotInreationCellsField_UnsuccessfullRotate()
        {
            CreateField();
            Field field = this.field.Clone();
            Field field2 = this.field.Clone();

            foreach (FieldCell cell in field2)
                cell.TurnClockwise();

            for (Int32 i = 0; i < Field.FIELD_SIZE; i++)
                for (Int32 j = 0; j < Field.FIELD_SIZE; j++)
                {
                    Assert.True(field[i, j] == field2[i, j]);
                }
        }
        /// <summary>
        /// Правильное количество сокровищ каждого типа при создании.
        /// </summary>
        [Fact]
        public void TestCreate_CreateAllTresures_RightCountTReasuresEachType()
        {

            CreateField();
            Field field = this.field.Clone();
            Int32 treasureType;
            Int32 countStartPoints = 0;
            Int32 countPinnedTreasures = 0;
            Int32 countMovingTreasures = 0;
            foreach (FieldCell cell in field)
            {
                treasureType = (Int32)cell.treasureOrStartPoints;
                if (treasureType >= 2 && treasureType <= 5)
                    ++countStartPoints;
                else if (treasureType >= 6 && treasureType <= 17)
                    ++countPinnedTreasures;
                else if (treasureType >= 18 && treasureType <= 29)
                    ++countMovingTreasures;
            }

            treasureType = (Int32)field.freeFieldCell.treasureOrStartPoints;
            if (treasureType >= 18 && treasureType <= 29)
                ++countMovingTreasures;

            Assert.Equal(4, countStartPoints);
            Assert.Equal(12, countPinnedTreasures);
            Assert.Equal(12, countMovingTreasures);
        }

        #endregion Create

        #region Движение линий.

        /// <summary>
        /// Тест на удачный сдвиг вверх второй по порядку линии.
        /// </summary>
        [Fact]
        public void TestMovement_MoveLineUp_SuccessfullMovement()
        {
            CreateField();
            Field field = this.field.Clone();

            //Предполагаемая новая свободная ячейка
            FieldCell estimatedFreeCell = field[1, 0];
            //Предполагаемая новая линия.
            FieldCell[] estimatedNewLine = new FieldCell[Field.FIELD_SIZE];
            for (Int32 i = 0, j = i + 1; i < estimatedNewLine.Length - 1; i++, j++)
            {
                estimatedNewLine[i] = field[1, j];
            }
            estimatedNewLine[estimatedNewLine.Length - 1] = field.freeFieldCell;

            //Подвинуть вверх вторую по порядку линию
            field.MoveLineUp(1);

            for (Int32 i = 0; i < estimatedNewLine.Length; i++)
            {
                Assert.Equal(estimatedNewLine[i], field[1, i]);
            }

            Assert.Equal(estimatedFreeCell, field.freeFieldCell);
        }
        /// <summary>
        /// Тест на удачный сдвиг влево второй по порядку линии.
        /// </summary>
        [Fact]
        public void TestMovement_MoveLineLeft_SuccessfullMovement()
        {
            CreateField();
            Field field = this.field.Clone();

            //Предполагаемая новая свободная ячейка
            FieldCell estimatedFreeCell = field[0, 1];
            //Предполагаемая новая линия.
            FieldCell[] estimatedNewLine = new FieldCell[Field.FIELD_SIZE];
            for (Int32 i = 0, j = i + 1; i < estimatedNewLine.Length - 1; i++, j++)
            {
                estimatedNewLine[i] = field[j, 1];
            }
            estimatedNewLine[estimatedNewLine.Length - 1] = field.freeFieldCell;

            //Подвинуть вверх вторую по порядку линию
            field.MoveLineLeft(1);

            for (Int32 i = 0; i < estimatedNewLine.Length; i++)
            {
                Assert.Equal(estimatedNewLine[i], field[i, 1]);
            }

            Assert.Equal(estimatedFreeCell, field.freeFieldCell);
        }
        /// <summary>
        /// Тест на удачный сдвиг вниз второй по порядку линии.
        /// </summary>
        [Fact]
        public void TestMovement_MoveLineDown_SuccessfullMovement()
        {
            CreateField();
            Field field = this.field.Clone();

            //Предполагаемая новая свободная ячейка
            FieldCell estimatedFreeCell = field[1, Field.FIELD_SIZE - 1];
            //Предполагаемая новая линия.
            FieldCell[] estimatedNewLine = new FieldCell[Field.FIELD_SIZE];
            for (Int32 i = estimatedNewLine.Length - 1, j = i - 1; i > 0; i--, j--)
            {
                estimatedNewLine[i] = field[1, j];
            }
            estimatedNewLine[0] = field.freeFieldCell;

            //Подвинуть вниз вторую по порядку линию
            field.MoveLineDown(1);

            for (Int32 i = 0; i < estimatedNewLine.Length; i++)
            {
                Assert.Equal(estimatedNewLine[i], field[1, i]);
            }

            Assert.Equal(estimatedFreeCell, field.freeFieldCell);
        }
        /// <summary>
        /// Тест на удачный сдвиг вправо второй по порядку линии.
        /// </summary>
        [Fact]
        public void TestMovement_MoveLineRight_SuccessfullMovement()
        {
            CreateField();
            Field field = this.field.Clone();

            //Предполагаемая новая свободная ячейка
            FieldCell estimatedFreeCell = field[Field.FIELD_SIZE - 1, 1];
            //Предполагаемая новая линия.
            FieldCell[] estimatedNewLine = new FieldCell[Field.FIELD_SIZE];
            for (Int32 i = estimatedNewLine.Length - 1, j = i - 1; i > 0; i--, j--)
            {
                estimatedNewLine[i] = field[j, 1];
            }
            estimatedNewLine[0] = field.freeFieldCell;

            //Подвинуть вправо вторую по порядку линию
            field.MoveLineRight(1);

            for (Int32 i = 0; i < estimatedNewLine.Length; i++)
            {
                Assert.Equal(estimatedNewLine[i], field[i, 1]);
            }

            Assert.Equal(estimatedFreeCell, field.freeFieldCell);
        }
        /// <summary>
        /// Тест на неудачный сдвиг всех нечетных по порядку линий.
        /// Т.к. нумерация с нуля, то все-таки четных.
        /// </summary>
        [Fact]
        public void TestMovement_SuccessfullMovement_SuccessfullMovement()
        {
            CreateField();
            Field field = this.field.Clone();

            for (Int32 i = 0; i < Field.FIELD_SIZE; i++)
            {
                if (i % 2 == 0)
                {
                    Assert.False(field.MoveLineUp(i));
                    Assert.False(field.MoveLineRight(i));
                    Assert.False(field.MoveLineDown(i));
                    Assert.False(field.MoveLineLeft(i));
                }
                else
                {
                    Assert.True(field.MoveLineUp(i));
                    Assert.True(field.MoveLineRight(i));
                    Assert.True(field.MoveLineDown(i));
                    Assert.True(field.MoveLineLeft(i));
                }
            }

        }

        #endregion Движение линий.

        #region Движение игроков вместе с линиями.

        /// <summary>
        /// Тест на удачный сдвиг игрока, при сдвиге вверх второй по порядку линии.
        /// </summary>
        [Fact]
        public void TestMovementPlayer_MoveLineUp_SuccessfullMovementPlayer()
        {
            CreateField();
            Field field = this.field.Clone();

            //выдать полю игроков
            GamePlayer[] players = new GamePlayer[]
            {
                new GamePlayer("1", Color.Red, new CardDeck(), 1, 0, 0),
                new GamePlayer("1", Color.Red, new CardDeck(), 1, 2, 0),
                new GamePlayer("1", Color.Red, new CardDeck(), 3, 0, 0),
                new GamePlayer("1", Color.Red, new CardDeck(), 3, 2, 0)
            };
            field.SetPlayers(players);


            //Подвинуть вверх вторую по порядку линию
            field.MoveLineUp(1);

            Assert.Equal(Field.FIELD_SIZE - 1, players[0].positionY);
            Assert.Equal(1, players[1].positionY);
            Assert.Equal(0, players[2].positionY);
            Assert.Equal(2, players[3].positionY);
        }
        /// <summary>
        /// Тест на удачный сдвиг игрока, при сдвиге вправо второй по порядку линии.
        /// </summary>
        [Fact]
        public void TestMovementPlayer_MoveLineRight_SuccessfullMovementPlayer()
        {
            CreateField();
            Field field = this.field.Clone();

            //выдать полю игроков
            GamePlayer[] players = new GamePlayer[]
            {
                new GamePlayer("1", Color.Red, new CardDeck(), Field.FIELD_SIZE-1, 1, 0),
                new GamePlayer("1", Color.Red, new CardDeck(), 2, 1, 0),
                new GamePlayer("1", Color.Red, new CardDeck(), 0, 3, 0),
                new GamePlayer("1", Color.Red, new CardDeck(), 2, 3, 0)
            };
            field.SetPlayers(players);


            //Подвинуть вправо вторую по порядку линию
            field.MoveLineRight(1);

            Assert.Equal(0, players[0].positionX);
            Assert.Equal(3, players[1].positionX);
            Assert.Equal(0, players[2].positionX);
            Assert.Equal(2, players[3].positionX);
        }
        /// <summary>
        /// Тест на удачный сдвиг игрока, при сдвиге вниз второй по порядку линии.
        /// </summary>
        [Fact]
        public void TestMovementPlayer_MoveLineDown_SuccessfullMovementPlayer()
        {
            CreateField();
            Field field = this.field.Clone();

            //выдать полю игроков
            GamePlayer[] players = new GamePlayer[]
            {
                new GamePlayer("1", Color.Red, new CardDeck(), 1, Field.FIELD_SIZE-1, 0),
                new GamePlayer("1", Color.Red, new CardDeck(), 1, 2, 0),
                new GamePlayer("1", Color.Red, new CardDeck(), 3, 0, 0),
                new GamePlayer("1", Color.Red, new CardDeck(), 3, 2, 0)
            };
            field.SetPlayers(players);


            //Подвинуть вниз вторую по порядку линию
            field.MoveLineDown(1);

            Assert.Equal(0, players[0].positionY);
            Assert.Equal(3, players[1].positionY);
            Assert.Equal(0, players[2].positionY);
            Assert.Equal(2, players[3].positionY);
        }
        /// <summary>
        /// Тест на удачный сдвиг игрока, при сдвиге влево второй по порядку линии.
        /// </summary>
        [Fact]
        public void TestMovementPlayer_MoveLineLeft_SuccessfullMovementPlayer()
        {
            CreateField();
            Field field = this.field.Clone();

            //выдать полю игроков
            GamePlayer[] players = new GamePlayer[]
            {
                new GamePlayer("1", Color.Red, new CardDeck(), 0, 1, 0),
                new GamePlayer("1", Color.Red, new CardDeck(), 2, 1, 0),
                new GamePlayer("1", Color.Red, new CardDeck(), 0, 3, 0),
                new GamePlayer("1", Color.Red, new CardDeck(), 2, 3, 0)
            };
            field.SetPlayers(players);


            //Подвинуть влево вторую по порядку линию
            field.MoveLineLeft(1);

            Assert.Equal(Field.FIELD_SIZE - 1, players[0].positionX);
            Assert.Equal(1, players[1].positionX);
            Assert.Equal(0, players[2].positionX);
            Assert.Equal(2, players[3].positionX);
        }

        #endregion Движение игроков вместе с линиями.
    }
}
