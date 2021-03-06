using Assets.Scripts.GameModel.PlayingField.FieldCells;
using Assets.Scripts.GameModel.PlayingField.Treasures;
using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Extensions;
using System.Drawing;
using Assets.Scripts.GameModel.Player;
using Assets.Scripts.GameModel.Rules;
using Assets.Scripts.GameModel.Logging;

namespace Assets.Scripts.GameModel.PlayingField
{
    /// <summary>
    /// Игровое поле.
    /// Для обращения к одной ячейке у класса есть индкесатор.
    /// Для перебора всех ячеек у класса есть итератор.
    /// </summary>
    public class Field : IEnumerable<FieldCell>
    {
        #region Данные игрового поля.

        /// <summary>
        /// Размер игрового поля.
        /// </summary>
        public const Int32 FIELD_SIZE = 7;
        /// <summary>
        /// Все игровое поле.
        /// </summary>
        private FieldCell[,] fieldCells = null;
        /// <summary>
        /// Прямое обращение к ячейке игрового поля по координатам.
        /// </summary>
        /// <param name="hIndex"></param>
        /// <param name="vIndex"></param>
        /// <returns></returns>
        public FieldCell this[Int32 hIndex, Int32 vIndex]
        {
            get => this.fieldCells[hIndex, vIndex];
        }
        private Int32 seedForShuffle;

        #region Свободная ячейка.

        /// <summary>
        /// Событие происходит при смене свободной ячейки.
        /// </summary>
        public event Action OnFreeCellChange;
        /// <summary>
        /// Свободная ячейка.
        /// </summary>
        private FieldCell freeFieldCellPrivate = null;
        /// <summary>
        /// Свободная ячейка.
        /// </summary>
        public FieldCell freeFieldCell
        {
            get
            {
                return this.freeFieldCellPrivate;
            }
            set
            {
                this.freeFieldCellPrivate = value;
                this.OnFreeCellChange?.Invoke();
            }
        }
        /// <summary>
        /// Количество поворотов свободной ячейки до свершения текущего поля.
        /// (В начале игры или после передвижения последнего аватара.)
        /// </summary>
        private Int32 turnClockwiseFreeCellBeforeMovePrivate;
        /// <summary>
        /// Количество поворотов свободной ячейки до свершения текущего поля.
        /// (В начале игры или после передвижения последнего аватара.)
        /// </summary>
        public Int32 turnClockwiseFreeCellBeforeMove
        {
            get => this.turnClockwiseFreeCellBeforeMovePrivate;
        }


        #endregion Свободная ячейка.


        /// <summary>
        /// Координаты стартовых точек.
        /// </summary>
        public readonly Point[] startPointsCoordinate = new Point[4]
        {
            new Point(0, 0),
            new Point(0, FIELD_SIZE - 1),
            new Point(FIELD_SIZE - 1, FIELD_SIZE - 1),
            new Point(FIELD_SIZE - 1, 0)
        };
        /// <summary>
        /// Список игроков.
        /// </summary>
        private GamePlayer[] players = null;
        /// <summary>
        /// Выдать полю информацию об игроках.
        /// <br/>Теперь они смогут двигаться вместе с ячейками.
        /// </summary>
        /// <param name="players">Информацию об игроках.</param>
        public void SetPlayers(GamePlayer[] players)
        {
            this.players = players;
        }

        #endregion Данные игрового поля.

        /// <summary>
        /// 
        /// </summary>
        public Field(Int32 seedForShuffle)
        {
            this.fieldCells = new FieldCell[FIELD_SIZE, FIELD_SIZE];
            this.seedForShuffle = seedForShuffle;
            CreateField();
        }

        #region Поиск пути.

