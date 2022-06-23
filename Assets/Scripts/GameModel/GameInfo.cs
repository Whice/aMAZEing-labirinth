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
        public readonly PlayerInfo[] playersInfo = null;

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

    }
}
