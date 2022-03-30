using System;
using System.Collections.Generic;

namespace Assets.Scripts.Extensions
{
    public static class ListExtensions
    {
        /// <summary>
        /// Перемешать.
        /// <br/>Тасование Фишера-Йетса.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public static void Shuffle<T>(this List<T> list)
        {
            Random rand = new Random();

            for (int i = list.Count - 1; i >= 1; i--)
            {
                int j = rand.Next(i + 1);

                (list[j], list[i]) = (list[i], list[j]);
            }
        }
    }
}
