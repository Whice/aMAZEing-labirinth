using System;

namespace Assets.Scripts.GameModel.PlayingField.Treasures
{
    public static class TreasureAndStartPointTypeExtensions
    {
        public static Int32 GetMinimalNumberTreasure(this TreasureAndStartPointsType type)
        {
            return 6;
        }
        public static Int32 GetMaximalNumberTreasure(this TreasureAndStartPointsType type)
        {
            return 29;
        }
        public static Int32 GetMinimalNumberStartPoint(this TreasureAndStartPointsType type)
        {
            return 2;
        }
        public static Int32 GetMaximalNumberStartPoint(this TreasureAndStartPointsType type)
        {
            return 4;
        }
        /// <summary>
        /// Тип сокровища - стартовая точка.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Boolean IsStartPoint(this TreasureAndStartPointsType type)
        {
            return (Int32)type >= ((Int32)type.GetMinimalNumberStartPoint()) 
                && (Int32)type <= ((Int32)type.GetMaximalNumberStartPoint());
        }

    }
}
