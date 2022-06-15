using System;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    /// <summary>
    /// Этот скрипт нужен, чтобы собирать информацию о том,
    /// был ли нажат какой-либо элемент интерфейса,
    /// который наследует от <see cref="GameUIOriginScript"/>.
    /// </summary>
    public class GameInterfaceRectanglesDetected : MonoSingleton<GameInterfaceRectanglesDetected>
    {
        /// <summary>
        /// Инфо о положении всех видимых UI элементов.
        /// </summary>
        private List<Transform> gameUIOriginScriptTransforms = new List<Transform>();
        /// <summary>
        /// Добавить скрипт видимого элемента.
        /// </summary>
        /// <param name="gameViewOriginScript"></param>
        public void AddGameViewOriginScripts(GameUIOriginScript gameViewOriginScript)
        {
            this.gameUIOriginScriptTransforms.Add(gameViewOriginScript.transform);
        }
        /// <summary>
        /// Удалить скрипт видимого элемента.
        /// </summary>
        /// <param name="gameViewOriginScript"></param>
        public void RemoveGameViewOriginScripts(GameUIOriginScript gameViewOriginScript)
        {
            this.gameUIOriginScriptTransforms.Remove(gameViewOriginScript.transform);
        }
        /// <summary>
        /// Получить прямоугольник видимого элемента.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        private Rect GetElementRect(Transform element)
        {
            Vector3 currentPosition = element.position;
            Vector3 lossyScale = element.transform.lossyScale;
            Rect currentRect = element.GetComponent<RectTransform>().rect;
            currentRect = new Rect(0, 0, currentRect.width * lossyScale.x, currentRect.height * lossyScale.y);
            //Смещение позиции для правильного расчета прямоугольника
            Single correctCurrentPositionX = currentPosition.x - currentRect.width * 0.5f;
            Single correctCurrentPositionY = currentPosition.y - currentRect.height * 0.5f;

            return new Rect(correctCurrentPositionX, correctCurrentPositionY, currentRect.width, currentRect.height);
        }

        /// <summary>
        /// Указатель мыши(или что там у вас) указывает в элемент UI.
        /// </summary>
        public Boolean isPointerOnUIElement
        {
            get
            {
                Vector3 cursorPosition = Input.mousePosition;
                Rect currentRect;
                for (Int32 i = 0; i < this.gameUIOriginScriptTransforms.Count; i++)
                {
                    if (this.gameUIOriginScriptTransforms[i].gameObject.activeSelf)
                    {
                        if (this.gameUIOriginScriptTransforms[i].GetComponent<GameUIOriginScript>().isShouldInterruptPressesInWorld)
                        {
                            currentRect = GetElementRect(this.gameUIOriginScriptTransforms[i]);
                            if (currentRect.Contains(new Vector2(cursorPosition.x, cursorPosition.y)))
                            {
                                return true;
                            }
                        }
                    }
                }

                return false;
            }
        }
    }
}