using Assets.Scripts.GameModel.TurnPhaseAndExtensions;
using System;
using UnityEngine;

namespace UI
{
    public class EndMoveButton : GameUIOriginScript
    {
        [SerializeField]
        private Animator hideShowAnimator = null;
        /// <summary>
        /// Считать ячейку скрытой.
        /// </summary>
        private Boolean isNeedShowHidden
        {
            get => this.gameModel.currentPhase == TurnPhase.movingAvatar;
        }
        /// <summary>
        /// Изменить видимость ячейки.
        /// </summary>
        private void ChangeVisibility()
        {
            SetEnableObjectAnimationBegin();
        }
        /// <summary>
        /// Начать анимацию включения или отключения объекта этого скрипта.
        /// </summary>
        public void SetEnableObjectAnimationBegin()
        {
            this.hideShowAnimator.SetBool("IsNeedHide", !this.isNeedShowHidden);
        }

        protected override void Awake()
        {
            base.Awake();
            SetEnableObjectAnimationBegin();
            this.gameModel.onPhaseChange += ChangeVisibility;
        }
        protected override void OnDestroy()
        {
            this.gameModel.onPhaseChange -= ChangeVisibility;
            base.OnDestroy();
        }
    }
}