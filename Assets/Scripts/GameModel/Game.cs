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
        public Game(PlayerInfo[] players, Int32 startPlayerNumber)
        {
            Start();
        }
        /// <summary>
        /// Начать игру.
        /// </summary>
        public void Start()
        {
            this.currentPhasePrivate = TurnPhase.movingCell;
            this.fieldPrivate = new Field();
            this.countOfPlayersPlayingPrivate = players.Length;

        }

        #region Данные игры.

        /// <summary>
        /// Колода карт, сокровища которых были найдены.
        /// </summary>
        public CardDeck deck = null;
        /// <summary>
        /// Список игроков.
        /// </summary>
        private GamePlayer[] players = null;
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
        private Int32 currentPlayerNumber = -1;
        /// <summary>
        ///  Игрок, которому принадлежит текущий ход.
        /// </summary>
        public GamePlayer currentPlayer
        {
            get=>this.players[currentPlayerNumber];
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
                ++this.currentPlayerNumber;
                if(this.currentPlayerNumber>=this.players.Length)
                {
                    this.currentPlayerNumber = 0;
                }
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
            this.currentPlayerNumber = random.Next(playerInfos.Length);

            DealCardsToPlayers(playerInfos);
        }
        /// <summary>
        /// Раздать карты игрокам.
        /// </summary>
        /// <param name="playerInfos">Информация об игроках.</param>
        /// <returns></returns>
        private Boolean DealCardsToPlayers(PlayerInfo[] playerInfos)
        {
            this.players = new GamePlayer[playerInfos.Length];
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
                    this.players[i] = new GamePlayer(playerInfos[i], deckForPlayer, startPointX, startPointY);
                }

                return true;
            }
            else
            {
                return false;
            }
        }

            #endregion Действия.
        }
}
