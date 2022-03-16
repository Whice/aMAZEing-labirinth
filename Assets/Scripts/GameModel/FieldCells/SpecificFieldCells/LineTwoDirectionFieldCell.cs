using System;
using System.Collections.Generic;

namespace Assets.Scripts.GameModel.FieldCells.SpecificFieldCells
{
    /// <summary>
    /// Клетка, где дорога проходит линией между двумя противоположными выходами
    /// <br/> По умолчанию есть проходы вверх и вниз.
    /// </summary>
    public class LineTwoDirectionFieldCell : FieldCell
    {
        public LineTwoDirectionFieldCell() : base(new List<Boolean>() { true, false, true, false })
        {

        }
    }
}
