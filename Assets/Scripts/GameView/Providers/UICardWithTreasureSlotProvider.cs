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

        /// <summary>
        /// Получить слот с картой из пулла.
        /// Если в пулле нет слота, то он создается и добавляется в пулл.
        /// </summary>
        /// <param name="treasureType">Тип сокровища, для которого надо получить слот.</param>
        /// <returns></returns>
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
