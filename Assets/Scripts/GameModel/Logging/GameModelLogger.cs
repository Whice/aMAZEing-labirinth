using System;

namespace Assets.Scripts.GameModel.Logging
{
    /// <summary>
    /// Класс для передачи сообщений из модели во внешние системы 
    /// вывода сообщений.
    /// </summary>
    public static class GameModelLogger
    {
        /// <summary>
        /// Было передано сообщение об ошибке.
        /// </summary>
        public static Action<String> onLogError;
        /// <summary>
        /// Было передано предупреждение.
        /// </summary>
        public static Action<String> onLogWarning;
        /// <summary>
        /// Было передано сообщение с информацией.
        /// </summary>
        public static Action<String> onLogInfo;


        /// <summary>
        /// Передать сообщение об ошибке.
        /// </summary>
        public static void LogError(String message)
        {
            GameModelLogger.onLogError?.Invoke(message);
        }
        /// <summary>
        /// Передать предупреждение.
        /// </summary>
        public static void LogWarning(String message)
        {
            GameModelLogger.onLogWarning?.Invoke(message);
        }
        /// <summary>
        /// Передать сообщение с информацией.
        /// </summary>
        public static void LogInfo(String message)
        {
            GameModelLogger.onLogInfo?.Invoke(message);
        }
        /// <summary>
        /// Передать сообщение об ошибке.
        /// </summary>
        public static void LogError(object message)
        {
            LogError(message.ToString());
        }
        /// <summary>
        /// Передать предупреждение.
        /// </summary>
        public static void LogWarning(object message)
        {
            LogWarning(message.ToString());
        }
        /// <summary>
        /// Передать сообщение с информацией.
        /// </summary>
        public static void LogInfo(object message)
        {
            LogInfo(message.ToString());
        }
    }
}
