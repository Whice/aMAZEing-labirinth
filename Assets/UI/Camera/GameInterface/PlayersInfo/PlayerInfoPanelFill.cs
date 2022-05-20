using Assets.Scripts.GameModel.Player;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace UI
{
    /// <summary>
    /// ������ ���������� ���������� �� ������, ��� ������ ���.
    /// </summary>
    public class PlayerInfoPanelFill : GameUIOriginScript
    {
        /// <summary>
        /// ��������� ���� ��� ����� �������� ������.
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI playerName;

        /// <summary>
        /// ��������� ��������� ���� ����� ������� �������� ������ � ������. 
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