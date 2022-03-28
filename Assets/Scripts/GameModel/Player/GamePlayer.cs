using Assets.Scripts.GameModel.CardDeck;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Assets.Scripts.GameModel.Player
{
    /// <summary>
    /// Игрок в партии.
    /// </summary>
    public class GamePlayer
    {
        /// <summary>
        /// Имя или прозвище игрока.
        /// </summary>
        public readonly String name;
        /// <summary>
        /// Цвет игрока.
        /// </summary>
        public readonly Color color;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">Имя или прозвище игрока.</param>
        /// <param name="color">Цвет игрока.</param>
        /// <param name="cards">Карты для колоды игрока.</param>
        /// <param name="positionX">Местоположение по оси X.</param>
        /// <param name="positionY">Местоположение по оси Y.</param>
        public GamePlayer(String name, Color color, List<Card> cards, Int32 positionX,Int32 positionY)
        {
            this.name = name;
            this.color = color;
            this.cardDeck = cards;
            this.positionX = positionX;
            this.positionY = positionY;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">Имя или прозвище игрока.</param>
        /// <param name="color">Цвет игрока.</param>
        /// <param name="cards">Карты для колоды игрока.</param>
        /// <param name="position">Местоположение.</param>
        public GamePlayer(String name, Color color, List<Card> cards, Point position)
            : this(name, color, cards, position.X, position.Y) { }


        #region Местоположение.

        /// <summary>
        /// Местоположение на поле по оси X.
        /// </summary>
        public Int32 positionX;
        /// <summary>
        /// Местоположение на поле по оси Y.
        /// </summary>
        public Int32 positionY;
        /// <summary>
        /// Местоположение на поле.
        /// </summary>
        public Point position
        {
            get
            {
                return new Point(this.positionX, this.positionY);
            }
            set
            {
                this.positionX = value.X;
                this.positionY = value.Y;
            }
        }

        #endregion Местоположение.

        #region Колода карт игрока.

        /// <summary>
        /// Колода карт игрока.
        /// </summary>
        private List<Card> cardDeck = null;
        /// <summary>
        /// Карта, сокровище которой надо игроку найти.
        /// </summary>
        public Card cardForSearch
        {
            get=>this.cardDeck[cardDeck.Count - 1];
        }
        /// <summary>
        /// Количество карт в колоде.
        /// </summary>
        public Int32 countCardInDeck
        {
            get => this.cardDeck.Count;
        }
        /// <summary>
        /// Вытащить и выдать текущую карту для поиска. 
        /// После этого она исчезнет из спика искомых карт и карта для поиска сменится на следующую.
        /// </summary>
        /// <returns></returns>
        public Card PopCurrentCardForSearch()
        {
            Card cardForPop = this.cardDeck[cardDeck.Count - 1];
            this.cardDeck.RemoveAt(cardDeck.Count - 1);
            return cardForPop;
        }

        #endregion Колода карт игрока.

        /// <summary>
        ///  Создать глубокую копию.
        /// </summary>
        /// <returns></returns>
        public GamePlayer Clone()
        {
            List<Card> cards = new List<Card>(this.cardDeck.Count);
            for(Int32 i=0;i<this.cardDeck.Count;i++)
            {
                cards.Add(this.cardDeck[i].Clone());
            }
            return new GamePlayer(this.name, this.color, cards, this.positionX, this.positionY);
        }

        #region Сравнение.

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj is GamePlayer otherPlayer)
            {
                if (this.name != otherPlayer.name)
                    return false;
                if (this.color != otherPlayer.color)
                    return false;

                if (this.cardDeck.Count != otherPlayer.cardDeck.Count)
                {
                    return false;
                }
                else
                {
                    for (Int32 i = 0; i < this.cardDeck.Count; i++)
                    {
                        if (cardDeck[i] != otherPlayer.cardDeck[i])
                            return false;
                    }
                }

                return true;
            }

            return false;
        }
        public override Int32 GetHashCode()
        {
            Int32 hashCode = this.color.GetHashCode();
            hashCode ^= this.name.GetHashCode();
            for (Int32 i = 0; i < this.cardDeck.Count; i++)
            {
                hashCode ^= this.cardDeck[i].GetHashCode();
            }
            
            return hashCode;
        }
        public static bool operator ==(GamePlayer l, GamePlayer r)
        {
            if (l is null && r is null)
                return true;
            else if (l is null)
                return false;
            else
                return l.Equals(r);
        }
        public static bool operator !=(GamePlayer l, GamePlayer r)
        {
            return !(l == r);
        }

        #endregion Сравнение.
    }
}
