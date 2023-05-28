using Assets.Scripts.GameModel.PlayingField.Treasures;
using System;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.GameView
{
    /// <summary>
    /// Скрипт заполнения и управления представлением сокровища.
    /// </summary>
    public class TreasureSlotFill : GameWorldViewOriginScript
    {
        /// <summary>
        /// Объект сокровища в слоте.
        /// </summary>
        private GameObject treasureObject = null;
        /// <summary>
        /// Слот для объемной моделки сокровища.
        /// </summary>
        [SerializeField]
        private Transform model3DSlot=null;
        /// <summary>
        /// Слот для иконки сокровища.
        /// </summary>
        [SerializeField]
        private SpriteRenderer treasureIconSlot=null;
        /// <summary>
        /// Установить сокровище в слот.
        /// </summary>
        /// <param name="type"></param>
        public void SetTreasure(TreasureAndStartPointsType type)
        {
            Int32 typeID = (Int32)type;
            if (typeID >= type.GetMinimalNumberTreasure() && typeID <= type.GetMaximalNumberTreasure())
            {
                this.treasureObject = this.gameManager.treasureProvider.GetPrefabClone(typeID);
                this.treasureObject.transform.parent = this.model3DSlot;
                this.treasureObject.transform.localPosition = Vector3.zero;
                //delete
                this.treasureObject.SetActive(false);
                this.treasureIconSlot.sprite = this.gameManager.treasureSpriteProvider.GetSpriteClone(typeID);
                this.transform.gameObject.SetActive(true);
            }
        }
    }
}