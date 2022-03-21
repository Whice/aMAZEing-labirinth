using System;
using System.Collections.Generic;

namespace Assets.Scripts.GameModel.PlayingField.FieldCells.SpecificFieldCells
{
    /// <summary>
    /// Клетка, где дорога проходит уголком между двумя соседними выходами.
    /// <br/> По умолчанию есть проходы вверх и право.
    /// </summary>
    public class CornerTwoDirectionFieldCell : FieldCell
    {
        public CornerTwoDirectionFieldCell() : base(new List<Boolean>() { true, true, false,  false }, CellType.corner)
        {

        }
        public override FieldCell Clone()
        {
            CornerTwoDirectionFieldCell cell = new CornerTwoDirectionFieldCell();
            cell.directions = this.CopyDirections();
            cell.isInteractable = this.isInteractable;
            return cell;
        }
    }
}
