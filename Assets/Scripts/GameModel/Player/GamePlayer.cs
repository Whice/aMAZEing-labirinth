using Assets.Scripts.GameModel.Cards;
using Assets.Scripts.GameModel.PlayingField;
using Assets.Scripts.GameModel.PlayingField.Treasures;
using System;
using System.Drawing;

namespace Assets.Scripts.GameModel.Player
{
    /// <summary>
    /// Игрок в партии.
    /// </summary>
    public class GamePlayer : PlayerInfo
    {
        /// <summary>
        /// Номер игрока.
        /// </summary>
        public readonly Int32 number;
        /// <summary>
        /// Игрок при перемещении ячеек был перемещен на противоположную сторону.
        /// </summary>
        public Boolean isMoveToOppositeSide = false;

        #region Номер игрока в списке победителей.

        /// <summary>
        /// Номер игрока в списке победителей.
        /// </summary>
        private Int32 winnerNumberPrivate = -1;
        /// <summary>
        /// Номер игрока в списке победителей.
        /// </summary>
        public Int32 winnerNumber
        {
            get => this.winnerNumberPrivate;
        }
        /// <summary>
        /// Игрок победил.
        /// </summary>
        private Boolean isWinnerPrivate = false;
        /// <summary>
        /// Игрок победил.
        /// </summary>
        public Boolean isWinner
        {
            get => this.isWinnerPrivate;
        }
        /// <summary>
        /// Установить игрока в качестве победителя и выдать ему место в списке победителей.
        /// </summary>
        /// <param name="numberInWinnerList">Место в списке победителей.</param>
        public void SetPlayerAsWinner(Int32 numberInWinnerList)
        {
            this.winnerNumberPrivate = numberInWinnerList;
            this.isWinnerPrivate = true;
        }

