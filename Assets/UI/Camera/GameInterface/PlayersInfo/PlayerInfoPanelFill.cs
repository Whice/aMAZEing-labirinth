using Assets.Scripts.GameModel.Player;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
        private TextMeshProUGUI playerName;

        /// <summary>
        /// Заполнить текстовое поле имени имененм текущего игрока в модели. 
        /// </summary>
        private void FillPlayerName()
        {
            this.playerName.text = this.currentPlayer.name;
        }

        protected override void Awake()
        {
            base.Awake();

            this.playerName.text = this.currentPlayer.name;
            this.gameModel.onNextTurnMoved += FillPlayerName;
        }

        private void OnDestroy()
        {
            this.gameModel.onNextTurnMoved -= FillPlayerName;
        }
    }
}