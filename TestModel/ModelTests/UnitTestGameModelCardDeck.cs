using Assets.Scripts.GameModel.Cards;
using Assets.Scripts.GameModel.PlayingField.Treasures;
using System;
using System.Collections.Generic;
using Xunit;

namespace TestModel.ModelTests
{
    /// <summary>
    /// Класс тестирования колоды карт.
    /// </summary>
    public class UnitTestGameModelCardDeck
    {
        /// <summary>
        /// Создание колоды карт. Результат - удачное создание с ожидаемыми данными.
        /// </summary>
        [Fact]
        public void TestCreate_CreateDeck_SuccessfullCreate()
        {
            CardDeck deck = new CardDeck();

            TreasureAndStartPointsType type = TreasureAndStartPointsType.empty;
            Int32 countCardWithTreasure = type.GetMaximalNumberTreasure() - type.GetMinimalNumberTreasure() + 1;
            Assert.Equal(countCardWithTreasure, deck.count);
            Assert.False(deck.isEmpty);
        }
        /// <summary>
        /// Удачное извлечение из колоды.
        /// </summary>
        [Fact]
        public void TestPop_PopDeck_SuccessfullPop()
        {
            CardDeck deck = new CardDeck();

            Int32 countBeforePop = deck.count;
            Card topCard = deck.topCard;
            Card popCard = deck.Pop();

            Assert.Equal(topCard, popCard);
            Assert.NotEqual(topCard, deck.topCard);
            Assert.Equal(countBeforePop-1, deck.count);
            Assert.False(deck.isEmpty);
        }
        /// <summary>
        /// Удачное извлечение из колоды множества карт.
        /// </summary>
        [Fact]
        public void TestPop_MultyPopDeck_SuccessfullPop()
        {
            CardDeck deck = new CardDeck();

            Int32 countBeforePop = deck.count;
            Card topCard = deck.topCard;
            List<Card> popCards = deck.Pop(countBeforePop);

            Assert.Equal(countBeforePop, popCards.Count);
            Assert.NotEqual(topCard, deck.topCard);
            Assert.Equal(0, deck.count);
            Assert.True(deck.isEmpty);
        }
        /// <summary>
        /// Удачное клонирование.
        /// </summary>
        [Fact]
        public void TestClone_CloneDeck_SuccessfullClone()
        {
            CardDeck deck = new CardDeck();
            CardDeck clone = deck.Clone();

            Assert.Equal(clone.isEmpty, deck.isEmpty);
            Assert.Equal(clone.count, deck.count);
            Assert.Equal(clone.topCard, deck.topCard);


            List<Card> deckCards = new List<Card>(deck.count);
            for(Int32 i = 0; i < deck.count; i++)
            {
                deckCards.Add(deck.Pop());
            }

            Assert.Equal(deckCards.Count, deck.count);

            List<Card> cloneCards = new List<Card>(clone.count);
            for(Int32 i = 0; i < clone.count; i++)
            {
                cloneCards.Add(clone.Pop());
            }

            Assert.Equal(cloneCards.Count, clone.count);

            for(Int32 i=0; i < cloneCards.Count; i++)
            {
                Assert.Equal(cloneCards[i], deckCards[i]);
            }
        }
        /// <summary>
        /// Удачное перемешивание, т.е. без вылетов. Других ожиданий не может быть.
        /// </summary>
        [Fact]
        public void TestShuffle_DeckShuffle_SuccessfullShuffle()
        {
            CardDeck deck = new CardDeck();
            Int32 count = deck.count;

            deck.Shuffle();

            Assert.Equal(count, deck.count);
        }
    }
}
