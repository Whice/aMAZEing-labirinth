using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.GameModel.PlayingField
{
    public enum FieldSide
    {
        /// <summary>
        /// Неопознано.
        /// </summary>
        unknow=0,
        /// <summary>
        /// Верхняя сторона.
        /// </summary>
        up=1,
        /// <summary>
        /// Правая сторона.
        /// </summary>
        right = 2,
        /// <summary>
        /// Нижняя сторона.
        /// </summary>
        down = 3,
        /// <summary>
        /// Левая сторона.
        /// </summary>
        left = 4,
    }
}