        /// <summary>
        /// Класс для поиска пути.
        /// </summary>
        private SearchRoadForAvatar searchRoadForPlayer = new SearchRoadForAvatar();
        /// <summary>
        /// Координаты ячеек игрового поля, куда игрок может совершить ход.
        /// </summary>
        private HashSet<Point> pointsForMove = null;
        /// <summary>
        /// Игрок может совершить ход в указанную ячейку.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="cellForMove"></param>
        /// <returns></returns>
        private Boolean IsPlayerCanMoveToCell(GamePlayer player, Point cellForMove)
        {
            if (this.pointsForMove == null)
            {
                this.pointsForMove = this.searchRoadForPlayer.GetCellsForMove(new Point(player.positionX, player.positionY), this.fieldCells);
            }

            return this.pointsForMove.Contains(cellForMove);
        }
        /// <summary>
        /// Получить все координаты ячеек игрового поля, куда игрок может совершить ход.
        /// </summary>
        /// <param name="player">Игрок, для которого надо показать ячейки, куда можно ходить.</param>
        /// <returns></returns>
        public HashSet<Point> GetPointsForMove(GamePlayer player)
        {
            return this.searchRoadForPlayer.GetCellsForMove(new Point(player.positionX, player.positionY), this.fieldCells);
        }
        /// <summary>
        /// Занулить объект с координатами ячеек игрового поля, куда игрок может совершить ход.
        /// </summary>
        public void ClearCellForMove()
        {
            this.pointsForMove= null;
        }

        #endregion Поиск пути.

        #region Создание ячеек на поле.

        #region Создание ячеек определенного типа.

        /// <summary>
        /// Создание ячеек по типу. Для типа "не определено" выбирается тип уголка.
        /// </summary>
        /// <param name="type">Тип ячейки.</param>
        /// <param name="count">Количество ячеек.</param>
        /// <returns></returns>
        private FieldCell[] CreateFieldCells(CellType type, Int32 count)
        {
            FieldCell[] cells = new FieldCell[count];
            for (int i = 0; i < count; i++)
            {
                cells[i] = new FieldCell(type);
            }
            return cells;
        }
        /// <summary>
        /// Создание ячеек с сокровищами или стартовыми точками.
        /// </summary>
        /// <param name="type">Тип ячейки.</param>
        /// <param name="startNumber">Начальный порядковый номер в типе сокровищ.</param>
        /// <param name="endNumber">Конечный порядковый номер в типе сокровищ.</param>
        /// <returns></returns>
        private FieldCell[] CreateFieldCellsWithTreasureAndStartPointsType(CellType type, Int32 startNumber, Int32 endNumber)
        {
            Int32 count = endNumber - startNumber + 1;
            FieldCell[] cells = new FieldCell[count];
            for (int i = startNumber, j = 0; i <= endNumber; i++, j++)
            {
                cells[j] = new FieldCell(type, (TreasureAndStartPointsType)i);
            }
            return cells;
        }


        #endregion Создание ячеек определенного типа.

        #region Создание и размещение специальных ячеек для игрового поля.

