using Assets.Scripts.GameModel.Player;
using System;
using System.Collections;
using System.Collections.Generic;
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
        /// Заполнить текстовое поле имени имененм текущего игрока в модели. 
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
        }

        protected override void Awake()
        {
            base.Awake();

            AnimationEnable(false);
            this.playerName.text = this.currentPlayer.name;
            this.gameModel.onNextTurnMoved += FillPlayerName;
        }
        protected override void OnDestroy()
        {
            this.gameModel.onNextTurnMoved -= FillPlayerName;
            base.OnDestroy();
        }
    }
}