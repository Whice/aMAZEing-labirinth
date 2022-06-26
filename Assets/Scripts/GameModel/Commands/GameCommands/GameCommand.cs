using Assets.Scripts.GameModel.Logging;

namespace Assets.Scripts.GameModel.Commands.GameCommands
{
    /// <summary>
    /// Общий класс для игровых команд.
    /// Содержит главный метод выполнения и методы вывод сообщений.
    /// </summary>
    public abstract class GameCommand
    {
        /// <summary>
        /// Выполнить команду для указаной модели игры.
        /// </summary>
        /// <param name="modelGame">Модель игры. Не должна быть null.</param>
        public virtual bool Execute(Game modelGame)
        {
            if (modelGame == null)
            {
                GameModelLogger.LogError(nameof(modelGame) + " in "
                    + nameof(Execute) + " in "
                    + nameof(GameCommand) + " is null!!!");
                return false;
            }

            return true;
        }
        /// <summary>
        /// Отменить эту команду.
        /// </summary>
        /// <param name="modelGame">Модель игры. Не должна быть null.</param>
        public virtual bool Undo(Game modelGame)
        {
            if (modelGame == null)
            {
                GameModelLogger.LogError(nameof(modelGame) + " in "
                    + nameof(Execute) + " in "
                    + nameof(GameCommand) + " is null!!!");
                return false;
            }

            return true;
        }
    }
}
