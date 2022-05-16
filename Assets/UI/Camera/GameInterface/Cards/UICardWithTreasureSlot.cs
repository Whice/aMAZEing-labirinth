using Assets.Scripts.GameModel.PlayingField.Treasures;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    /// <summary>
    /// Слот для карты с сокровищем.
    /// </summary>
    public class UICardWithTreasureSlot : GameViewOriginScript
    {
        #region Внешние даные.

        /// <summary>
        /// Рубашка карты.
        /// </summary>
        [SerializeField]
        private GameObject cardBack = null;
        /// <summary>
        /// Лицо карты.
        /// </summary>
        [SerializeField]
        private GameObject cardFace = null;
        /// <summary>
        /// Слот картинки сокровища.
        /// </summary>
        [SerializeField]
        private Image treasureSlot = null;

        #endregion Внешние даные.

        #region Переворачивание карты рубашкой вверх или вниз.

        /// <summary>
        /// Карта лежит рубашкой вверх.
        /// По умолчанию true.
        /// </summary>
        private Boolean isCardBackUpPrivate = true;
        /// <summary>
        /// Карта лежит рубашкой вверх.
        /// По умолчанию true.
        /// </summary>
        public Boolean isCardBackUp
        {
            get
            {
                return this.isCardBackUpPrivate;
            }
            set
            {
                this.isCardBackUpPrivate = value;
                this.cardBack.SetActive(value);
                this.cardFace.SetActive(!value);
            }
        }
        /// <summary>
        /// Открыть карту.
        /// Перевернуть карту лицом вверх.
        /// </summary>
        public void Open()
        {
            if (this.isCardBackUp)
            {
                this.isCardBackUp = false;
            }
        }
        /// <summary>
        /// Закрыть карту.
        /// Перевернуть карту рубашкой вверх.
        /// </summary>
        public void Close()
        {
            if (!this.isCardBackUp)
            {
                this.isCardBackUp = true;
            }
        }

        #endregion Переворачивание карты рубашкой вверх или вниз.

        /// <summary>
        /// Задать сокровище этой карте.
        /// </summary>
        /// <param name="treasure"></param>
        public void SetTreasure(TreasureAndStartPointsType treasure)
        {
            Int32 treasureID = (Int32)treasure;
            if (treasure.GetMinimalNumberTreasure() <= treasureID && treasure.GetMaximalNumberTreasure() >= treasureID)
            {
                this.treasureSlot.sprite = GameManager.instance.treasureSpriteProvider.GetSpriteClone(treasureID);
            }
        }
    }
}
