using System;
using System.Drawing;

namespace Assets.Scripts.GameModel.Player
{
    /// <summary>
    /// Класс, содержащий информацию о игроке, которая не относиться к игровой логике.
    /// </summary>
    public class PlayerInfo
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
        /// Заполнить инфо о игроке, которая не относиться к игровой логике.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="color"></param>
        public PlayerInfo(String name, Color color)
        {
            this.name = name;
            this.color = color;
        }
    }
}