        #endregion Номер игрока в списке победителей.

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">Имя или прозвище игрока.</param>
        /// <param name="color">Цвет игрока.</param>
        /// <param name="cardDeck">Карты для колоды игрока.</param>
        /// <param name="positionX">Местоположение по оси X.</param>
        /// <param name="positionY">Местоположение по оси Y.</param>
        public GamePlayer(String name, Color color, CardDeck cardDeck, Int32 positionX, Int32 positionY, Int32 playerNumer)
            : base(name, color)
        {
            this.cardDeck = cardDeck;
            this.number = playerNumer;
            SetPosition(positionX, positionY);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="info">Содержит информацию об игроке, которая не относиться к игровой логике.</param>
        /// <param name="cardDeck">Карты для колоды игрока.</param>
        /// <param name="positionX">Местоположение по оси X.</param>
        /// <param name="positionY">Местоположение по оси Y.</param>
        public GamePlayer(PlayerInfo info, CardDeck cardDeck, Int32 positionX, Int32 positionY, Int32 playerNumer)
            : this(info.name, info.color, cardDeck, positionX, positionY, playerNumer) { }


        #region Местоположение.

        /// <summary>
        /// Размер игрового поля.
        /// </summary>
        private Int32 FIELD_SIZE
        {
            get => Field.FIELD_SIZE;
        }


        /// <summary>
        /// Движение аватара игрока.
        /// </summary>
        /// <param name="fromX">Позиция по x, откуда происходит перемещение.</param>
        /// <param name="fromY">Позиция по y, откуда происходит перемещение.</param>
        /// <param name="toX">Позиция по x, куда происходит перемещение.</param>
        /// <param name="toY">Позиция по y, куда происходит перемещение.</param>
        /// <param name="playerNumber">Номер игрока, который сделал ход.</param>
        public delegate void OnAvatarMove(Int32 fromX, Int32 fromY, Int32 toX, Int32 toY, Int32 playerNumber);
        /// <summary>
        /// Аватар игрока был передвинут.
        /// </summary>
        public event OnAvatarMove onAvatarMoved;
        /// <summary>
        /// Местоположение на поле по оси X.
        /// </summary>
        private Int32 positionXPrivate;
        /// <summary>
        /// Местоположение на поле по оси X.
        /// <br/>Игрок сам следит за "выходом" за пределы поля и 
        /// пересталяет на другую его сторону свою фишку, если надо.
        /// </summary>
        public Int32 positionX
        {
            get
            {
                return this.positionXPrivate;
            }
        }
        /// <summary>
        /// Местоположение на поле по оси Y.
        /// </summary>
        private Int32 positionYPrivate;
        /// <summary>
        /// Местоположение на поле по оси Y.
        /// <br/>Игрок сам следит за "выходом" за пределы поля и 
        /// пересталяет на другую его сторону свою фишку, если надо.
        /// </summary>
        public Int32 positionY
        {
            get
            {
                return this.positionYPrivate;
            }
        }
        /// <summary>
        /// Местоположение на поле.
        /// </summary>
        public Point position
        {
            get
            {
                return new Point(this.positionX, this.positionY);
            }
        }
        /// <summary>
        /// Установить новое местоположение для игрока.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void SetPosition(Int32 x, Int32 y)
        {

            Int32 oldPositionX = this.positionXPrivate;
            Int32 oldPositionY = this.positionYPrivate;

            //Если положение игрока выходит за пределы поля,
            //то он появляется с другой стороны.
            {
                //Проверка по X
                if (x >= this.FIELD_SIZE)
                {
                    this.positionXPrivate = 0;
                    this.isMoveToOppositeSide = true;
                }
                else if (x < 0)
                {
                    this.positionXPrivate = this.FIELD_SIZE - 1;
                    this.isMoveToOppositeSide = true;
                }
                else
                {
                    this.positionXPrivate = x;
                }


                //Проверка по Y
                if (y >= this.FIELD_SIZE)
                {
                    this.positionYPrivate = 0;
                    this.isMoveToOppositeSide = true;
                }
                else if (y < 0)
                {
                    this.positionYPrivate = this.FIELD_SIZE - 1;
                    this.isMoveToOppositeSide = true;
                }
                else
                {
                    this.positionYPrivate = y;
                }
            }


            //Если положение игрока изменилось, то надо сообщить.
            if (oldPositionX != this.positionX || oldPositionY != this.positionY)
            {
                this.onAvatarMoved?.Invoke(oldPositionX, oldPositionY, this.positionX, this.positionY, this.number);
            }
        }

        #endregion Местоположение.

        #region Колода карт игрока.

        /// <summary>
        /// Колода карт игрока.
        /// </summary>
        private CardDeck cardDeck = null;
        /// <summary>
        /// Карта, сокровище которой надо игроку найти.
        /// </summary>
        public Card cardForSearch
        {
            get => this.cardDeck.topCard;
        }
        /// <summary>
        /// Сокровища в картах колоды этого игрока.
        /// </summary>
        public TreasureAndStartPointsType[] treasuresOfThisDeck
        {
            get => this.cardDeck.treasuresOfThisDeck;

        }
        /// <summary>
        /// Количество карт в колоде.
        /// </summary>
        public Int32 countCardInDeck
        {
            get => this.cardDeck.count;
        }
        /// <summary>
        /// Вытащить и выдать текущую карту для поиска. 
        /// После этого она исчезнет из спика искомых карт и карта для поиска сменится на следующую.
        /// </summary>
        /// <returns></returns>
        public Card PopCurrentCardForSearch()
        {
            return this.cardDeck.Pop();
        }

        #endregion Колода карт игрока.

        /// <summary>
        ///  Создать глубокую копию.
        /// </summary>
        /// <returns></returns>
        public new GamePlayer Clone()
        {
            CardDeck cardDeck = this.cardDeck.Clone();
            return new GamePlayer(base.Clone(), cardDeck, this.positionX, this.positionY, number);
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

                if (this.cardDeck != otherPlayer.cardDeck)
                {
                    return false;
                }

                return true;
            }

            return false;
        }
        public override Int32 GetHashCode()
        {
            Int32 hashCode = this.color.GetHashCode();
            hashCode ^= this.name.GetHashCode();
            hashCode ^= this.cardDeck.GetHashCode();

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
