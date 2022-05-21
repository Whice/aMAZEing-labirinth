using Assets.Scripts.GameModel.PlayingField.Treasures;
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

        /// <summary>
        /// «аполнить ui колоду игрока.
        /// </summary>
        private void FillPlayersDeck()
        {
            UICardWithTreasureSlot currentSlot = null;
            TreasureAndStartPointsType[] treasures = this.gameModel.currentPlayer.treasuresOfThisDeck;
            this.cardSlots = new List<UICardWithTreasureSlot>(treasures.Length);
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

            FillPlayersDeck();
        }
    }
}
