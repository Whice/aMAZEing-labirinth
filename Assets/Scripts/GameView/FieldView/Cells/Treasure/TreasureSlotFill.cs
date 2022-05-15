using Assets.Scripts.GameModel.PlayingField.Treasures;
using System;
using UnityEngine;

namespace Assets.Scripts.GameView
{
    /// <summary>
    /// Скрипт заполнения и управления представлением сокровища.
    /// </summary>
    public class TreasureSlotFill : GameViewOriginScript
    {
        /// <summary>
        /// Объект сокровища в слоте.
        /// </summary>
        private GameObject treasureObject = null;
        [SerializeField]
        private Transform model3DSlot=null;
        public void SetTreasure(TreasureAndStartPointsType type)
        {
            Int32 typeID = (Int32)type;
            if (typeID >= type.GetMinimalNumberTreasure() && typeID <= type.GetMaximalNumberTreasure())
            {
                this.treasureObject = GameManager.instance.treasureProvider.GetPrefabClone(typeID);
                this.treasureObject.transform.parent = this.model3DSlot;
                this.treasureObject.transform.localPosition = Vector3.zero;
                this.transform.gameObject.SetActive(true);
            }
        }
    }
}