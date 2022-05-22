using Assets.Scripts.GameModel.Player;
using System;
using System.Collections.Generic;
using UnityEngine;


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