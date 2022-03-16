using System;
using System.Collections.Generic;

namespace Assets.Scripts.GameModel.FieldCells.SpecificFieldCells
{
    /// <summary>
    /// Клетка, где дорога проходит уголком между двумя соседними выходами.
    /// <br/> По умолчанию есть проходы вверх и право.
    /// </summary>
    public class CornerTwoDirectionFieldCell : FieldCell
    {
        public CornerTwoDirectionFieldCell() : base(new List<Boolean>() { true, true, false,  false })
        {

        }
    }
}
