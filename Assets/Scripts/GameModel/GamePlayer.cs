using System;
using System.Drawing;

namespace Assets.Scripts.GameModel
{
    /// <summary>
    /// Игрок в партии.
    /// </summary>
    public class GamePlayer
    {
        public readonly String name;
        public readonly Color color;

        public GamePlayer(String name, Color color)
        {
            this.name = name;
            this.color = color;
        }


        public Int32 positionX;
        public Int32 positionY;
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
