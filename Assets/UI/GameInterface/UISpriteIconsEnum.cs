using System;

namespace Assets.UI
{
    /// <summary>
    /// Названия и номера элементов-спрайтов в интерфейсе.
    /// </summary>
    public enum UISpriteIconsEnum
    {
        /// <summary>
        /// Иконка отправления на стартовую точку.
        /// </summary>
        GoHome_icon = 1,
        /// <summary>
        /// Иконка победителя.
        /// </summary>
        Winner_icon = 2
    }

    public static class UISpriteIconsEnumExtensions
    {
        /// <summary>
        /// Получить код - номер элемента в перечислении.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumElement"></param>
        /// <returns></returns>
        public static Int32 ToInt32<T>(this UISpriteIconsEnum enumElement)
        {
            return (Int32)enumElement;
        }
    }
}
