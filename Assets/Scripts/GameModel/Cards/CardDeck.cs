using Assets.Scripts.Extensions;
using Assets.Scripts.GameModel.PlayingField.Treasures;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.GameModel.Cards
{
    /// <summary>
    /// Колода карт.
    /// </summary>
    public class CardDeck
    {
        #region Конструкторы.

        /// <summary>
        /// Создать колоду из всех карт.
        /// </summary>
        public CardDeck()
        {
            Int32 maxNumber = TreasureAndStartPointsType.empty.GetMaximalNumberTreasure();
            Int32 minNumber = TreasureAndStartPointsType.empty.GetMinimalNumberTreasure();
            this.cards = new List<Card>(maxNumber - minNumber);

            for (Int32 i = minNumber; i <= maxNumber; i++)
            {
                this.cards.Add(new Card((TreasureAndStartPointsType)i));
            }
            this.lastCardNumber = this.cards.Count - 1;
        }
        /// <summary>
        /// Создать колоду из предоставленых карт.
        /// </summary>
        /// <param name="cards">Список карт для колоды.</param>
        public CardDeck(List<Card> cards)
        {
            this.cards = cards;
            this.lastCardNumber = this.cards.Count - 1;
        }

        #endregion Конструкторы.

        #region Данные.

        /// <summary>
        /// Количество карт в колоде.
        /// </summary>
        public Int32 count
        {
            get => this.lastCardNumber + 1;
        }
        /// <summary>
        /// Пуста ли колода.
        /// </summary>
        public Boolean isEmpty
        {
            get => this.count == 0;
        }
        /// <summary>
        /// Список карт.
        /// </summary>
        List<Card> cards = null;
        /// <summary>
        /// Верхняя карта в колоде.
        /// <br/>Эта карта будет вытащена из колоды первой.
        /// </summary>
        public Card topCard
        {
            get => this.cards[this.lastCardNumber];
        }
        /// <summary>
        /// Номер последней карты.
        /// </summary>
        private Int32 lastCardNumber = -1;

        #endregion Данные.

        #region Действия.

        /// <summary>
        /// Перемешать колоду.
        /// </summary>
        public void Shuffle()
        {
            this.cards.Shuffle();
        }
        /// <summary>
        /// Вытащить из колоды одну карту.
        /// </summary>
        /// <returns>Карту или null, если карты закончились.</returns>
        public Card Pop()
        {
            if (this.lastCardNumber > -1)
            {
                --this.lastCardNumber;
                return this.cards[this.lastCardNumber + 1];
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// Вытащить из колоды несколько карт.
        /// </summary>
        /// <param name="count">Количество карт, которое требуется вытащить.</param>
        /// <returns>Список карт или null, если столько карт нет.</returns>
        public List<Card> Pop(Int32 count)
        {
            if (count < this.lastCardNumber + 1)
            {
                List<Card> popCards = new List<Card>(count);
                for (int i = 0; i < count; i++)
                {
                    popCards.Add(this.Pop());
                }
                return popCards;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// Глубокий клон колоды.
        /// </summary>
        /// <returns></returns>
        public CardDeck Clone()
        {
            List<Card> cards = new List<Card>(this.cards.Count);
            for (Int32 i = 0; i < this.count; i++)
            {
                cards.Add(this.cards[i].Clone());
            }
            return new CardDeck(cards);
        }

        #endregion Действия.

        #region Сравнение.

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj is CardDeck otherDeck)
            {
                if (otherDeck.count != this.count)
                {
                    return false;
                }

                for (Int32 i = 0; i < this.count; i++)
                {
                    if (!this.cards[i].Equals(otherDeck.cards[i]))
                    {
                        return false;
                    }
                }

                return true;
            }

            return false;
        }
        public override Int32 GetHashCode()
        {
            Int32 hashCode = 0;
            for (Int32 i = 0; i < this.count; i++)
            {
                hashCode ^= this.cards[i].GetHashCode();
            }

            return hashCode;
        }
        public static bool operator ==(CardDeck l, CardDeck r)
        {
            if (l is null && r is null)
                return true;
            else if (l is null)
                return false;
            else
                return l.Equals(r);
        }
        public static bool operator !=(CardDeck l, CardDeck r)
        {
            return !(l == r);
        }

        #endregion Сравнение.
    }
}
