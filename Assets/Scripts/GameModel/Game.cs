using Assets.Scripts.GameModel.Cards;
using Assets.Scripts.GameModel.Player;
using Assets.Scripts.GameModel.PlayingField;
using Assets.Scripts.GameModel.PlayingField.FieldCells;
using Assets.Scripts.GameModel.PlayingField.Treasures;
using Assets.Scripts.GameModel.TurnPhaseAndExtensions;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.GameModel
{
    /// <summary>
    /// Модель игры.
    /// </summary>
    public class Game
    {
        /// <summary>
        /// Создание игры. Не означает ее начало.
        /// <br/>Начинается она только методом <see cref="Start(PlayerInfo[])"/>,
        /// который вернет true при удачном старте.
        /// </summary>
        public Game()
        {
        }

        #region Данные игры.

        /// <summary>
        /// Колода карт, сокровища которых были найдены. Тут не может быть стартовых точек.
        /// </summary>
        private CardDeck deckPrivate = null;
        /// <summary>
        /// Колода карт, сокровища которых были найдены. Тут не может быть стартовых точек.
        /// </summary>
        public CardDeck deck
        {
            get => this.deckPrivate;
        }
       
        /// <summary>
        /// Текущая фаза хода игрока.
        /// </summary>
        private TurnPhase currentPhasePrivate = TurnPhase.unknow;
        /// <summary>
        /// Текущая фаза хода игрока.
        /// </summary>
        public TurnPhase currentPhase
        {
            get => this.currentPhasePrivate;
        }
        /// <summary>
        /// Игровое поле.
        /// </summary>
        private Field fieldPrivate = null;
        /// <summary>
        /// Игровое поле.
        /// </summary>
        public Field field
        {
            get => this.fieldPrivate;
        }
        /// <summary>
        /// Свободная ячейка.
        /// </summary>
        public FieldCell freeCell
        {
            get => this.field.freeFieldCell;
        }

        #endregion Данные игры.

        #region Действия.


        #region Конец игры.

        /// <summary>
        /// Игра окончилась.
        /// </summary>
        private Boolean isEndPrivate = false;
        /// <summary>
        /// Игра окончилась.
        /// </summary>
        public Boolean isEnd
        {
            get
            {
                if (this.isEndPrivate == false)
                {
                    Int32 winnersCount = 0;
                    foreach (GamePlayer player in this.playersPrivate)
                    {
                        if (player.isWinner)
                        {
                            ++winnersCount;
                        }
                    }

                    //Если играть остается один игрок или никого, то игра кончается.
                    if (winnersCount + 1 >= this.playersPrivate.Length)
                    {
                        this.isEndPrivate = true;
                    }
                }

                return this.isEndPrivate;
            }
        }
        /// <summary>
        /// Получить копию списка игроков.
        /// </summary>
        /// <returns></returns>
        private GamePlayer[] GetPlayersListClone()
        {
            GamePlayer[] gamePlayers = new GamePlayer[this.playersPrivate.Length];
            for(Int32 i = 0; i < gamePlayers.Length; i++)
            {
                gamePlayers[i] = this.playersPrivate[i].Clone();
            }
            return gamePlayers;
        }
        /// <summary>
        /// Сравнитель победителей.
        /// <br/>Сравнение идет честно для тех, кто уже победил.
        /// А для тех, кто еще не победил происходит нечестное сравнение:
        /// <br/>Сперва сравнивается, у кого больше осталось карт. У кого больше, тот ниже местом.
        /// <br/>Если же их одинакого, то сравнивается хеш-код имени. У кого больше, тот выше местом.
        /// </summary>
        private class WinnerComparer : IComparer<GamePlayer>
        {
            /// <summary>
            /// Сравнение идет честно для тех, кто уже победил.
            /// А для тех, кто еще не победил происходит нечестное сравнение:
            /// <br/>Сперва сравнивается, у кого больше осталось карт. У кого больше, тот ниже местом.
            /// <br/>Если же их одинакого, то сравнивается хеш-код имени. У кого больше, тот выше местом.
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <returns></returns>
            public int Compare(GamePlayer x, GamePlayer y)
            {
                if (x.winnerNumber > y.winnerNumber)
                {
                    return 1;
                }
                else if (x.winnerNumber < y.winnerNumber)
                {
                    return -1;
                }
                else
                {
                    if (x.countCardInDeck > y.countCardInDeck)
                    {
                        return -1;
                    }
                    else if (x.countCardInDeck < y.countCardInDeck)
                    {
                        return 1;
                    }
                    else
                    {
                        if (x.name.GetHashCode() > y.name.GetHashCode())
                        {
                            return 1;
                        }
                        else
                        {
                            return -1;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Посчитать список победителей.
        /// Массив сортируется специальным образом.
        /// </summary>
        private GamePlayer[] CalculateWinnerPlace()
        {
            GamePlayer[] gamePlayers = GetPlayersListClone();
            Array.Sort(gamePlayers, new WinnerComparer());
            return gamePlayers;
        }
        /// <summary>
        /// Получить список победителей.
        /// </summary>
        /// <returns></returns>
        public GamePlayer[] GetWinners()
        {
            return CalculateWinnerPlace();
        }
            

        #endregion Конец игры.

        #region Во время игры.

        #region Предыдущий ход.

        /// <summary>
        /// Номер линии, которая двигалась в предыдущем ходу.
        /// </summary>
        private Int32 lastNumberLine = 0;
        /// <summary>
        /// Сторона поля, с которой выполнялся сдвиг в пердыдущем ходу.
        /// </summary>
        private FieldSide lastFieldSide = FieldSide.unknow;
        /// <summary>
        /// Проверить, не отменяет ли нынешний ход предыдущий.
        /// </summary>
        /// <param name="numberLine">Номер линии, по которой выполняется сдвиг в этом ходу.</param>
        /// <param name="side">Сторона поля, с которой выполняется сдвиг в этом ходу.</param>
        /// <returns></returns>
        private Boolean IsNotUndoPreviousMove(Int32 numberLine, FieldSide side)
        {
            if(numberLine == this.lastNumberLine)
            {
                if((Int32)side + (Int32)this.lastFieldSide == 0)
                {
                    return false;
                }
            }
            return true;
        }

        #endregion Предыдущий ход.


        /// <summary>
        /// Номер следующего победителя.
        /// </summary>
        private Int32 numberOfNextWinner = 0;
        /// <summary>
        /// Переход к следующему ходу.
        /// </summary>
        public event Action onNextTurnMoved;

        /// <summary>
        /// Движение аватара игрока.
        /// </summary>
        /// <param name="fromX">Позиция по x, откуда происходит перемещение.</param>
        /// <param name="fromY">Позиция по y, откуда происходит перемещение.</param>
        /// <param name="toX">Позиция по x, куда происходит перемещение.</param>
        /// <param name="toY">Позиция по y, куда происходит перемещение.</param>
        public delegate void OnAvatarMove(Int32 fromX, Int32 fromY, Int32 toX, Int32 toY);
        /// <summary>
        /// Аватар игрока был передвинут.
        /// </summary>
        public event OnAvatarMove onAvatarMoved;

        /// <summary>
        /// Поставить свободную ячейку на поле сдвинув линию. 
        /// </summary>
        /// <param name="numberLine">Номер линии, куда вставить ячейку.</param>
        /// <param name="side">Сторона поля, куда вставить ячейку.</param>
        /// <returns></returns>
        public Boolean SetFreeCellToField(Int32 numberLine, FieldSide side)
        {
            Boolean successfulMove = false;

            if (IsNotUndoPreviousMove(numberLine, side))
            {
                switch (side)
                {
                    case FieldSide.top:
                        {
                            successfulMove = this.field.MoveLineUp(numberLine);
                            break;
                        }
                    case FieldSide.right:
                        {
                            successfulMove = this.field.MoveLineRight(numberLine);
                            break;
                        }
                    case FieldSide.bottom:
                        {
                            successfulMove = this.field.MoveLineDown(numberLine);
                            break;
                        }
                    case FieldSide.left:
                        {
                            successfulMove = this.field.MoveLineLeft(numberLine);
                            break;
                        }
                }

                if (successfulMove)
                {
                    //Еслли игра закончилась, то ход уже невозможен.
                    if (this.isEnd)
                    {
                        return false;
                    }
                    else
                    {
                        SetNextPhase();
                    }

                    this.lastNumberLine = numberLine;
                    this.lastFieldSide = side;
                }
            }

            return successfulMove;
        }
        /// <summary>
        /// Попытаться получить сокровище искомое нынешним игроком после его хода.
        /// </summary>
        private void TryGetTreasureFromFieldForCurrentPlayer()
        {
            Int32 currentPlayerX = this.currentPlayer.positionX;
            Int32 currentPlayerY = this.currentPlayer.positionY;

            TreasureAndStartPointsType cellTreasure = this.field[currentPlayerX, currentPlayerY].treasureOrStartPoints;
            TreasureAndStartPointsType playerSearchTreasure = this.currentPlayer.cardForSearch.treasure;

            if (cellTreasure == playerSearchTreasure)
            {
                Card foundCard = this.currentPlayer.PopCurrentCardForSearch();

                //Если последняя найденая карта игрока это стартовая точка, то он победил,
                //Если нет, то она добавляется в список найденых.

                if (foundCard.treasure.IsStartPoint())
                {
                    this.currentPlayer.SetPlayerAsWinner(this.numberOfNextWinner);
                    ++this.numberOfNextWinner;
                }
                else
                {
                    this.deckPrivate.Add(foundCard);
                }
            }
        }
        /// <summary>
        /// Переместить аватар игрока в указанную позицию, если это возможно.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>true, если аватар игрока был перемещен.</returns>
        public Boolean SetPlayerAvatarToField(Int32 x, Int32 y)
        {
            Boolean successfulMove = this.currentPhase == TurnPhase.movingAvatar;
            successfulMove = this.field.IsPossibleMove(this.currentPlayer.positionX, this.currentPlayer.positionY, x, y);


            if (successfulMove)
            {
                Int32 oldPositionX = this.currentPlayer.positionX;
                Int32 oldPositionY = this.currentPlayer.positionY;

                this.currentPlayer.SetPosition(x, y);
                TryGetTreasureFromFieldForCurrentPlayer();

                this.onAvatarMoved?.Invoke(oldPositionX, oldPositionY, x, y);

                //Если игра закончилась, то ход уже невозможен.
                if (this.isEnd)
                {
                    return false;
                }
                else
                {
                    SetNextPhase();
                }
            }

            return successfulMove;
        }
        /// <summary>
        /// Ход перешел к следующему игроку.
        /// </summary>
        public event Action onPlayerChanged;
        /// <summary>
        /// Установить следующего играющего игрока.
        /// <br/>Выбирается следующий игрок из массива, который не победил.
        /// </summary>
        private void SetNextPlayer()
        {
            //Выбирается следующий игрок из массива, который не победил.
            do
            {
                ++this.currentPlayerNumberPrivate;
                if (this.currentPlayerNumberPrivate >= this.playersPrivate.Length)
                {
                    this.currentPlayerNumberPrivate = 0;
                }
            }
            while (this.currentPlayer.isWinner);

            this.onPlayerChanged?.Invoke();
        }
        /// <summary>
        /// Установить следующую фазу, с переходом хода к следующиму игроку.
        /// </summary>
        private void SetNextPhase()
        {
            this.currentPhasePrivate = this.currentPhasePrivate.GetNextPhase();

            if(this.currentPhasePrivate==TurnPhase.movingCell)
            {
                SetNextPlayer();
                this.onNextTurnMoved?.Invoke();
            }
        }

        #endregion Во время игры.

        #region Создание игроков и начало игры.

        /// <summary>
        /// Список игроков.
        /// </summary>
        private GamePlayer[] playersPrivate = null;
        /// <summary>
        /// Список игроков.
        /// </summary>
        public GamePlayer[] players
        {
            get => this.playersPrivate;
        }
        /// <summary>
        /// Количество играющих игроков.
        /// </summary>
        private Int32 countOfPlayersPlayingPrivate = -1;
        /// <summary>
        /// Количество играющих игроков.
        /// </summary>
        public Int32 countOfPlayersPlaying
        {
            get => countOfPlayersPlayingPrivate;
        }
        /// <summary>
        ///  Номер игрока, которому принадлежит текущий ход.
        /// </summary>
        private Int32 currentPlayerNumberPrivate = -1;
        /// <summary>
        ///  Номер игрока, которому принадлежит текущий ход.
        /// </summary>
        public Int32 currentPlayerNumber
        {
            get => this.currentPlayerNumberPrivate;
        }
        /// <summary>
        ///  Игрок, которому принадлежит текущий ход.
        /// </summary>
        public GamePlayer currentPlayer
        {
            get => this.playersPrivate[currentPlayerNumberPrivate];
        }

        /// <summary>
        /// Раздать карты игрокам.
        /// </summary>
        /// <param name="playerInfos">Информация об игроках.</param>
        /// <returns></returns>
        private Boolean DealCardsToPlayers(PlayerInfo[] playerInfos)
        {
            this.playersPrivate = new GamePlayer[playerInfos.Length];
            if (this.playersPrivate != null && this.countOfPlayersPlaying > 0)
            {
                //пол умолчанию колода создается со всеми сокровищами.
                CardDeck deck = new CardDeck();
                deck.Shuffle();

                //выяснить, сколько карт каждому игроку
                Int32 countCardsForOnePlayer = deck.count/this.countOfPlayersPlaying;

                List<Card> startPointAndDeckForPlayer;
                CardDeck deckForPlayer;

                for (Int32 i = 0; i < this.countOfPlayersPlaying; i++)
                {
                    //задать вместимость карты для игрока+точка старта
                    startPointAndDeckForPlayer = new List<Card>(countCardsForOnePlayer + 1);

                    //выдать сперва точку старта, потом все карты для игрока.
                    startPointAndDeckForPlayer.Add(new Card((TreasureAndStartPointsType)(i + 2)));
                    startPointAndDeckForPlayer = deck.Pop(countCardsForOnePlayer);

                    //создать колоду для игрока
                    deckForPlayer = new CardDeck(startPointAndDeckForPlayer);

                    //Выдать координаты
                    Int32 startPointX = this.field.startPointsCoordinate[i].X;
                    Int32 startPointY = this.field.startPointsCoordinate[i].Y;

                    //Создать игровое инфо игрока.
                    this.playersPrivate[i] = new GamePlayer(playerInfos[i], deckForPlayer, startPointX, startPointY);
                }

                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Создать объекты игроков для игры.
        /// </summary>
        /// <param name="playerInfos">Информация об игроках.</param>
        private void FillInfoPlayers(PlayerInfo[] playerInfos)
        {
            //Выбрать начального игрока случайным образом.
            Random random = new Random();
            this.currentPlayerNumberPrivate = random.Next(playerInfos.Length);

            DealCardsToPlayers(playerInfos);
        }
        /// <summary>
        /// Начать игру.
        /// </summary>
        /// <param name="playerInfos">Информация об игроках.</param>
        public Boolean Start(PlayerInfo[] playerInfos, out String errorMessage)
        {
            if (playerInfos == null)
            {
                errorMessage = "Инфо об игроках не может содержать нулевую ссылку!";
                return false;
            }
            else if (playerInfos.Length > 4)
            {
                errorMessage = "В игре не может быть больше 4х игроков!";
                return false;
            }
            else if (playerInfos.Length < 2)
            {
                errorMessage = "В игре не может быть меьше 2х игроков!";
                return false;
            }

            for(Int32 i=0; i < playerInfos.Length; i++)
            {
                for(Int32 j=i+1; j < playerInfos.Length; j++)
                {
                    if(playerInfos[i].name == playerInfos[j].name)
                    {
                        errorMessage = "Игроки не могут иметь одинаковые имена!";
                        return false;
                    }
                    if(playerInfos[i].color == playerInfos[j].color)
                    {
                        errorMessage = "Игроки не могут иметь одинаковые цвета!";
                        return false;
                    }
                }
            }

            this.currentPhasePrivate = TurnPhase.movingCell;
            this.fieldPrivate = new Field();
            this.countOfPlayersPlayingPrivate = playerInfos.Length;
            this.deckPrivate = CardDeck.empty;

            FillInfoPlayers(playerInfos);
            this.fieldPrivate.SetPlayers(this.playersPrivate);

            errorMessage = "Нет ошибок.";
            return true;
        }
        /// <summary>
        /// Создать и сразу начать игру.
        /// </summary>
        /// <param name="playerInfos">Информация об игроках.</param>
        /// <returns>isLuckyStart - Началась ли игра.<br/>game - Объект игры.
        /// <br/>errorMessage - Инфо ошибки в случае, если не удалось запустить игру.</returns>
        public static (Boolean isLuckyStart, Game game, String errorMessage) CreateGameWithStart(PlayerInfo[] playerInfos)
        {
            Game game = new Game();
            String errorMessage;
            return (game.Start(playerInfos, out errorMessage), game, errorMessage);
        }

        #endregion Создание игроков и начало игры.

        #endregion Действия.
    }
}
