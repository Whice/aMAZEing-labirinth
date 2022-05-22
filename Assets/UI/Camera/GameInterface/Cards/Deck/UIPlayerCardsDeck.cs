using Assets.Scripts.GameModel.PlayingField.Treasures;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class UIPlayerCardsDeck : GameUIOriginScript
    {
        /// <summary>
        /// UI колода игрока.
        /// </summary>
        private List<UICardWithTreasureSlot> cardSlots;

        private Int32 countCards
        {
            get => this.gameModel.currentPlayer.countCardInDeck;
        }
        private void CreateDeck()
        {
            this.cardSlots = new List<UICardWithTreasureSlot>(this.countCards);
        }
        private void ClearDeck()
        {
            for (Int32 i = 0; i < this.cardSlots.Count; i++)
            {
                this.cardSlots[i].Hide();
            }
            this.cardSlots.Clear();
        }

        /// <summary>
        /// «аполнить ui колоду игрока.
        /// </summary>
        private void FillPlayersDeck()
        {
            ClearDeck();
            UICardWithTreasureSlot currentSlot = null;
            TreasureAndStartPointsType[] treasures = this.gameModel.currentPlayer.treasuresOfThisDeck;
            foreach (TreasureAndStartPointsType treasure in treasures)
            {
                currentSlot = GameManager.instance.uiCardWithTreasureSlotProvider.GetCardSlot(treasure);
                this.cardSlots.Add(currentSlot);
                currentSlot.Close();
                currentSlot.transform.SetParent(this.transform);
            }
            currentSlot.Open();
        }

        protected override void Awake()
        {
            base.Awake();

            this.gameModel.onPlayerChanged += FillPlayersDeck;

            CreateDeck();
            FillPlayersDeck();
        }

        protected override void OnDestroy()
        {

            this.gameModel.onPlayerChanged -= FillPlayersDeck;
            base.OnDestroy();
        }
    }
}