        /// <summary>
        /// Создание закрепленных ячеек.
        /// </summary>
        /// <returns></returns>
        private void CreatePinnedFieldCells()
        {
            #region Создание угловых ячеек

            FieldCell[] cornerFieldCells = CreateFieldCellsWithTreasureAndStartPointsType(CellType.corner, 2, 5);
            //Верхняя левая
            cornerFieldCells[0].TurnClockwise(1);
            this.fieldCells[this.startPointsCoordinate[0].X, this.startPointsCoordinate[0].Y] = cornerFieldCells[0];
            //Верхняя правая
            cornerFieldCells[1].TurnClockwise(2);
            this.fieldCells[this.startPointsCoordinate[1].X, this.startPointsCoordinate[1].Y] = cornerFieldCells[1];
            //Нижняя правая
            cornerFieldCells[2].TurnClockwise(3);
            this.fieldCells[this.startPointsCoordinate[2].X, this.startPointsCoordinate[2].Y] = cornerFieldCells[2];
            //Нижняя левая
            this.fieldCells[this.startPointsCoordinate[3].X, this.startPointsCoordinate[3].Y] = cornerFieldCells[3];

            #endregion Создание угловых ячеек

            #region Создание ячеек по краям

            FieldCell[] borderFieldCells = CreateFieldCellsWithTreasureAndStartPointsType(CellType.threeDirection, 6, 13);

            //Две верхние
            borderFieldCells[0].TurnClockwise(1);
            borderFieldCells[1].TurnClockwise(1);
            this.fieldCells[0, 2] = borderFieldCells[0];
            this.fieldCells[0, 4] = borderFieldCells[1];
            //Две правые
            borderFieldCells[2].TurnClockwise(2);
            borderFieldCells[3].TurnClockwise(2);
            this.fieldCells[2, FIELD_SIZE - 1] = borderFieldCells[2];
            this.fieldCells[4, FIELD_SIZE - 1] = borderFieldCells[3];
            //Две нижние
            borderFieldCells[4].TurnClockwise(3);
            borderFieldCells[5].TurnClockwise(3);
            this.fieldCells[FIELD_SIZE - 1, 2] = borderFieldCells[4];
            this.fieldCells[FIELD_SIZE - 1, 4] = borderFieldCells[5];
            //Две левые
            this.fieldCells[2, 0] = borderFieldCells[6];
            this.fieldCells[4, 0] = borderFieldCells[7];

            #endregion Создание ячеек по краям

            #region Создание центральных ячеек

            FieldCell[] centerFieldCells = CreateFieldCellsWithTreasureAndStartPointsType(CellType.threeDirection, 14, 17);

            //Верхняя левая
            this.fieldCells[2, 2] = centerFieldCells[0];
            //Верхняя правая
            centerFieldCells[1].TurnClockwise(1);
            this.fieldCells[2, 4] = centerFieldCells[1];
            //Нижняя правая
            centerFieldCells[2].TurnClockwise(2);
            this.fieldCells[4, 4] = centerFieldCells[2];
            //Нижняя левая
            centerFieldCells[3].TurnClockwise(3);
            this.fieldCells[4, 2] = centerFieldCells[3];

            #endregion Создание центральных ячеек

        }
        /// <summary>
        /// Создание двигающихся ячеек.
        /// </summary>
        /// <returns></returns>
        private void CreateMovingFieldCells()
        {
            //Заполнения списка нужным количеством клеток каждого типа.
            List<FieldCell> fieldCellsList = new List<FieldCell>(CreateFieldCells(CellType.corner, 10));
            fieldCellsList.AddRange(CreateFieldCellsWithTreasureAndStartPointsType(CellType.corner, 18, 23));
            fieldCellsList.AddRange(CreateFieldCells(CellType.line, 12));
            fieldCellsList.AddRange(CreateFieldCellsWithTreasureAndStartPointsType(CellType.threeDirection, 24, 29));
            //Перемешывание списка и запись его в стэк
            fieldCellsList.Shuffle(this.seedForShuffle);
            Stack<FieldCell> fieldCellsStack = new Stack<FieldCell>(fieldCellsList);

            Random random = new Random(this.seedForShuffle);
            for (Int32 i = 0; i < FIELD_SIZE; i++)
            {
                for (Int32 j = 0; j < FIELD_SIZE; j++)
                {
                    if (this.fieldCells[i, j] == null)
                    {
                        if (this.fieldCells[i, j] == null)
                        {
                            this.fieldCells[i, j] = fieldCellsStack.Pop();

                            //случайным образом ее повернуть
                            Int32 choice = random.Next(0, 99) % 4;
                            this.fieldCells[i, j].TurnClockwise(choice);
                        }
                    }
                }
            }

            //Пометить последнюю оставшуюся ячейку как свободную
            this.freeFieldCell = fieldCellsStack.Pop();
        }

        #endregion Создание и размещение специальных ячеек для игрового поля.

        /// <summary>
        /// Создание игрового поля.
        /// </summary>
        private void CreateField()
        {
            CreatePinnedFieldCells();
            CreateMovingFieldCells();

            //Ячейки поля не должны изменяться.
            foreach (FieldCell cell in this.fieldCells)
                cell.isInteractable = false;

            this.turnClockwiseFreeCellBeforeMovePrivate = this.freeFieldCell.turnsClockwiseCount;
        }

        #endregion Создание ячеек на поле.

        #region Передвижение ячеек.

        /// <summary>
        /// Игрок должен быть перемещен вместе с ячейками.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="numberLine"></param>
        /// <param name="isVerical"></param>
        /// <param name="isForward"></param>
        /// <returns></returns>
        private void MovePlayer(GamePlayer player, Int32 numberLine, Boolean isVerical, Boolean isForward)
        {
            if (isVerical)
            {
                if (numberLine == player.positionX)
                {
                    //Игрок сам следит за "выходом" за пределы поля и 
                    // пересталяет на другую его сторону свою фишку, если надо.
                    if (isForward)
                        player.SetPosition(player.positionX, player.positionY + 1);
                    else
                        player.SetPosition(player.positionX, player.positionY - 1);
                }
            }
            else
            {
                if (numberLine == player.positionY)
                {
                    //Игрок сам следит за "выходом" за пределы поля и 
                    // пересталяет на другую его сторону свою фишку, если надо.
                    if (isForward)
                        player.SetPosition(player.positionX + 1, player.positionY);
                    else
                        player.SetPosition(player.positionX - 1, player.positionY);
                }
            }
        }
        /// <summary>
        /// Игроки должны быть перемещены вместе с ячейками.
        /// </summary>
        /// <param name="numberLine"></param>
        /// <param name="isVerical"></param>
        /// <param name="isForward"></param>
        /// <returns></returns>
        private void MovePlayers(Int32 numberLine, Boolean isVerical, Boolean isForward)
        {
            foreach (GamePlayer player in this.players)
            {
                MovePlayer(player, numberLine, isVerical, isForward);
            }
        }

