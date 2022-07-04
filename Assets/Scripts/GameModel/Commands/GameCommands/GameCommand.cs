using Assets.Scripts.GameModel.Logging;
using System;

namespace Assets.Scripts.GameModel.Commands.GameCommands
{
    /// <summary>
    /// Общий класс для игровых команд.
    /// Содержит главный метод выполнения и методы вывод сообщений.
    /// </summary>
#pragma warning disable CS0660 // Тип определяет оператор == или оператор !=, но не переопределяет Object.Equals(object o)
#pragma warning disable CS0661 // Тип определяет оператор == или оператор !=, но не переопределяет Object.GetHashCode()
    [Serializable]
    public abstract class GameCommand
#pragma warning restore CS0661 // Тип определяет оператор == или оператор !=, но не переопределяет Object.GetHashCode()
#pragma warning restore CS0660 // Тип определяет оператор == или оператор !=, но не переопределяет Object.Equals(object o)
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

        /// <summary>
        /// Выполнить глубокое клонирование команды и получить клон.
        /// </summary>
        /// <returns></returns>
        public abstract GameCommand Clone();

        public static bool operator ==(GameCommand l, GameCommand r)
        {
            if (l is null && r is null)
                return true;
            else if (l is null)
                return false;
            else
                return l.Equals(r);
        }
        public static bool operator !=(GameCommand l, GameCommand r)
        {
            return !(l == r);
        }
    }
}
