using Assets.Scripts.GameModel.Cards;
using Assets.Scripts.GameModel.Player;
using Assets.Scripts.GameModel.PlayingField;
using Assets.Scripts.GameModel.PlayingField.FieldCells;
using Assets.Scripts.GameModel.TurnPhaseAndExtensions;
using System;

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
        /// Колода карт, сокровища которых были найдены.
        /// </summary>
        private CardDeck deckPrivate = null;
        /// <summary>
        /// Колода карт, сокровища которых были найдены.
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

        #region Во время игры.

        /// <summary>
        /// Поставить свободную ячейку на поле сдвинув линию. 
        /// </summary>
        /// <param name="numberLine">Номер линии, куда вставить ячейку.</param>
        /// <param name="side">Сторона поля, куда вставить ячейку.</param>
        /// <returns></returns>
        public Boolean SetFreeCellToField(Int32 numberLine, FieldSide side)
        {
            Boolean successfulMove = false;

            switch (side)
            {
                case FieldSide.up:
                    {
                        successfulMove = this.field.MoveLineUp(numberLine);
                        break;
                    }
                case FieldSide.right:
                    {
                        successfulMove = this.field.MoveLineRight(numberLine);
                        break;
                    }
                case FieldSide.down:
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
                SetNextPhase();
            }

            return successfulMove;
        }
        /// <summary>
        /// Переместить аватар игрока в указанную позицию, если это возможно.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>true, если аватар игрока был перемещен.</returns>
        public Boolean SetPlayerAvatarToField(Int32 x, Int32 y)
        {
            Boolean successfulMove = this.field.IsPossibleMove(this.currentPlayer.positionX, this.currentPlayer.positionY, x, y);
            if (successfulMove)
            {
                this.currentPlayer.SetPosition(x, y);
                SetNextPhase();
            }

            return successfulMove;
        }
        /// <summary>
        /// Установить следующую фазу, с переходом хода к следующиму игроку.
        /// </summary>
        private void SetNextPhase()
        {
            this.currentPhasePrivate = this.currentPhasePrivate.GetNextPhase();

            if(this.currentPhasePrivate==TurnPhase.movingCell)
            {
                ++this.currentPlayerNumberPrivate;
                if(this.currentPlayerNumberPrivate>=this.playersPrivate.Length)
                {
                    this.currentPlayerNumberPrivate = 0;
                }
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
            if (this.countOfPlayersPlaying > 0)
            {
                CardDeck deck = new CardDeck();
                deck.Shuffle();

                Int32 countCardsForOnePlayer = deck.count/this.countOfPlayersPlaying;
                for(Int32 i = 0; i < this.countOfPlayersPlaying; i++)
                {
                    CardDeck deckForPlayer = new CardDeck(deck.Pop(countCardsForOnePlayer));
                    Int32 startPointX = this.field.startPointsCoordinate[i].X;
                    Int32 startPointY = this.field.startPointsCoordinate[i].Y;
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
