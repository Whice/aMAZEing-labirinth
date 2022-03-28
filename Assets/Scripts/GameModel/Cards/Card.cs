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
            if((Int32)treasure<treasure.GetMinimalNumberTreasure() || (Int32)treasure>treasure.GetMaximalNumberTreasure())
            {
                throw new ArgumentException("Карте должен быть задан тип сокровища!\nThe card must be set to a treasure type!");
            }

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

        /// <summary>
        ///  Создать глубокую копию.
        /// </summary>
        /// <returns></returns>
        public Card Clone()
        {
           return new Card(this.treasure, this.isOpen); 
        }

        #region Сравнение.

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj is Card otherPlayer)
            {
                if (this.treasure != otherPlayer.treasure)
                    return false;

                return true;
            }

            return false;
        }
        public override Int32 GetHashCode()
        {
            return (Int32)this.treasure;
        }
        public static bool operator ==(Card l, Card r)
        {
            if (l is null && r is null)
                return true;
            else if (l is null)
                return false;
            else
                return l.Equals(r);
        }
        public static bool operator !=(Card l, Card r)
        {
            return !(l == r);
        }

        #endregion Сравнение.
    }
}
