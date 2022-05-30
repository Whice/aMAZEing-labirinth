using Assets.Scripts.GameModel.Player;
using System;
using UnityEngine;
using static UnityEngine.ParticleSystem;

namespace Assets.Scripts.GameView
{
    /// <summary>
    /// Слот для аватара игрока.
    /// </summary>
    public class PlayerAvatarSlot : GameWorldViewOriginScript
    {
        /// <summary>
        /// Объект аватара игрока.
        /// </summary>
        private GameObject avatar;
        /// <summary>
        /// Установить в слот аватар.
        /// </summary>
        private void SetAvatar()
        {
            this.avatar = GameManager.instance.playerAvatarsProvider.GetPrefabClone("Player" + this.playerNumber);
            this.avatar.transform.parent = this.transform;
            this.avatar.transform.localPosition = Vector3.zero;
        }
        /// <summary>
        /// Частицы над головой.
        /// </summary>
        [SerializeField]
        private ParticleSystem particleUnderHead = null;
        /// <summary>
        /// Частицы над головой аватара показаны.
        /// </summary>
        public Boolean isShowParticleUnderHead
        {
            get
            {
                return this.particleUnderHead.gameObject.activeSelf;
            }
            set
            {
                this.particleUnderHead.gameObject.SetActive(value);

                //Установить цвет частицам соответсвенно цвету игрока.
                if (value)
                {
                    MainModule particleUnderHeadMain = this.particleUnderHead.main;
                    particleUnderHeadMain.startColor = new MinMaxGradient(new Color
                        (
                        this.gameModel.currentPlayer.color.R,
                        this.gameModel.currentPlayer.color.G,
                        this.gameModel.currentPlayer.color.B
                        ));
                }
            }
        }
        /// <summary>
        /// Смена игрока.
        /// </summary>
        private void PlayerChanged()
        {
            if (currentPlayer != null)
                this.isShowParticleUnderHead = this.gameModel.currentPlayer.playerNumer == this.playerNumber;
        }
        /// <summary>
        /// Объект игрока из модели для этого слота.
        /// </summary>
        private GamePlayer currentPlayerPrivate;
        /// <summary>
        /// Объект игрока из модели для этого слота.
        /// </summary>
        public GamePlayer currentPlayer
        {
            get => this.currentPlayerPrivate;
        }
        /// <summary>
        /// Номер игрока в этом слоте.
        /// </summary>
        public Int32 playerNumber
        {
            get => currentPlayerPrivate.playerNumer;
        }
        /// <summary>
        /// Установить объект игрока из модели для этого слота.
        /// </summary>
        /// <param name="playerNumber">Номер игрока.</param>
        public void SetPlayer(Int32 playerNumber)
        {
            this.currentPlayerPrivate = this.gameModel.players[playerNumber];
            SetAvatar();
        }

        protected override void Awake()
        {
            base.Awake();
            this.gameModel.onPlayerChanged += PlayerChanged;
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            this.gameModel.onPlayerChanged -= PlayerChanged;
        }
        public override string ToString()
        {
            String ansver = "Avatar: ";
            ansver += this.avatar == null ? "null" : this.avatar.name;
            ansver += "; Parent of avatar: " + this.transform.name + "\nPlayer number: ";
            ansver += this.avatar == null ? "null" : this.playerNumber.ToString();
            ansver += "; Player position: ";
            ansver += this.avatar == null ? "null" : this.currentPlayer.position.ToString();
            return ansver;
        }
    }
}