using Assets.Scripts.GameModel.PlayingField.FieldCells;
using Assets.Scripts.GameModel.PlayingField.FieldCells.SpecificFieldCells;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.GameModel.PlayingField
{
    /// <summary>
    /// Игровое поле.
    /// </summary>
    public class PlayingField
    {
        #region Данные игрового поля.

        /// <summary>
        /// Размер игрового поля.
        /// </summary>
        public const Int32 fieldSize = 7;
        /// <summary>
        /// Все игровое поле.
        /// </summary>
        public FieldCell[,] fieldCells = null;
        /// <summary>
        /// Свободная ячейка.
        /// </summary>
        public FieldCell freeFieldCell = null;

        #endregion Данные игрового поля.

        public PlayingField()
        {
            this.fieldCells = new FieldCell[fieldSize, fieldSize];
            CreateField();
        }

        #region Создание ячеек на поле.

        /// <summary>
        /// Создание ячеек-уголков.
        /// </summary>
        /// <param name="count">Количество.</param>
        /// <returns></returns>
        private CornerTwoDirectionFieldCell[] CreateCornerTwoDirectionFieldCells(Int32 count)
        {
            CornerTwoDirectionFieldCell[] cells = new CornerTwoDirectionFieldCell[count];
            for (int i = 0; i < count; i++)
            {
                cells[i] = new CornerTwoDirectionFieldCell();
            }

            return cells;
        }
        /// <summary>
        /// Создание ячеек-линий.
        /// </summary>
        /// <param name="count">Количество.</param>
        /// <returns></returns>
        private LineTwoDirectionFieldCell[] CreateLineTwoDirectionFieldCells(Int32 count)
        {
            LineTwoDirectionFieldCell[] cells = new LineTwoDirectionFieldCell[count];
            for (int i = 0; i < count; i++)
            {
                cells[i] = new LineTwoDirectionFieldCell();
            }

            return cells;
        }
        /// <summary>
        /// Создание ячеек с тремя проходами.
        /// </summary>
        /// <param name="count">Количество.</param>
        /// <returns></returns>
        private ThreeDirectionFieldCell[] CreateThreeDirectionFieldCells(Int32 count)
        {
            ThreeDirectionFieldCell[] cells = new ThreeDirectionFieldCell[count];
            for (int i = 0; i < count; i++)
            {
                cells[i] = new ThreeDirectionFieldCell();
            }

            return cells;
        }
        /// <summary>
        /// Создание ячейки по типу. Для типа "не определено" выбирается тип уголка.
        /// </summary>
        /// <param name="type">Тип ячейки.</param>
        /// <param name="count">Количество ячеек.</param>
        /// <returns></returns>
        private FieldCell[] CreateFieldCells(CellType type, Int32 count)
        {
            switch(type)
            {
                case CellType.corner: return CreateCornerTwoDirectionFieldCells(count);
                case CellType.line: return CreateLineTwoDirectionFieldCells(count);
                case CellType.threeDirection: return CreateThreeDirectionFieldCells(count);

                    default: return CreateCornerTwoDirectionFieldCells(count);
            }
        }

        /// <summary>
        /// Создание закрепленных ячеек.
        /// </summary>
        /// <returns></returns>
        private void CreatePinnedFieldCells()
        {
            #region Создание угловых ячеек

            FieldCell[] cornerFieldCells = CreateFieldCells(CellType.corner, 4);
            //Верхняя левая
            cornerFieldCells[0].TurnClockwise(1);
            this.fieldCells[0, 0] = cornerFieldCells[0];
            //Верхняя правая
            cornerFieldCells[1].TurnClockwise(2);
            this.fieldCells[0, fieldSize-1] = cornerFieldCells[1];
            //Нижняя правая
            cornerFieldCells[2].TurnClockwise(3);
            this.fieldCells[fieldSize - 1, fieldSize - 1] = cornerFieldCells[2];
            //Нижняя левая
            this.fieldCells[fieldSize - 1, 0] = cornerFieldCells[3];

            #endregion Создание угловых ячеек

            #region Создание ячеек по краям

            FieldCell[] borderFieldCells = CreateFieldCells(CellType.threeDirection, 8);

            //Две верхние
            borderFieldCells[0].TurnClockwise(1);
            borderFieldCells[1].TurnClockwise(1);
            this.fieldCells[0, 2] = borderFieldCells[0];
            this.fieldCells[0, 4] = borderFieldCells[1];
            //Две правые
            borderFieldCells[2].TurnClockwise(2);
            borderFieldCells[3].TurnClockwise(2);
            this.fieldCells[2, fieldSize - 1] = borderFieldCells[2];
            this.fieldCells[4, fieldSize - 1] = borderFieldCells[3];
            //Две нижние
            borderFieldCells[4].TurnClockwise(3);
            borderFieldCells[5].TurnClockwise(3);
            this.fieldCells[fieldSize - 1, 2] = borderFieldCells[4];
            this.fieldCells[fieldSize - 1, 4] = borderFieldCells[5];
            //Две левые
            this.fieldCells[2, 0] = borderFieldCells[6];
            this.fieldCells[4, 0] = borderFieldCells[7];

            #endregion Создание ячеек по краям

            #region Создание центральных ячеек

            FieldCell[] centerFieldCells = CreateFieldCells(CellType.threeDirection, 4);

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
        /// Перемешать.
        /// <br/>Тасование Фишера-Йетса.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        private void Shuffle(List<FieldCell> list)
        {
            Random rand = new Random();

            for (int i = list.Count - 1; i >= 1; i--)
            {
                int j = rand.Next(i + 1);

                (list[j], list[i]) = (list[i], list[j]);
            }
        }
        /// <summary>
        /// Создание двигающихся ячеек.
        /// </summary>
        /// <returns></returns>
        private void CreateMovingFieldCells()
        {
            //Заполнения списка нужным количеством клеток каждого типа.
            List<FieldCell> fieldCellsList = new List<FieldCell>(CreateFieldCells(CellType.corner, 16));
            fieldCellsList.AddRange(CreateFieldCells(CellType.line, 12));
            fieldCellsList.AddRange(CreateFieldCells(CellType.threeDirection, 6));
            //Перемешывание списка и запись его в стэк
            Shuffle(fieldCellsList);
            Stack<FieldCell> fieldCellsStack = new Stack<FieldCell>(fieldCellsList);

            Random random = new Random();
            for (Int32 i = 0; i < fieldSize; i++)
            {
                for (Int32 j = 0; j < fieldSize; j++)
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
        /// <summary>
        /// Создание игрового поля.
        /// </summary>
        private void CreateField()
        {
            CreatePinnedFieldCells();
            CreateMovingFieldCells();

            //Ячейки поля не должны изменяться.
            foreach(FieldCell cell in this.fieldCells)
                cell.isInteractable = false;
        }

        #endregion Создание ячеек на поле.

        #region Клонирование.

        /// <summary>
        /// Создание глубокого клона игрового поля.
        /// </summary>
        /// <returns></returns>
        public PlayingField Clone()
        {
            PlayingField fieldClone = new PlayingField();

            for (Int32 i = 0; i < fieldSize; i++)
            {
                for (Int32 j = 0; j < fieldSize; j++)
                {
                    fieldClone.fieldCells[i, j] = this.fieldCells[i, j].Clone();
                }
            }
            fieldClone.freeFieldCell = this.freeFieldCell.Clone();

            return fieldClone;
        }

        #endregion Клонирование.
    }
}
