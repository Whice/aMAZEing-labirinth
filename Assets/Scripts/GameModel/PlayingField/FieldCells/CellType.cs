using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.GameModel.PlayingField.FieldCells
{
    /// <summary>
    /// Тип ячейки.
    /// Хранит информацию о том, сколько проходов у нее и как они расположены.
    /// </summary>
    public enum CellType : byte
    {
        /// <summary>
        /// Не известно.
        /// </summary>
        unknown = 0,
        /// <summary>
        /// Уголок. Два прохода.
        /// </summary>
        corner = 1,
        /// <summary>
        /// Линия. Два прохода.
        /// </summary>
        line,
        /// <summary>
        /// Три прохода.
        /// </summary>
        threeDirection
    }
}
