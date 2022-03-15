using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.GameModel.FieldCells
{
    /// <summary>
    ///  Направление прохода.
    /// </summary>
    public enum PassageDirection
    {
        /// <summary>
        /// Проход отсутствует.
        /// </summary>
        none,
        /// <summary>
        /// Вверх.
        /// </summary>
        up,
        /// <summary>
        /// Влево.
        /// </summary>
        left,
        /// <summary>
        /// Вниз.
        /// </summary>
        down,
        /// <summary>
        /// Вправо.
        /// </summary>
        right
    }
    public abstract class FieldCell
    {
        /// <summary>
        /// Проходы.
        /// </summary>
        protected List<PassageDirection> directions = null;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="directions">Проходы.</param>
        public FieldCell(List<PassageDirection> directions)
        {
            this.directions = directions;
        }


    }
}
