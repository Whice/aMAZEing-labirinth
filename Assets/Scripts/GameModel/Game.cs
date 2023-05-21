using Assets.Scripts.GameModel.Cards;
using Assets.Scripts.GameModel.Commands;
using Assets.Scripts.GameModel.Logging;
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
        /// Конец игры настал.
        /// </summary>
        public event Action OnGameEnded;
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
                        this.OnGameEnded?.Invoke();
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
            for (Int32 i = 0; i < gamePlayers.Length; i++)
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
                //Для игроков честно считаем, кто первый вышел, кто второй и т.д.
                if (x.winnerNumber > y.winnerNumber)
                {
                    return -1;
                }
                else if (x.winnerNumber < y.winnerNumber)
                {
                    return 1;
                }
                else//Если остались только боты, то они равняются по количетсву карт.
                {//Типа побеждает тот, у кого их меньше
                    if (x.countCardInDeck > y.countCardInDeck)
                    {
                        return 1;
                    }
                    else if (x.countCardInDeck < y.countCardInDeck)
                    {
                        return -1;
                    }
                    else//если количество карт одинакого, то просто воспользуемся именами. Они то точно разные.
                    {
                        return 0;
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

        #region Игровые команды.

        /// <summary>
        /// Пул игровых команд.
        /// </summary>
        private GameCommandPool commandPool = new GameCommandPool();
        /// <summary>
        /// Хранитель игровых команд.
        /// </summary>
        private GameCommandKeeper commandKeeperPrivate;
        /// <summary>
        /// Хранитель игровых команд.
        /// </summary>
        public GameCommandKeeper commandKeeper
        {
            get => this.commandKeeperPrivate;
        }
        /// <summary>
        /// Информация об игре, должна быть заполнена при создании игры.
        /// </summary>
        private GameInfo gameInfoPrivate;
        /// <summary>
        /// Информация об игре, должна быть заполнена при создании игры.
        /// </summary>
        public GameInfo gameInfo
        {
            get => gameInfoPrivate;
        }
        /// <summary>
        /// Создать команду хода аватара.
        /// Она возьмется из пула, инициализируется и положится в хранителя.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void CreateAvatarMovingCommand(Int32 x, Int32 y)
        {
            AvatarMoveCommandSetup setup = new AvatarMoveCommandSetup();
            setup.playerMoveToX = x;
            setup.playerMoveToY = y;
            setup.playerMoveFromX= this.currentPlayer.positionX;
            setup.playerMoveFromY= this.currentPlayer.positionY;
            setup.playerNumber = this.currentPlayer.number;

            GameCommand command = this.commandPool.GetPlayerMoveCommand(setup);
            this.commandKeeperPrivate.Add(command);
        }
        /// <summary>
        /// Создать команду движения ячеек.
        /// Она возьмется из пула, инициализируется и положится в хранителя.
        /// </summary>
        private void CreateCellMovingCommand(in CellMoveCommandSetup setup)
        {
            CellMoveCommand command = this.commandPool.GetCellMoveCommand(setup);
            this.commandKeeperPrivate.Add(command);
        }
        /// <summary>
        /// Выполнить команду.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public Boolean ExecuteCommand(GameCommand command)
        {
            Boolean result = false;

            if (command is CellMoveCommand cellMoveCommand)
            {
                result = SetFreeCellToField(cellMoveCommand);
            }
            else if (command is AvatarMoveCommand avatarMoveCommand)
            {
                result = SetPlayerAvatarToField(avatarMoveCommand);
            }
            else
            {
                GameModelLogger.LogError("Unknow command!");
            }

            return result;
        }
        /// <summary>
        /// Отменить команду.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public Boolean UndoCommand(GameCommand command)
        {
            Boolean result = false;

            GameCommand canceledCommand = this.commandKeeper.Pop();

            if (command is CellMoveCommand cellMoveCommand)
            {
                result = UndoInsertFreeCellToField(cellMoveCommand);
            }
            else if (command is AvatarMoveCommand avatarMoveCommand)
            {
                result = UndoPlayerAvatarMove(avatarMoveCommand);
            }
            else
            {
                GameModelLogger.LogError("Unknow command!");
            }

            if (result == false)
            {
                this.commandKeeper.Add(canceledCommand);
            }

            return result;
        }
        /// <summary>
        /// Отменить сдвиг ячеек укзанный в этой команде.
        /// </summary>
        private Boolean UndoInsertFreeCellToField(CellMoveCommand cellMoveCommand)
        {
            //Подготовить игру к перемещению.
            this.currentPhasePrivate = TurnPhase.movingCell;

            Boolean result = cellMoveCommand.Undo(this);
            this.commandPool.PutInPool(cellMoveCommand);

            //Если было отменено движение ячейкой, то она может двигаться.
            this.currentPhasePrivate = TurnPhase.movingCell;
            CellMoveCommand lastCellMoveCommand = this.commandKeeper.GetLastCellMoveCommand();
            if (lastCellMoveCommand == null)
            {
                this.lastNumberLine = 0;
                this.lastFieldSide = FieldSide.unknow;
            }
            else
            {
                this.lastFieldSide = lastCellMoveCommand.side;
                this.lastNumberLine = lastCellMoveCommand.numberLine;
            }

            return result;
        }
        /// <summary>
        /// Поставить свободную ячейку на поле сдвинув линию. 
        /// </summary>
        /// <param name="numberLine">Номер линии, куда вставить ячейку.</param>
        /// <param name="side">Сторона поля, куда вставить ячейку.</param>
        /// <returns></returns>
        private Boolean SetFreeCellToField(CellMoveCommand cellMoveCommand)
        {
            Boolean result = cellMoveCommand.Execute(this);
            this.commandPool.PutInPool(cellMoveCommand);
            return result;
        }
        /// <summary>
        /// Переместить аватар игрока в указанную позицию в команде, если это возможно.
        /// </summary>
        /// <returns>true, если аватар игрока был перемещен.</returns>
        private Boolean SetPlayerAvatarToField(AvatarMoveCommand playerMoveCommand)
        {
            Boolean result = playerMoveCommand.Execute(this);
            this.commandPool.PutInPool(playerMoveCommand);
            return result;
        }
        /// <summary>
        /// Отменить перемещение аватара игрока в указанную начальную позицию в команде.
        /// </summary>
        /// <returns>true, если аватар игрока был перемещен.</returns>
        private Boolean UndoPlayerAvatarMove(AvatarMoveCommand playerMoveCommand)
        {
            //Подготовить игру к перемещению.
            this.currentPhasePrivate = TurnPhase.movingAvatar;
            this.currentPlayerNumberPrivate = playerMoveCommand.playerNumber;

            Boolean result = playerMoveCommand.Undo(this);
            this.commandPool.PutInPool(playerMoveCommand);

            //Если был отменен ход игрока, то теперь игрок может ходить.
            this.currentPhasePrivate = TurnPhase.movingAvatar;
            //Ходить может тот же игрок, ход для которого был отменен.
            this.currentPlayerNumberPrivate = playerMoveCommand.playerNumber;

            return result;
        }

        #endregion Игровые команды.

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
            if (numberLine == this.lastNumberLine)
            {
                if ((Int32)side + (Int32)this.lastFieldSide == 0)
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
        /// Поставить свободную ячейку на поле сдвинув линию. 
        /// </summary>
        /// <param name="numberLine">Номер линии, куда вставить ячейку.</param>
        /// <param name="side">Сторона поля, куда вставить ячейку.</param>
        /// <returns></returns>
        public Boolean SetFreeCellToField(Int32 numberLine, FieldSide side, Boolean isSaveCommand = true)
        {
            Boolean successfulMove = false;

            if (IsNotUndoPreviousMove(numberLine, side))
            {
                CellMoveCommandSetup setup = new CellMoveCommandSetup();
                setup.turnsClockwiseCountFreeCellBefore = this.freeCell.turnsClockwiseCount;

                switch (side)
                {
                    case FieldSide.bottom:
                        {
                            successfulMove = this.field.MoveLineUp(numberLine);
                            break;
                        }
                    case FieldSide.right:
                        {
                            successfulMove = this.field.MoveLineRight(numberLine);
                            break;
                        }
                    case FieldSide.top:
                        {
                            successfulMove = this.field.MoveLineDown(numberLine);
                            break;
                        }
                    case FieldSide.left:
                        {
                            successfulMove = this.field.MoveLineLeft(numberLine);
                            break;
                        }
                    default:
                        {
                            GameModelLogger.LogError(nameof(FieldSide) + " is unknow!");
                            break;
                        }
                }
                setup.turnsClockwiseCountFreeCellAfter = this.freeCell.turnsClockwiseCount;
                setup.numberLine = numberLine;
                setup.side = side;

                if (successfulMove)
                {
                    //Если игра закончилась, то ход уже невозможен.
                    if (this.isEnd)
                    {
                        return false;
                    }
                    else
                    {
                        if (isSaveCommand)
                        {
                            CreateCellMovingCommand(setup);
                        }

                        SetNextPhase();

                        this.lastNumberLine = numberLine;
                        this.lastFieldSide = side;
                    }
                }
            }


            return successfulMove;
        }
        /// <summary>
        /// Поставить свободную ячейку на поле сдвинув линию. 
        /// <br/>Отмена хода разрешена, т.к. именно для этого и предназначен этот метод.
        /// <br/>Тут не записываются ходы в команды.
        /// </summary>
        /// <param name="numberLine">Номер линии, куда вставить ячейку.</param>
        /// <param name="side">Сторона поля, куда вставить ячейку.</param>
        /// <returns></returns>
        public Boolean SetFreeCellToFieldWithAllowedMovesCancellation(Int32 numberLine, FieldSide side, Boolean isSaveCommand = true)
        {
            Boolean successfulMove = false;

            CellMoveCommandSetup setup = new CellMoveCommandSetup();
            setup.turnsClockwiseCountFreeCellBefore = this.freeCell.turnsClockwiseCount;

            switch (side)
            {
                case FieldSide.bottom:
                    {
                        successfulMove = this.field.MoveLineUp(numberLine);
                        break;
                    }
                case FieldSide.right:
                    {
                        successfulMove = this.field.MoveLineRight(numberLine);
                        break;
                    }
                case FieldSide.top:
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

            setup.turnsClockwiseCountFreeCellAfter = this.freeCell.turnsClockwiseCount;
            setup.numberLine = numberLine;
            setup.side = side;

            if (successfulMove)
            {
                //Еслли игра закончилась, то ход уже невозможен.
                if (this.isEnd)
                {
                    return false;
                }
                else
                {
                    if (isSaveCommand)
                    {
                        CreateCellMovingCommand(setup);
                    }

                    SetNextPhase();

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
        public Boolean SetPlayerAvatarToField(Int32 x, Int32 y, Boolean isSaveCommand = true)
        {
            if (isSaveCommand)
            {
                CreateAvatarMovingCommand(x, y);
            }

            Boolean successfulMove = this.currentPhase == TurnPhase.movingAvatar;
            successfulMove = successfulMove && this.field.IsPossibleMove(this.currentPlayer, this.currentPlayer.positionX, this.currentPlayer.positionY, x, y);

            if (successfulMove)
            {
                this.currentPlayer.SetPosition(x, y);
                TryGetTreasureFromFieldForCurrentPlayer();

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
            else
            {
                GameModelLogger.LogWarning("The player was unable to complete the move.");
            }

            return successfulMove;
        }
        /// <summary>
        /// Пропустить ход для текущего игрока.
        /// </summary>
        public void PlayerMissMove()
        {
            SetPlayerAvatarToField(this.currentPlayer.positionX, this.currentPlayer.positionY);
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
        /// Событие смены фазы игры.
        /// </summary>
        public event Action onPhaseChange;
        /// <summary>
        /// Установить следующую фазу, с переходом хода к следующиму игроку.
        /// </summary>
        private void SetNextPhase()
        {
            if (this.currentPhase == TurnPhase.movingAvatar)
            {
                this.field.ClearCellForMove();
            }

            this.currentPhasePrivate = this.currentPhasePrivate.GetNextPhase();

            if (this.currentPhasePrivate == TurnPhase.movingCell)
            {
                SetNextPlayer();
                this.onNextTurnMoved?.Invoke();
            }
            this.onPhaseChange?.Invoke();
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
                //по умолчанию колода создается со всеми сокровищами.
                CardDeck deck = CardDeck.full;
                deck.Shuffle(this.gameInfoPrivate.cardsShuffleSeed);

                //выяснить, сколько карт каждому игроку
                Int32 countCardsForOnePlayer = deck.count / this.countOfPlayersPlaying;

                CardDeck deckForPlayer;

                for (Int32 i = 0; i < this.countOfPlayersPlaying; i++)
                {
                    //создать колоду для игрока
                    deckForPlayer = new CardDeck();

                    //выдать сперва точку старта, потом все карты для игрока.
                    deckForPlayer.Add(new Card((TreasureAndStartPointsType)(i + 2)));
                    deckForPlayer.Add(deck.Pop(countCardsForOnePlayer));

                    //Выдать координаты
                    Int32 startPointX = this.field.startPointsCoordinate[i].X;
                    Int32 startPointY = this.field.startPointsCoordinate[i].Y;

                    //Создать игровое инфо игрока.
                    this.playersPrivate[i] = new GamePlayer(playerInfos[i], deckForPlayer, startPointX, startPointY, i);
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
            Random random = new Random(this.gameInfoPrivate.fisrtPlayerNumberSeed);
            this.currentPlayerNumberPrivate = random.Next(playerInfos.Length);

            DealCardsToPlayers(playerInfos);
        }
        /// <summary>
        /// Начать игру.
        /// </summary>
        /// <param name="gameInfo">Начальные данные игры.</param>
        public Boolean Start(GameInfo gameInfo)
        {
            this.gameInfoPrivate = gameInfo;
            this.commandKeeperPrivate = new GameCommandKeeper(gameInfo);
            PlayerInfo[] playerInfos = gameInfo.playersInfo;

            if (playerInfos == null)
            {
                GameModelLogger.LogError("Player info cannot contain a null link!");
                return false;
            }
            else if (playerInfos.Length > 4)
            {
                GameModelLogger.LogError("The game can not be more than 4 players!");
                return false;
            }
            else if (playerInfos.Length < 2)
            {
                GameModelLogger.LogError("There can be no more than 2 players in the game!");
                return false;
            }

            for (Int32 i = 0; i < playerInfos.Length; i++)
            {
                for (Int32 j = i + 1; j < playerInfos.Length; j++)
                {
                    if (playerInfos[i].name == playerInfos[j].name)
                    {
                        GameModelLogger.LogError("Players cannot have the same name!");
                        return false;
                    }
                    if (playerInfos[i].color == playerInfos[j].color)
                    {
                        GameModelLogger.LogError("Players cannot have the same color!");
                        return false;
                    }
                }
            }

            this.fieldPrivate = new Field(this.gameInfoPrivate.cellsShuffleSeed);
            this.currentPhasePrivate = TurnPhase.movingCell;
            this.countOfPlayersPlayingPrivate = playerInfos.Length;
            this.deckPrivate = CardDeck.empty;

            FillInfoPlayers(playerInfos);
            this.fieldPrivate.SetPlayers(this.playersPrivate);

            GameModelLogger.LogInfo("No errors. Game started.");
            return true;
        }
        /// <summary>
        /// Создать и сразу начать игру.
        /// </summary>
        /// <param name="playerInfos">Информация об игроках.</param>
        /// <returns>isLuckyStart - Началась ли игра.<br/>game - Объект игры.
        /// <br/>errorMessage - Инфо ошибки в случае, если не удалось запустить игру.</returns>
        public static (Boolean isLuckyStart, Game game) CreateGameWithStart(GameInfo gameInfo)
        {
            Game game = new Game();
            return (game.Start(gameInfo), game);
        }
        /// <summary>
        /// Получить глубокий клон игры.
        /// </summary>
        /// <returns></returns>
        public Game Clone()
        {
            Game clone = new Game();

            clone.deckPrivate = this.deckPrivate.Clone();
            clone.fieldPrivate = this.fieldPrivate.Clone();
            clone.currentPhasePrivate = this.currentPhasePrivate;
            //clone.commandPool = this.commandPool.Clone(); - пул не надо клонировать, он не относится к основным данным игры.
            clone.commandKeeperPrivate = this.commandKeeperPrivate.Clone();
            clone.countOfPlayersPlayingPrivate = this.countOfPlayersPlayingPrivate;
            clone.currentPlayerNumberPrivate = this.currentPlayerNumberPrivate;
            clone.gameInfoPrivate = this.gameInfoPrivate.Clone();
            clone.isEndPrivate = this.isEndPrivate;
            clone.lastFieldSide = this.lastFieldSide;
            clone.lastNumberLine = this.lastNumberLine;
            clone.numberOfNextWinner = this.numberOfNextWinner;

            clone.playersPrivate = new GamePlayer[this.playersPrivate.Length];
            for (Int32 i = 0; i < this.playersPrivate.Length; i++)
            {
                clone.playersPrivate[i] = this.playersPrivate[i].Clone();
            }

            return clone;
        }

        #endregion Создание игроков и начало игры.

        #endregion Действия.

        /// <summary>
        /// Сравнение двух игр.
        /// </summary>
        /// <param name="otherGame">Другая игра.</param>
        /// <returns></returns>
        public bool Equals(Game otherGame)
        {
            if (otherGame == null)
            {
                return false;
            }
            if (otherGame.commandKeeperPrivate != this.commandKeeperPrivate)
            {
                return false;
            }
            //if (otherGame.commandPool != this.commandPool) - пулы не надо сравнивать, они не относятся к основным данным игры.
            if (otherGame.countOfPlayersPlayingPrivate != this.countOfPlayersPlayingPrivate)
            {
                return false;
            }
            if (otherGame.currentPhasePrivate != this.currentPhasePrivate)
            {
                return false;
            }
            if (otherGame.currentPlayerNumberPrivate != this.currentPlayerNumberPrivate)
            {
                return false;
            }
            if (otherGame.deckPrivate != this.deckPrivate)
            {
                return false;
            }
            if (otherGame.fieldPrivate != this.fieldPrivate)
            {
                return false;
            }
            if (otherGame.gameInfoPrivate != this.gameInfoPrivate)
            {
                return false;
            }
            if (otherGame.isEndPrivate != this.isEndPrivate)
            {
                return false;
            }
            if (otherGame.lastFieldSide != this.lastFieldSide)
            {
                return false;
            }
            if (otherGame.lastNumberLine != this.lastNumberLine)
            {
                return false;
            }
            if (otherGame.numberOfNextWinner != this.numberOfNextWinner)
            {
                return false;
            }
            if (otherGame.playersPrivate.Length != this.playersPrivate.Length)
            {
                return false;
            }
            else
            {
                for (int i = 0; i < this.playersPrivate.Length; i++)
                {
                    if (otherGame.playersPrivate[i] != this.playersPrivate[i])
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