        /// <summary>
        /// Сдвиг линии по указанному номеру в указанном направлении. 
        /// Номер соответствует положению линии в массиве, т.е. начинается с 0.
        /// </summary>
        /// <param name="numberLine">Номер линии.</param>
        /// <param name="isVerical">Выполнить ли сдвиг по вертикали.</param>
        /// <param name="isForward">Выполнить ли сдвиг вперед,
        /// т.е. от меньшего порядкого номера к большему.
        /// <br/>Если да, то ячейка 0 встанет на меесто ячейки 1,
        /// ячейка 1 встанет на место ячейки 2, и т.д.</param>
        private Boolean MoveLine(Int32 numberLine, Boolean isVerical, Boolean isForward)
        {
            if (this.players != null)
            {
                MovePlayers(numberLine, isVerical, isForward);
            }
            else
            {
                GameModelLogger.LogError(nameof(this.players) + " in " + nameof(Field) + " is null reference!");
            }

            //Двигать можно только четные(нечетные. т.к. нумерация идет с 0) линии.
            if (numberLine % 2 != 0)
            {
                //Выпавшая ячейка
                FieldCell plowingCell;
                FieldCell[,] field = this.fieldCells;
                this.freeFieldCell.isInteractable = false;
                if (isVerical)
                {
                    if (isForward)
                    {
                        plowingCell = field[numberLine, field.GetLength(1) - 1];
                        for (Int32 i = field.GetLength(1) - 1; i > 0; i--)
                        {
                            field[numberLine, i] = field[numberLine, i - 1];
                        }
                        field[numberLine, 0] = this.freeFieldCell;
                    }
                    else
                    {
                        plowingCell = field[numberLine, 0];
                        for (Int32 i = 0; i < field.GetLength(1) - 1; i++)
                        {
                            field[numberLine, i] = field[numberLine, i + 1];
                        }
                        field[numberLine, field.GetLength(1) - 1] = this.freeFieldCell;
                    }
                }
                else
                {
                    if (isForward)
                    {
                        plowingCell = field[field.GetLength(0) - 1, numberLine];
                        for (Int32 i = field.GetLength(0) - 1; i > 0; i--)
                        {
                            field[i, numberLine] = field[i - 1, numberLine];
                        }
                        field[0, numberLine] = this.freeFieldCell;
                    }
                    else
                    {
                        plowingCell = field[0, numberLine];
                        for (Int32 i = 0; i < field.GetLength(0) - 1; i++)
                        {
                            field[i, numberLine] = field[i + 1, numberLine];
                        }
                        field[field.GetLength(0) - 1, numberLine] = this.freeFieldCell;
                    }
                }

                this.freeFieldCell = plowingCell;
                this.freeFieldCell.isInteractable = true;
                this.turnClockwiseFreeCellBeforeMovePrivate = this.freeFieldCell.turnsClockwiseCount;

                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Сдвиг вверх линии по указанному номеру. 
        /// Номер соответствует положению линии в массиве, т.е. начинается с 0.
        /// </summary>
        /// <param name="numberLine">Номер линии.</param>
        public Boolean MoveLineUp(Int32 numberLine)
        {
            return MoveLine(numberLine, true, false);
        }
        /// <summary>
        /// Сдвиг вниз линии по указанному номеру. 
        /// Номер соответствует положению линии в массиве, т.е. начинается с 0.
        /// </summary>
        /// <param name="numberLine">Номер линии.</param>
        public Boolean MoveLineDown(Int32 numberLine)
        {
            return MoveLine(numberLine, true, true);
        }
        /// <summary>
        /// Сдвиг вправо линии по указанному номеру. 
        /// Номер соответствует положению линии в массиве, т.е. начинается с 0.
        /// </summary>
        /// <param name="numberLine">Номер линии.</param>
        public Boolean MoveLineRight(Int32 numberLine)
        {
            return MoveLine(numberLine, false, true);
        }
        /// <summary>
        /// Сдвиг влево линии по указанному номеру. 
        /// Номер соответствует положению линии в массиве, т.е. начинается с 0.
        /// </summary>
        /// <param name="numberLine">Номер линии.</param>
        public Boolean MoveLineLeft(Int32 numberLine)
        {
            return MoveLine(numberLine, false, false);
        }

        #endregion Передвижение ячеек.

        #region Клонирование.

        /// <summary>
        /// Создание глубокого клона игрового поля.
        /// </summary>
        /// <returns></returns>
        public Field Clone()
        {

            Field fieldClone = new Field(this.seedForShuffle);

            if (this.players != null)
            {
                GamePlayer[] playersClone = new GamePlayer[this.players.Length];
                for (int i = 0; i < playersClone.Length; i++)
                {
                    playersClone[i] = this.players[i].Clone();
                }
                fieldClone.SetPlayers(playersClone);
            }

            for (Int32 i = 0; i < FIELD_SIZE; i++)
            {
                for (Int32 j = 0; j < FIELD_SIZE; j++)
                {
                    fieldClone.fieldCells[i, j] = this.fieldCells[i, j].Clone();
                }
            }
            fieldClone.freeFieldCell = this.freeFieldCell.Clone();

            return fieldClone;
        }

        #endregion Клонирование.

        #region IEnumerator.

        public IEnumerator<FieldCell> GetEnumerator()
        {
            foreach (FieldCell cell in this.fieldCells)
            {
                yield return cell;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion IEnumerator.

        /// <summary>
        /// Возможен ли ход.
        /// </summary>
        /// <param name="startX"></param>
        /// <param name="startY"></param>
        /// <param name="endX"></param>
        /// <param name="endY"></param>
        /// <returns></returns>
        public Boolean IsPossibleMove(GamePlayer player, Int32 startX, Int32 startY, Int32 endX, Int32 endY)
        {
            //Проверить, что все координаты оказались внутрии поля.
            Boolean isWithinField = startX > -1 && startY > -1 && endX > -1 && endY > -1;
            isWithinField &= startX < Field.FIELD_SIZE && startY < Field.FIELD_SIZE && endX < Field.FIELD_SIZE && endY < Field.FIELD_SIZE;
            if (!isWithinField)
            {
                return false;
            }

            if (IsPlayerCanMoveToCell(player, new Point(endX, endY)))
            {
                return true;
            }

            return false;
        }

        #region Сравнение.

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj is Field field)
            {
                if (field.freeFieldCellPrivate != this.freeFieldCellPrivate)
                {
                    return false;
                }
                if (field.seedForShuffle != this.seedForShuffle)
                {
                    return false;
                }
                for (Int32 i = 0; i < Field.FIELD_SIZE; i++)
                    for (Int32 j = 0; j < Field.FIELD_SIZE; j++)
                    {
                        if (field.fieldCells[i, j] != this.fieldCells[i, j])
                        {
                            return false;
                        }
                    }
                for (Int32 i = 0; i < this.players.Length; i++)
                {
                    if (this.players[i] != field.players[i])
                    {
                        return false;
                    }
                }


                        return true;
            }

            return false;
        }
        public override Int32 GetHashCode()
        {
            Int32 hashCode = 0;
            hashCode ^= this.freeFieldCellPrivate.GetHashCode();
            hashCode ^= this.seedForShuffle.GetHashCode();
            for (Int32 i = 0; i < Field.FIELD_SIZE; i++)
                for (Int32 j = 0; j < Field.FIELD_SIZE; j++)
                {
                    hashCode ^= this.fieldCells[i, j].GetHashCode();
                }
            for (Int32 i = 0; i < this.players.Length; i++)
            {
                hashCode ^= this.players[i].GetHashCode();
            }

            return hashCode;
        }
        public static bool operator ==(Field l, Field r)
        {
            if (l is null && r is null)
                return true;
            else if (l is null)
                return false;
            else
                return l.Equals(r);
        }
        public static bool operator !=(Field l, Field r)
        {
            return !(l == r);
        }

        #endregion Сравнение.
    }
}
