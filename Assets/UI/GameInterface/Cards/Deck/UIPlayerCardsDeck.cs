using Assets.Scripts.GameModel.PlayingField.Treasures;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace UI
{
    /// <summary>
    /// Отображение в UI колоды  текущего игрока.
    /// </summary>
    public class UIPlayerCardsDeck : GameUIOriginScript
    {
        /// <summary>
        /// Место, куда будут складироваться карты,
        /// которые показываются в нынешний момент.
        /// </summary>
        [SerializeField]
        private Transform activeCardsParent = null;
        /// <summary>
        /// Место, куда будут складироваться карты,
        /// которые скрыты в нынешний момент.
        /// </summary>
        [SerializeField]
        private Transform unactiveCardsParent = null;
        /// <summary>
        /// UI колода игрока.
        /// </summary>
        private List<UICardWithTreasureSlot> cardSlots;
        /// <summary>
        /// Последняя карта в колоде.
        /// </summary>
        private UICardWithTreasureSlot lastCard
        {
            get => this.cardSlots[this.cardSlots.Count - 1];
        } 
        /// <summary>
                 /// Количество карт у игрока.
                 /// </summary>
        private Int32 countCardsPlayerHas
        {
            get => this.gameModel.currentPlayer.countCardInDeck;
        }
        /// <summary>
        /// Создать список для отображения колоды.
        /// </summary>
        private void CreateDeck()
        {
            this.cardSlots = new List<UICardWithTreasureSlot>(this.countCardsPlayerHas);
        }
        /// <summary>
        /// Очистить список от карт другого игрока.
        /// </summary>
        private void ClearDeck()
        {
            for (Int32 i = 0; i < this.cardSlots.Count; i++)
            {
                this.cardSlots[i].transform.SetParent(this.unactiveCardsParent, false);
                this.cardSlots[i].Hide();
                this.cardSlots[i].OnCliked -= FlipCard;
            }
            this.cardSlots.Clear();
        }

        /// <summary>
        /// Заполнить ui колоду игрока.
        /// </summary>
        private void FillPlayersDeck()
        {
            if (this.cardSlots == null)
            {
                CreateDeck();
            }
             
            ClearDeck();
            UICardWithTreasureSlot currentSlot = null;
            TreasureAndStartPointsType[] treasures = this.gameModel.currentPlayer.treasuresOfThisDeck;
            foreach (TreasureAndStartPointsType treasure in treasures)
            {
                //Точка старта записана для простоты как сокровище,
                //Потому ее надо пропустить и показывать только карты с сокровищем.
                if ((Int32)treasure > treasure.GetMaximalNumberStartPoint())
                {
                    currentSlot = this.gameManager.uiCardWithTreasureSlotProvider.GetCardSlot(treasure);
                    this.cardSlots.Add(currentSlot);
                    currentSlot.Close();
                    currentSlot.transform.SetParent(this.activeCardsParent, false);
                    currentSlot.OnCliked += FlipCard;
                }
            }
        }
        /// <summary>
        /// Перевернуть карту.
        /// </summary>
        private void FlipCard()
        {
            if (this.lastCard.isCardBackUp)
            {
                this.lastCard.Open();
            }
            else
            {
                this.lastCard.Close();
            }
        }


        protected override void Subscribe()
        {
            base.Subscribe();
            this.gameModel.onPlayerChanged += FillPlayersDeck;
        }
        protected override void Unsubscribe()
        {
            this.gameModel.onPlayerChanged -= FillPlayersDeck;
            base.Unsubscribe();
        }
        public override void Initialized()
        {
            base.Initialized();
            FillPlayersDeck();
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
        }
    }
}
