using Assets.Scripts.GameModel.PlayingField.Treasures;
using System;

namespace Assets.Scripts.GameModel.CardDeck
{
    /// <summary>
    /// Катра для игры.
    /// </summary>
    public class Card
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="treasure">Сокровище для этой карты.</param>
        /// <param name="isOpen">Открыта ли карта, по умолчанию нет.</param>
        public Card(TreasureAndStartPointsType treasure, Boolean isOpen = false)
        {
            this.treasure = treasure;
            this.isOpenField = isOpen;
        }

        /// <summary>
        /// Сокровище, с которым связана эта карта.
        /// </summary>
        public readonly TreasureAndStartPointsType treasure = TreasureAndStartPointsType.unknown;
        /// <summary>
        /// Раскрыта ли карта.
        /// <br/>Если карта та раскрыта, значит она лежит "рубашкой" вниз.
        /// </summary>
        private Boolean isOpenField = false;
        /// <summary>
        /// Раскрыта ли карта.
        /// <br/>Если карта та раскрыта, значит она лежит "рубашкой" вниз.
        /// </summary>
        public Boolean isOpen
        {
            get => this.isOpenField;
        }
        /// <summary>
        /// Перевернуть карту.
        /// </summary>
        public void FlipCard()
        {
            this.isOpenField = !this.isOpenField;
        }
    }
}
