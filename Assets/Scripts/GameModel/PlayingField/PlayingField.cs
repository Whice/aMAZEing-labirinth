using Assets.Scripts.GameModel.PlayingField.FieldCells;
using Assets.Scripts.GameModel.PlayingField.FieldCells.SpecificFieldCells;
using System;

namespace Assets.Scripts.GameModel.PlayingField
{
    /// <summary>
    /// Игровое поле.
    /// </summary>
    public class PlayingField
    {
        /// <summary>
        /// Размер игрового поля.
        /// </summary>
        public Int32 fieldSize = 7;
        /// <summary>
        /// Все игровое поле.
        /// </summary>
        public FieldCell[,] fieldCells = null;
        /// <summary>
        /// Свободная ячейка.
        /// </summary>
        public FieldCell freeFieldCell = null;
        public PlayingField()
        {
            this.fieldCells = new FieldCell[this.fieldSize, fieldSize];
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
        /// Создание двигающихся ячеек.
        /// </summary>
        /// <returns></returns>
        private FieldCell[] CreateMovingFieldCells()
        {
            
            FieldCell[] fieldCells = new FieldCell[fieldSize*fieldSize];

            return fieldCells;

        }
        /// <summary>
        /// Создание закрепленных ячеек.
        /// </summary>
        /// <returns></returns>
        private void CreatePinnedFieldCells()
        {
            #region Создание угловых ячеек

            FieldCell[] cornerFieldCells = CreateCornerTwoDirectionFieldCells(4);
            //Верхняя левая
            cornerFieldCells[0].TurnClockwise(1);
            this.fieldCells[0, 0] = cornerFieldCells[0];
            //Верхняя правая
            cornerFieldCells[1].TurnClockwise(2);
            this.fieldCells[0, this.fieldSize-1] = cornerFieldCells[1];
            //Нижняя правая
            cornerFieldCells[2].TurnClockwise(3);
            this.fieldCells[this.fieldSize - 1, 0] = cornerFieldCells[2];
            //Нижняя левая
            this.fieldCells[this.fieldSize - 1, this.fieldSize - 1] = cornerFieldCells[3];

            #endregion Создание угловых ячеек

            #region Создание ячеек по краям

            FieldCell[] borderFieldCells = CreateThreeDirectionFieldCells(8);

            //Две верхние
            borderFieldCells[0].TurnClockwise(1);
            borderFieldCells[1].TurnClockwise(1);
            this.fieldCells[0, 2] = borderFieldCells[0];
            this.fieldCells[0, 4] = borderFieldCells[1];
            //Две правые
            borderFieldCells[2].TurnClockwise(2);
            borderFieldCells[3].TurnClockwise(2);
            this.fieldCells[2, this.fieldSize - 1] = borderFieldCells[2];
            this.fieldCells[4, this.fieldSize - 1] = borderFieldCells[3];
            //Две нижние
            borderFieldCells[4].TurnClockwise(3);
            borderFieldCells[5].TurnClockwise(3);
            this.fieldCells[this.fieldSize - 1, 2] = borderFieldCells[4];
            this.fieldCells[this.fieldSize - 1, 4] = borderFieldCells[5];
            //Две левые
            this.fieldCells[2, 0] = borderFieldCells[6];
            this.fieldCells[4, 0] = borderFieldCells[7];

            #endregion Создание ячеек по краям

            #region Создание центральных ячеек

            FieldCell[] centerFieldCells = CreateThreeDirectionFieldCells(4);

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
        private void CreateField()
        {
            CreatePinnedFieldCells();
        }

        #endregion Создание ячеек на поле.
    }
}
