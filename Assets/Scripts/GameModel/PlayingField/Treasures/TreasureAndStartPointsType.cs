using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.GameModel.PlayingField.Treasures
{
    /// <summary>
    /// Тип сокровища. 
    /// <br/>В этот тип так же входят стартовые точки,
    /// т.к. они будут использоваться в ячейках как и обычные сокровища.
    /// <br/>0: неопознаный тип.
    /// <br/>1: пустая ячейка.
    /// <br/>2-5: стартовые точки.
    /// <br/>6-17: закрепленная клетка.
    /// <br/>18-29: подвижная клетка.
    /// </summary>
    public enum TreasureAndStartPointsType : byte
    {
        /// <summary>
        /// Неизвестно.
        /// </summary>
        unknown = 0,
        /// <summary>
        /// Ячейка пуста.
        /// </summary>
        empty = 1,
        /// <summary>
        /// Стартовая точка.
        /// </summary>
        startPoint1 = 2,
        /// <summary>
        /// Стартовая точка.
        /// </summary>
        startPoint2 = 3,
        /// <summary>
        /// Стартовая точка.
        /// </summary>
        startPoint3 = 4,
        /// <summary>
        /// Стартовая точка.
        /// </summary>
        startPoint4 = 5,
        /// <summary>
        /// Закрепленная ячейка.
        /// </summary>
        treasureOnPinnedCell1 = 6,
        /// <summary>
        /// Закрепленная ячейка.
        /// </summary>
        treasureOnPinnedCell2 = 7,
        /// <summary>
        /// Закрепленная ячейка.
        /// </summary>
        treasureOnPinnedCell3 = 8,
        /// <summary>
        /// Закрепленная ячейка.
        /// </summary>
        treasureOnPinnedCell4 = 9,
        /// <summary>
        /// Закрепленная ячейка.
        /// </summary>
        treasureOnPinnedCell5 = 10,
        /// <summary>
        /// Закрепленная ячейка.
        /// </summary>
        treasureOnPinnedCell6 = 11,
        /// <summary>
        /// Закрепленная ячейка.
        /// </summary>
        treasureOnPinnedCell7 = 12,
        /// <summary>
        /// Закрепленная ячейка.
        /// </summary>
        treasureOnPinnedCell8 = 13,
        /// <summary>
        /// Закрепленная ячейка.
        /// </summary>
        treasureOnPinnedCell9 = 14,
        /// <summary>
        /// Закрепленная ячейка.
        /// </summary>
        treasureOnPinnedCell10 = 15,
        /// <summary>
        /// Закрепленная ячейка.
        /// </summary>
        treasureOnPinnedCell11 = 16,
        /// <summary>
        /// Закрепленная ячейка.
        /// </summary>
        treasureOnPinnedCell12 = 17,
        /// <summary>
        /// Подвижная ячейка.
        /// </summary>
        treasureOnMovingCell1 = 18,
        /// <summary>
        /// Подвижная ячейка.
        /// </summary>
        treasureOnMovingCell2 = 19,
        /// <summary>
        /// Подвижная ячейка.
        /// </summary>
        treasureOnMovingCell3 = 20,
        /// <summary>
        /// Подвижная ячейка.
        /// </summary>
        treasureOnMovingCell4 = 21,
        /// <summary>
        /// Подвижная ячейка.
        /// </summary>
        treasureOnMovingCell5 = 22,
        /// <summary>
        /// Подвижная ячейка.
        /// </summary>
        treasureOnMovingCell6 = 23,
        /// <summary>
        /// Подвижная ячейка.
        /// </summary>
        treasureOnMovingCell7 = 24,
        /// <summary>
        /// Подвижная ячейка.
        /// </summary>
        treasureOnMovingCell8 = 25,
        /// <summary>
        /// Подвижная ячейка.
        /// </summary>
        treasureOnMovingCell9 = 26,
        /// <summary>
        /// Подвижная ячейка.
        /// </summary>
        treasureOnMovingCell10 = 27,
        /// <summary>
        /// Подвижная ячейка.
        /// </summary>
        treasureOnMovingCell11 = 28,
        /// <summary>
        /// Подвижная ячейка.
        /// </summary>
        treasureOnMovingCell12 = 29

    }
}
