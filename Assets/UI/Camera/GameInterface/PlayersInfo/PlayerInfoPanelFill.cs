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
    /// ������ ���������� ���������� �� ������, ��� ������ ���.
    /// </summary>
    public class PlayerInfoPanelFill : GameUIOriginScript
    {
        /// <summary>
        /// ��������� ���� ��� ����� �������� ������.
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI playerName = null;
        /// <summary>
        /// �������� ��� ��������.
        /// </summary>
        [SerializeField]
        private Animator animatorPlayerInfo = null;
        /// <summary>
        /// �������� � ������ ������.
        /// </summary>
        [SerializeField]
        private Image playerColor = null;

        /// <summary>
        /// ��������� ��������� ���� ����� ������� �������� ������ � ������. 
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
        /// ��������� ��������.
        /// </summary>
        public void DisableAnimation()
        {
            AnimationEnable(false);
        }
        /// <summary>
        /// �������� ��������.
        /// </summary>
        /// <param name="isEnable">���� �� �������� ���������.</param>
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