using System;
using System.Drawing;

namespace Assets.Scripts.GameModel.Player
{
    /// <summary>
    /// Класс, содержащий информацию о игроке, которая не относиться к игровой логике.
    /// </summary>
    [Serializable]
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
        /// <param name="playerNumer"></param>
        public PlayerInfo(String name, Color color)
        {
            this.name = name;
            this.color = color;
        }

        #region Сравнение.

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj is PlayerInfo otherDeck)
            {
                if(this.name!=otherDeck.name)
                {
                    return false;
                }
                if(this.color!=otherDeck.color)
                {
                    return false;
                }

                return true;
            }

            return false;
        }
        public override Int32 GetHashCode()
        {
            Int32 hashCode = this.name.GetHashCode();
            hashCode |= this.color.GetHashCode();

            return hashCode;
        }
        public static bool operator ==(PlayerInfo l, PlayerInfo r)
        {
            if (l is null && r is null)
                return true;
            else if (l is null)
                return false;
            else
                return l.Equals(r);
        }
        public static bool operator !=(PlayerInfo l, PlayerInfo r)
        {
            return !(l == r);
        }

        #endregion Сравнение.

        /// <summary>
        /// Выполнить глубокое клонирование и получить клон.
        /// </summary>
        /// <returns></returns>
        public PlayerInfo Clone()
        {
            return new PlayerInfo(this.name, this.color);
        }
    }
}
