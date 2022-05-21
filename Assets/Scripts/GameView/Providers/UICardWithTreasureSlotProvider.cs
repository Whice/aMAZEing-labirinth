using Assets.Scripts.GameModel.PlayingField.Treasures;
using System;
using System.Collections.Generic;
using UI;
using UnityEngine;


namespace Assets.Scripts.GameView
{
    [Serializable]
    [CreateAssetMenu(fileName = "UICardWithTreasureSlot Provider", menuName = "Game view/UICardWithTreasureSlot Provider")]
    public class UICardWithTreasureSlotProvider : GameObjectProvider
    {
        private Dictionary<TreasureAndStartPointsType, UICardWithTreasureSlot> pull
            = new Dictionary<TreasureAndStartPointsType, UICardWithTreasureSlot>(30);


        public UICardWithTreasureSlot GetCardSlot(TreasureAndStartPointsType treasureType)
        {
            if (!this.pull.ContainsKey(treasureType))
            {
                GameObject newSlotObject = base.GetPrefabClone(nameof(UICardWithTreasureSlot));
                UICardWithTreasureSlot newSlot = newSlotObject.GetComponent<UICardWithTreasureSlot>();
                newSlot.SetTreasure(treasureType);
                this.pull.Add(treasureType, newSlot);
            }

            this.pull[treasureType].gameObject.SetActive(true);
            return this.pull[treasureType];
        }
    }
}
