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
            if (!this.gameObject.activeSelf)
            {
                this.gameObject.SetActive(true);
            }

            SetEnableObjectAnimationBegin();
        }
        /// <summary>
        /// Начать анимацию включения или отключения объекта этого скрипта.
        /// </summary>
        public void SetEnableObjectAnimationBegin()
        {
            int isNeedShowHiddenInt = this.isNeedShowHidden ? 1 : -1;
            this.hideShowAnimator.SetInteger("IsNeedHide", isNeedShowHiddenInt);
            this.hideShowAnimator.speed = 1;
        }
        /// <summary>
        /// Отключить анимацию.
        /// </summary>
        public void DisableAnimation()
        {
            this.hideShowAnimator.speed = 0;
        }

        protected override void Subscribe()
        {
            base.Subscribe();
            this.gameModel.onPhaseChange += ChangeVisibility;
        }
        protected override void Unsubscribe()
        {
            this.gameModel.onPhaseChange -= ChangeVisibility;
            base.Unsubscribe();
        }
        public override void Initialized()
        {
            base.Initialized();
            SetEnableObjectAnimationBegin();
            this.gameObject.SetActive(false);
        }
    }
}