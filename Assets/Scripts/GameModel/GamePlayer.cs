using System;
using System.Drawing;

namespace Assets.Scripts.GameModel
{
    /// <summary>
    /// Игрок в партии.
    /// </summary>
    public class GamePlayer
    {
        /// <summary>
        /// Имя или прозвище игрока.
        /// </summary>
        public readonly String name;
        /// <summary>
        /// Цвет игрока.
        /// </summary>
        public readonly Color color;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">Имя или прозвище игрока.</param>
        /// <param name="color">Цвет игрока.</param>
        public GamePlayer(String name, Color color)
        {
            this.name = name;
            this.color = color;
        }

        /// <summary>
        /// Местоположение на поле по оси X.
        /// </summary>
        public Int32 positionX;
        /// <summary>
        /// Местоположение на поле по оси Y.
        /// </summary>
        public Int32 positionY;
        /// <summary>
        /// Местоположение на поле.
        /// </summary>
        public Point position
        {
            get
            {
                return new Point(this.positionX, this.positionY);
            }
            set
            {
                this.positionX = value.X;
                this.positionY = value.Y;
            }
        }
    }
}
