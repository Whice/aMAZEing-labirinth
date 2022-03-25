using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    }
}
