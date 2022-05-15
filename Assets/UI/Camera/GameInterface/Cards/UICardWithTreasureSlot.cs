using System;
using System.Collections.Generic;
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

        /// <summary>
        /// Карта лежит рубашкой вверх.
        /// По умолчанию true.
        /// </summary>
        private Boolean isCardBackUp = true;
    }
}
