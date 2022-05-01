using System;

namespace UnityEngine
{
    /// <summary>
    /// Создан для упрощения выдачи сообщений в консоль,
    /// а также формирования своих.
    /// </summary>
    public class MonoBehaviourLogger : MonoBehaviour
    {
        /// <summary>
        /// Вывести сообщение об ошибке в консоль.
        /// </summary>
        /// <param name="message"></param>
        protected void LogError(String message)
        {
            Debug.LogError(message);
        }
        /// <summary>
        /// Вывести предупреждене в консоль.
        /// </summary>
        /// <param name="message"></param>
        protected void LogWarning(String message)
        {
            Debug.LogWarning(message);
        }
        /// <summary>
        /// Вывести сообщение в консоль.
        /// </summary>
        /// <param name="message"></param>
        protected void LogInfo(String message)
        {
            Debug.Log(message);
        }
    }
}
