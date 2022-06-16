using Assets.Scripts.GameModel.Player;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    /// <summary>
    /// В первый раз табличка показывается красочно, чтобы дать понять, что надо топать домой.
    /// Потом просто присутствует около имени.
    /// </summary>
    public class GoHomeImageAnimation : GameUIOriginScript
    {
        /// <summary>
        /// Икроки, для которых уже показали табличку.
        /// </summary>
        private HashSet<Int32> playersForWhichHaveAlreadyShown = new HashSet<int>(4);
        /// <summary>
        /// Аниматор, увеличивающий и уменьшающий домик.
        /// </summary>
        [SerializeField]
        private Animator animatorGoHome = null;
        /// <summary>
        /// Активировать префаб домика.
        /// Если он активируется в первый раз для указанного игрока, то проигрывается анимация.
        /// </summary>
        /// <param name="player"></param>
        public void Activate(GamePlayer player)
        {
            //Если в колоде осталась только одна карта - вернуться на точку старта.
            Boolean isActivate = player.countCardInDeck == 1;

            this.gameObject.SetActive(isActivate);

            //по номеру игрока определяется, проигрывалась ли для него анимация.
            Int32 playerNumber = player.playerNumer;
            Boolean isFisrtTimeForPlayer = !this.playersForWhichHaveAlreadyShown.Contains(playerNumber);
            if (isActivate && isFisrtTimeForPlayer)
            {
                this.animatorGoHome.SetBool("ShowGoHome", true);
                this.playersForWhichHaveAlreadyShown.Add(playerNumber);

                this.screenCenterPosition = new Vector3
                    (
                    Screen.width * 0.5f,
                    Screen.height * 0.5f,
                    this.transform.position.z
                    ) ;
                this.movementFromCenterOfScreenTimeLeft = 0;
                this.originPosition = this.transform.position;
            }
        }

        /// <summary>
        /// Отключить анимацию.
        /// </summary>
        public void DisableAnimation()
        {
            this.animatorGoHome.SetBool("ShowGoHome", false);
        }

        #region Движение от центра на положенное место.

        /// <summary>
        /// Изначальная позиция домика, до анимации движения.
        /// </summary>
        private Vector3 originPosition;
        /// <summary>
        /// Центр экрана, откуда начинает появляться домик.
        /// </summary>
        private Vector3 screenCenterPosition;
        /// <summary>
        /// Время, за которое домик вернется в изначальное положение.
        /// </summary>
        [SerializeField]
        private Single movementFromCenterOfScreenTime = 2.5f;
        /// <summary>
        /// Время, которое прошло с тех пор, как домик начал движение к изначальному положению.
        /// </summary>
        private Single movementFromCenterOfScreenTimeLeft;
        /// <summary>
        /// Выполняется движение от центра к изначальному положению.
        /// </summary>
        private Boolean isPerformMovementFromCenter
        {
            get => this.movementFromCenterOfScreenTimeLeft < this.movementFromCenterOfScreenTime;
        }

        #endregion Движение от центра на положенное место.

        void Update()
        {
            if (this.isPerformMovementFromCenter)
            {
                this.movementFromCenterOfScreenTimeLeft += Time.deltaTime;
                if (this.movementFromCenterOfScreenTimeLeft > this.movementFromCenterOfScreenTime)
                {
                    this.movementFromCenterOfScreenTimeLeft = this.movementFromCenterOfScreenTime;
                }

                Single distanceTraveledPercentage = this.movementFromCenterOfScreenTimeLeft / this.movementFromCenterOfScreenTime;
                this.transform.position = Vector3.Lerp(this.screenCenterPosition, this.originPosition, distanceTraveledPercentage);
            }
        }
    }
}