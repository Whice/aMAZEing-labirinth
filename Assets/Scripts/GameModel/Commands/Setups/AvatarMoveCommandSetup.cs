using System;

namespace Assets.Scripts.GameModel.Commands
{
    /// <summary>
    /// Структура для передачи значений в команду хода игрока.
    /// </summary>
    public struct AvatarMoveCommandSetup
    {
        /// <summary>
        /// Куда игрок должен пойти по горизонтали.
        /// </summary>
        public int playerMoveToX;
        /// <summary>
        /// Куда игрок должен пойти по вертикали.
        /// </summary>
        public int playerMoveToY;
        /// <summary>
        /// Откуда игрок должен пойти по горизонтали.
        /// </summary>
        public int playerMoveFromX;
        /// <summary>
        /// Откуда игрок должен пойти по вертикали.
        /// </summary>
        public int playerMoveFromY;
        /// <summary>
        /// Номер игрока, нужен для проверки правильности игрока.
        /// </summary>
        public int playerNumber;

        /// <summary>
        /// Инициализировать команду.
        /// </summary>
        /// <param name="playerMoveToX">Куда игрок должен пойти по горизонтали.</param>
        /// <param name="playerMoveToY">Куда игрок должен пойти по вертикали.</param>
        /// <param name="playerMoveFromX">Откуда игрок должен пойти по горизонтали.</param>
        /// <param name="playerMoveFromY">Откуда игрок должен пойти по вертикали.</param>
        /// <param name="playerNumber">Номер игрока, нужен для проверки правильности игрока.</param>
        public AvatarMoveCommandSetup(Int32 playerMoveToX, Int32 playerMoveToY,
            Int32 playerMoveFromX, Int32 playerMoveFromY, Int32 playerNumber)
        {
            this.playerMoveToX = playerMoveToX;
            this.playerMoveToY = playerMoveToY;
            this.playerMoveFromX = playerMoveFromX;
            this.playerMoveFromY = playerMoveFromY;
            this.playerNumber = playerNumber;
        }
    }
}
