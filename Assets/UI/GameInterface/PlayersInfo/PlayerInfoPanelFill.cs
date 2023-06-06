using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    /// <summary>
    /// Скрипт заполнения информации об игроке, чей сейчас ход.
    /// </summary>
    public class PlayerInfoPanelFill : GameUIOriginScript
    {
        #region Заполнение общей информации об игроке.

        /// <summary>
        /// Текстовое поля для имени текущего игрока.
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI playerName = null;
        /// <summary>
        /// Аниматор для таблички.
        /// </summary>
        [SerializeField]
        private Animator animatorPlayerInfo = null;
        /// <summary>
        /// Картинка с цветом игрока.
        /// </summary>
        [SerializeField]
        private Image playerColor = null;

        /// <summary>
        /// Заполнить текстовое поле имени именем текущего игрока в модели. 
        /// </summary>
        private void FillPlayerName()
        {
            this.playerName.text = this.currentPlayer.name;
            
            this.playerColor.color = new Color
                (
                this.gameModel.currentPlayer.color.R,
                this.gameModel.currentPlayer.color.G,
                this.gameModel.currentPlayer.color.B
                );
            AnimationEnable(true);
        }
        /// <summary>
        /// Отключить анимацию.
        /// </summary>
        public void DisableAnimation()
        {
            AnimationEnable(false);
        }
        /// <summary>
        /// Включить анимацию.
        /// </summary>
        /// <param name="isEnable">Надо ли включить анимаюцию.</param>
        private void AnimationEnable(Boolean isEnable)
        {
            this.animatorPlayerInfo.SetBool("PlayerChange", isEnable);
            // Добавить иконку возвращения на стартовую точку на панель инфо.
            this.GoHome.Activate(this.currentPlayer);
        }

        #endregion Заполнение общей информации об игроке.

        #region Дополнительная информация об игроке.

        #region Иконка "домой"

        /// <summary>
        /// Иконка возвращения на стартовую точку.
        /// </summary>
        [SerializeField]
        private GoHomeImageAnimation GoHome = null;

        #endregion Иконка "домой"

        #endregion Дополнительная информация об игроке.


        protected override void Subscribe()
        {
            base.Subscribe();
            this.gameModel.onNextTurnMoved += FillPlayerName;
        }
        protected override void Unsubscribe()
        {
            this.gameModel.onNextTurnMoved -= FillPlayerName;
            base.Unsubscribe();
        }
        public override void Initialize()
        {
            base.Initialize();
            AnimationEnable(false);
            this.playerName.text = this.currentPlayer.name;
            FillPlayerName();
        }
    }
}