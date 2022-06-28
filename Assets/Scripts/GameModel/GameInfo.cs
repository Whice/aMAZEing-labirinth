using Assets.Scripts.GameModel.Player;
using System;

namespace Assets.Scripts.GameModel
{
    /// <summary>
    /// Информация об игре.
    /// <br/>Содержит информацию об игроках и сиды для:
    /// <br/>- Выбора игрока, который будет ходить первым.
    /// <br/>- Перемешивания колоды карт перед игрой.
    /// <br/>- Перемешивания ячеек на поле перед игрой.
    /// </summary>
    [Serializable]
    public class GameInfo
    {
        /// <summary>
        /// Задачть игроков. Сиды задаются отдельно.
        /// </summary>
        /// <param name="playersInfo"></param>
        public GameInfo(PlayerInfo[] playersInfo)
        {
            this.playersInfo = playersInfo;
        }
        /// <summary>
        /// Информация об игроках.
        /// </summary>
        public readonly PlayerInfo[] playersInfo;

        /// <summary>
        /// Сид для выбора игрока, который будет ходить первым.
        /// </summary>
        public Int32 fisrtPlayerNumberSeed = 0;
        /// <summary>
        /// Сид для перемешивания карт.
        /// </summary>
        public Int32 cardsShuffleSeed = 0;
        /// <summary>
        /// Сид для перемешивания ячеек.
        /// </summary>
        public Int32 cellsShuffleSeed = 0;

        /// <summary>
        /// Выполнить глубокое клонирование и получить клон.
        /// </summary>
        /// <returns></returns>
        public GameInfo Clone()
        {
            PlayerInfo[] clonePlayersInfo = new PlayerInfo[this.playersInfo.Length];
            for (int i = 0; i < clonePlayersInfo.Length; i++)
            {
                clonePlayersInfo[i] = this.playersInfo[i].Clone();
            }
            GameInfo clone = new GameInfo(clonePlayersInfo);
            clone.cardsShuffleSeed = this.cardsShuffleSeed;
            clone.cellsShuffleSeed = this.cellsShuffleSeed;
            clone.fisrtPlayerNumberSeed = this.fisrtPlayerNumberSeed;

            return clone;
        }

        #region Сравнение.

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj is GameInfo gameInfo)
            {
                if (gameInfo.playersInfo.Length != this.playersInfo.Length)
                {
                    return false;
                }
                else
                {
                    for (int i = 0; i < this.playersInfo.Length; i++)
                    {
                        if (gameInfo.playersInfo[i] != this.playersInfo[i])
                        {
                            return false;
                        }
                    }
                }

                if (gameInfo.cardsShuffleSeed != this.cardsShuffleSeed)
                {
                    return false;
                }
                if (gameInfo.cellsShuffleSeed != this.cellsShuffleSeed)
                {
                    return false;
                }
                if (gameInfo.fisrtPlayerNumberSeed != this.fisrtPlayerNumberSeed)
                {
                    return false;
                }

                return true;
            }

            return false;
        }
        public override Int32 GetHashCode()
        {
            Int32 hashCode = 0;
            foreach (PlayerInfo info in this.playersInfo)
            {
                hashCode ^= info.GetHashCode();
            }
            hashCode ^= this.cardsShuffleSeed;
            hashCode ^= this.cellsShuffleSeed;
            hashCode ^= this.fisrtPlayerNumberSeed;

            return hashCode;
        }
        public static bool operator ==(GameInfo l, GameInfo r)
        {
            if (l is null && r is null)
                return true;
            else if (l is null)
                return false;
            else
                return l.Equals(r);
        }
        public static bool operator !=(GameInfo l, GameInfo r)
        {
            return !(l == r);
        }

        #endregion Сравнение.
    }
}
