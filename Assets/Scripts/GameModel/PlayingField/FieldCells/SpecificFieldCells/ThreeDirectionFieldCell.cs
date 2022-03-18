using System;
using System.Collections.Generic;


namespace Assets.Scripts.GameModel.PlayingField.FieldCells.SpecificFieldCells
{
    /// <summary>
    /// Клетка, где три пути пересекаются, один проход отсутсвует.
    /// <br/> По умолчанию есть проходы вверх, вправо, вниз.
    /// </summary>
    public class ThreeDirectionFieldCell: FieldCell
    {
        public ThreeDirectionFieldCell() : base(new List<Boolean>() { true, true, true, false }, CellType.threeDirection)
        {

        }
    }
}
