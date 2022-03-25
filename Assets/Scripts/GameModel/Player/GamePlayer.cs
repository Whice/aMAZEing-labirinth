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
        public GamePlayer(String name, Color color, Card[] cards)
        {
            this.name = name;
            this.color = color;
            this.cardDeck = new List<Card>(cards);
        }

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
    }
}
