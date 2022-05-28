using Assets.Scripts.GameModel;
using Assets.Scripts.GameModel.Player;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace UI
{

    public class GameInterfaceRectanglesDetected : MonoSingleton<GameInterfaceRectanglesDetected>
    {
        /// <summary>
        /// Модель игры, реализовывает логику взаимодействия всех частей.
        /// </summary>
        private Game gameModel
        {
            get => GameManager.instance.gameModel;
        }
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
            Rect currentRect = element.GetComponent<RectTransform>().rect;
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
                for(Int32 i=0; i<this.gameUIOriginScriptTransforms.Count;i++)
                {
                    currentRect = GetElementRect(this.gameUIOriginScriptTransforms[i]);
                    if (currentRect.Contains(new Vector2(cursorPosition.x, cursorPosition.y)))
                    {
                        return true;
                    }

                }

                return false;
            }
        }

        #region Конец игры.

        [Header("End Game")]
        [SerializeField]
        private TextMeshProUGUI tmpForWinnerTable = null;
        [SerializeField]
        private GameObject endGameObject = null;
        private void ShowGameEndTable()
        {
            String winnerTable = "Позиции игроков:\n";
            GamePlayer[] winners = this.gameModel.GetWinners();
            Int32 numberWinner = 0;
            foreach (GamePlayer winner in winners)
            {
                ++numberWinner;
                winnerTable += numberWinner + ". " + winner.name + "\n";
            }

            this.tmpForWinnerTable.text = winnerTable;
            this.endGameObject.SetActive(true);
        }

        #endregion Конец игры.

        protected virtual void Awake()
        {
            this.gameModel.OnGameEnded += ShowGameEndTable;
        }
        protected virtual void OnDestroy()
        {
            this.gameModel.OnGameEnded -= ShowGameEndTable;
        }
    }
}