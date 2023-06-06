using Assets.Scripts.GameModel.Player;
using Assets.Scripts.GameModel.PlayingField;
using Assets.Scripts.GameModel.TurnPhaseAndExtensions;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GameView
{
    /// <summary>
    /// Представление игрового поля.
    /// Немного не соответствует повороту модели. Координаты, которые идут
    /// в модели сверху вниз, в представлении слева на право.
    /// </summary>
    public class FieldView : GameWorldViewOriginScript
    {
        #region Данные игрового поля.

        /// <summary>
        /// Модель игрового поля.
        /// </summary>
        private Field playingField
        {
            get => this.gameModel.field;
        }

        /// <summary>
        /// Слот, куда будет помещена ячейка.
        /// </summary>
        [SerializeField]
        private CellSlotFill cellSlotPrefab = null;
        /// <summary>
        /// Размер слота ячейки.
        /// </summary>
        [SerializeField]
        private Single sizeCellSlot = 3f;
        /// <summary>
        /// Размер слота ячейки.
        /// Зависит от размера ячейки.
        /// </summary>
        [SerializeField]
        private Single spacingBetweenCellSlot
        {
            get => this.sizeCellSlot * 0.05f;
        }

        #endregion Данные игрового поля.

        #region Ячейки игрового поля.

        /// <summary>
        /// Слот, куда будут помещены ячейки поля.
        /// </summary>
        [SerializeField]
        private Transform slotForFieldSlots = null;
        /// <summary>
        /// Все слоты поля.
        /// </summary>
        private CellSlotFill[,] slots = new CellSlotFill[Field.FIELD_SIZE, Field.FIELD_SIZE];
        /// <summary>
        /// Множитель расположения, чтобы было небольшое расстояние между слотами.
        /// </summary>
        private Single positionMultiplier
        {
            get => this.sizeCellSlot + this.spacingBetweenCellSlot;
        }

        /// <summary>
        /// Расставить стартовые точки для игроков.
        /// </summary>
        /// <param name="player"></param>
        private void SetPlayerStartPointFromNumber(GamePlayer player)
        {
            Int32 lastNumberPositionInField = Field.FIELD_SIZE - 1;
            CellSlotFill slot;

            switch (player.number)
            {
                case 0:
                    {
                        slot = this.slots[0, 0];
                        break;
                    }
                case 1:
                    {
                        slot = this.slots[0, lastNumberPositionInField];
                        break;
                    }
                case 2:
                    {
                        slot = this.slots[lastNumberPositionInField, lastNumberPositionInField];
                        break;
                    }
                case 3:
                    {
                        slot = this.slots[lastNumberPositionInField, 0];
                        break;
                    }
                default:
                    {
                        LogError("Player number may be in range 0..3!");
                        return;
                    }
            }

            slot.SetPlayerStartPoint
                        (
                        new Color
                        (
                            player.color.R,
                            player.color.G,
                            player.color.B
                        )
                        );
        }
        /// <summary>
        /// Заполнить игровое поле ячейками.
        /// </summary>
        private void FillFieldWithCell()
        {
            //Заполнение ячейками.
            CellSlotFill.positionMultiplier = this.positionMultiplier;
            for (Int32 i = 0; i < Field.FIELD_SIZE; i++)
                for (Int32 j = 0; j < Field.FIELD_SIZE; j++)
                {
                    this.slots[i, j] = InstantiateWithInject(this.cellSlotPrefab);
                    this.slots[i, j].SetCellFromModelCell(this.playingField[i, j]);
                    this.slots[i, j].SetSlotPosition(i, j);
                    this.slots[i, j].transform.parent = this.slotForFieldSlots;
                    this.slots[i, j].OnCellSlotClicked += MoveAvatarInModel;
                }

            //Установить стартовые точки для игроков.
            foreach (GamePlayer player in this.gameModel.players)
            {
                SetPlayerStartPointFromNumber(player);
            }

            //Установить игроков на поле.
            Int32 playerPositionX;
            Int32 playerPositionY;
            for (Int32 i = 0; i < this.gameModel.players.Length; ++i)
            {
                playerPositionX = this.gameModel.players[i].positionX;
                playerPositionY = this.gameModel.players[i].positionY;
                this.slots[playerPositionX, playerPositionY].SetPlayerAvatarSlot(i);
            }
        }

        #region Свободная ячейка.

        /// <summary>
        /// Слот для запоминания свободной ячейки.
        /// </summary>
        private CellSlotFill freeCellSlot;
        /// <summary>
        /// Слот для слота свободной ячейки.
        /// </summary>
        [SerializeField]
        private Transform slotForFreeCellSlot = null;
        /// <summary>
        /// Задать местоположение для слота свободной ячейки через положение слота для нее.
        /// </summary>
        /// <param name="parent"></param>
        private void SetPlaceForFreeCellSlot(Transform parent, ArrowForFreeCellSlotFill slot)
        {
            if (!this.isShifting)
            {
                //Если слот остался прежним, то должен произойти сдвиг.
                if (slot == this.freeCellSlot.arrowForFreeCellSlotFill)
                {
                    BeginPerformShift(slot);
                }
                else
                {
                    this.freeCellSlot.transform.parent = parent;
                    this.freeCellSlot.transform.localPosition = new Vector3(0, -ArrowForFreeCellSlotFill.HEIGHT_FOR_ARROW_SLOT, 0);

                    ArrowForFreeCellSlotFill oldSlot = this.freeCellSlot.arrowForFreeCellSlotFill as ArrowForFreeCellSlotFill;
                    slot.freeCellSlot = this.freeCellSlot;
                    this.freeCellSlot.arrowForFreeCellSlotFill = slot;
                    if (oldSlot != null)
                    {
                        oldSlot.freeCellSlot = null;
                    }
                }
            }
        }
        /// <summary>
        /// Задать указанную ячейку как свободную и посметить в слот.
        /// </summary>
        /// <param name="slot"></param>
        private void SetFreeCell(CellSlotFill slot)
        {
            this.freeCellSlot = slot;
            this.freeCellSlot.transform.parent = this.slotForFreeCellSlot;
            this.freeCellSlot.transform.localPosition = Vector3.zero;
            this.freeCellSlot.transform.rotation = Quaternion.identity;
        }
        /// <summary>
        /// Заполнить слот для свободной ячейкой.
        /// </summary>
        private void FillFreeCellSlot()
        {
            CellSlotFill freeCellSlot = InstantiateWithInject(this.cellSlotPrefab);
            freeCellSlot.OnCellSlotClicked += MoveAvatarInModel;
            freeCellSlot.SetCellFromModelCell(this.playingField.freeFieldCell);
            SetFreeCell(freeCellSlot);
        }
        /// <summary>
        /// Очистить слот свободной ячейки. Переменная после этого равна null.
        /// </summary>
        private void ClearFreeCellSlot()
        {
            this.freeCellSlot = null;
        }

        #endregion Свободная ячейка.

        #endregion Ячейки игрового поля.

        #region Игровые аватары.

        /// <summary>
        ///  Игрок, которому принадлежит текущий ход.
        /// </summary>
        private GamePlayer gamePlayer
        {
            get => this.gameModel.currentPlayer;
        }
        /// <summary>
        ///  Номер игрока, которому принадлежит текущий ход.
        /// </summary>
        private Int32 currentPlayerNumber
        {
            get => this.gameModel.currentPlayerNumber;
        }
        /// <summary>
        /// Передвинуть аватар игрока в представлении.
        /// </summary>
        /// <param name="fromX"></param>
        /// <param name="fromY"></param>
        /// <param name="toX"></param>
        /// <param name="toY"></param>
        private void MoveAvatarView(Int32 fromX, Int32 fromY, Int32 toX, Int32 toY, Int32 playerNumber)
        {
            //Передвижение выполняется в двух случаях:
            // 1) Аватара надо переместить на другую сторону поля, 
            //      т.к. его вынесло за пределы поля.
            // 2) Если фаза хода аватаром и его переместили. 
            if (this.gameModel.players[playerNumber].isMoveToOppositeSide)
            {
                this.gameModel.players[playerNumber].isMoveToOppositeSide = false;
                this.freeCellSlot.SwapAvatarSlot(this.slots[fromX, fromY], playerNumber);
            }
            else if (this.gameModel.currentPhase == TurnPhase.movingAvatar)
            {
                this.slots[toX, toY].SwapAvatarSlot(this.slots[fromX, fromY], playerNumber);
            }
        }
        /// <summary>
        /// Передвинуть аватар игрока в модели.
        /// </summary>
        /// <param name="newSlotForAvatar">Слот, куда происходит перемещение.</param>
        private void MoveAvatarInModel(CellSlotFill newSlotForAvatar)
        {
            if (this.gameModel.currentPhase == TurnPhase.movingAvatar)
            {
                this.gameModel.SetPlayerAvatarToField(newSlotForAvatar.positionInField.x, newSlotForAvatar.positionInField.y);
            }
        }

        #endregion Игровые аватары.

        #region Стрелочки игрового поля.

        /// <summary>
        /// Скрытая стрелочка, которая может "отменить" ход.
        /// </summary>
        private ArrowForFreeCellSlotFill hiddenArrow;
        /// <summary>
        /// 4 линии для стрелочек-мест для свободных ячеек.
        /// <br/>1е несколько - верхняя сторона.
        /// <br/>2е несколько - правая сторона.
        /// <br/>3е несколько - нижняя сторона.
        /// <br/>4е несколько - левая сторона.
        /// </summary>
        private ArrowForFreeCellSlotFill[] arrowForFreeCellSlot = new ArrowForFreeCellSlotFill[4 * Field.FIELD_SIZE / 2];
        /// <summary>
        /// Слот, куда будет помещеная свободная ячейка, а пока ее нет, то там будет стрелка.
        /// </summary>
        [SerializeField]
        private ArrowForFreeCellSlotFill arrowForFreeCellSlotPrefab = null;
        /// <summary>
        /// Слот, куда будут стрелочки-слоты для свободной ячейки поля.
        /// </summary>
        [SerializeField]
        private Transform arrowsForFreeCellSlots = null;

        /// <summary>
        /// Добавить слоты для стрелочек - мест, куда будет помещена свободная ячейка.
        /// </summary>
        private void AddArrowSlotsForFreeCell()
        {
            //Добавить только одну стрелочку для 
            void SetParametrsForArrowSlotForFreeCell(ref ArrowForFreeCellSlotFill slot, Int32 turnClockwiseCount, FieldSide side)
            {
                slot = InstantiateWithInject(this.arrowForFreeCellSlotPrefab);
                slot.SetArrowForFreeCell();
                slot.transform.parent = this.arrowsForFreeCellSlots;
                slot.OnArrowClicked += SetPlaceForFreeCellSlot;
                slot.TurnClockwise(turnClockwiseCount);
                slot.SetFieldSide(side);
            }

            Single positionMultiplier = this.sizeCellSlot + this.spacingBetweenCellSlot;
            Int32 currentCellNumber = 0;
            const Int32 COUNT_SLOTS_IN_LINE = Field.FIELD_SIZE / 2;
            Vector3 leftLineStart = new Vector3(0, ArrowForFreeCellSlotFill.HEIGHT_FOR_ARROW_SLOT, 0);
            for (Int32 i = 0; i < COUNT_SLOTS_IN_LINE; i++)
            {
                //левая линия
                currentCellNumber = i + COUNT_SLOTS_IN_LINE * 3;
                SetParametrsForArrowSlotForFreeCell(ref this.arrowForFreeCellSlot[currentCellNumber], 0, FieldSide.left);
                this.arrowForFreeCellSlot[currentCellNumber].localPosition = new Vector3
                        (
                        leftLineStart.x - positionMultiplier,
                        leftLineStart.y,
                        leftLineStart.z + (i * 2 + 1) * positionMultiplier
                        );
                this.arrowForFreeCellSlot[currentCellNumber].SetPositionInField(-1, i * 2 + 1);
                ArrowForFreeCellSlotFill leftLineArrow = this.arrowForFreeCellSlot[currentCellNumber];

                //нижяя линия
                currentCellNumber = i + COUNT_SLOTS_IN_LINE * 2;
                SetParametrsForArrowSlotForFreeCell(ref this.arrowForFreeCellSlot[currentCellNumber], 3, FieldSide.bottom);
                this.arrowForFreeCellSlot[currentCellNumber].localPosition = new Vector3
                        (
                        leftLineStart.x + (i * 2 + 1) * positionMultiplier,
                        leftLineStart.y,
                        leftLineStart.z - positionMultiplier
                        );
                this.arrowForFreeCellSlot[currentCellNumber].SetPositionInField(i * 2 + 1, -1);
                ArrowForFreeCellSlotFill bottomLineArrow = this.arrowForFreeCellSlot[currentCellNumber];

                //правая линия
                currentCellNumber = i + COUNT_SLOTS_IN_LINE;
                SetParametrsForArrowSlotForFreeCell(ref this.arrowForFreeCellSlot[currentCellNumber], 2, FieldSide.right);
                this.arrowForFreeCellSlot[currentCellNumber].localPosition = new Vector3
                        (
                        leftLineStart.x + positionMultiplier * Field.FIELD_SIZE,
                        leftLineStart.y,
                        leftLineStart.z + (i * 2 + 1) * positionMultiplier
                        );
                this.arrowForFreeCellSlot[currentCellNumber].SetPositionInField(Field.FIELD_SIZE, i * 2 + 1);
                ArrowForFreeCellSlotFill rightLineArrow = this.arrowForFreeCellSlot[currentCellNumber];

                //верхняя линия
                currentCellNumber = i;
                SetParametrsForArrowSlotForFreeCell(ref this.arrowForFreeCellSlot[currentCellNumber], 1, FieldSide.top);
                this.arrowForFreeCellSlot[currentCellNumber].localPosition = new Vector3
                        (
                        leftLineStart.x + (i * 2 + 1) * positionMultiplier,
                        leftLineStart.y,
                        leftLineStart.z + positionMultiplier * Field.FIELD_SIZE
                        );
                this.arrowForFreeCellSlot[currentCellNumber].SetPositionInField(i * 2 + 1, Field.FIELD_SIZE);
                ArrowForFreeCellSlotFill topLineArrow = this.arrowForFreeCellSlot[currentCellNumber];

                leftLineArrow.SetArrowOnOpositeSide(rightLineArrow);
                rightLineArrow.SetArrowOnOpositeSide(leftLineArrow);
                topLineArrow.SetArrowOnOpositeSide(bottomLineArrow);
                bottomLineArrow.SetArrowOnOpositeSide(topLineArrow);
            }
        }

        #endregion Стрелочки игрового поля.

        #region Сдвиг линии при толчке свободной ячейкой.

        /// <summary>
        /// Началось движение линии ячеек.
        /// </summary>
        public event Action OnBeginLineMove;
        /// <summary>
        /// Закончилось движение линии ячеек.
        /// </summary>
        public event Action OnEndLineMove;
        /// <summary>
        /// Сторона, к которой производится сдвиг.
        /// </summary>
        private FieldSide sideWhereToMove;
        /// <summary>
        /// Номер линии, по которой выполняется сдвиг.
        /// </summary>
        private Int32 numberLineForShift;
        /// <summary>
        /// Происходит сдвиг.
        /// </summary>
        private Boolean isShiftingField = false;
        /// <summary>
        /// Происходит сдвиг.
        /// </summary>
        private Boolean isShifting
        {
            get
            {
                return this.isShiftingField;
            }
            set
            {
                this.isShiftingField = value;
                if (value)
                {
                    this.OnBeginLineMove?.Invoke();
                }
                else
                {
                    this.OnEndLineMove?.Invoke();
                }
            }
        }
        /// <summary>
        /// Максимальное время передвижения ячеек.
        /// </summary>
        private const Single MAX_TIME_SHIFT = 1f;
        /// <summary>
        /// Общее, прошедшее время для перемещения.
        /// </summary>
        private Single timeShift;
        /// <summary>
        /// Слоты для передвижения.
        /// </summary>
        private CellSlotFill[] slotsForShift = new CellSlotFill[Field.FIELD_SIZE + 1];
        /// <summary>
        /// Начать анимацию сдвига.
        /// </summary>
        private void BeginAnimationOfPerformShift()
        {
            Int32 numberLine = this.numberLineForShift;
            Int32 positionLastCell = 0;
            Int32 slotsForShiftCounter = 0;
            CellSlotFill lastCell = null;

            switch (this.sideWhereToMove)
            {
                case FieldSide.right:
                    {
                        lastCell = this.slots[0, numberLine];
                        positionLastCell = lastCell.positionInField.x;
                        for (Int32 i = Field.FIELD_SIZE - 1; i > -1; i--)
                        {
                            this.slots[i, numberLine].SetTargetSlotPosition
                                   (
                                   this.slots[i, numberLine].positionInField.x + 1,
                                   this.slots[i, numberLine].positionInField.y,
                                   MAX_TIME_SHIFT
                                   );

                            this.slotsForShift[slotsForShiftCounter] = this.slots[i, numberLine];
                            slotsForShiftCounter++;
                        }
                        break;
                    }
                case FieldSide.left:
                    {
                        lastCell = this.slots[Field.FIELD_SIZE - 1, numberLine];
                        positionLastCell = lastCell.positionInField.x;
                        for (Int32 i = 0; i < Field.FIELD_SIZE; i++)
                        {
                            this.slots[i, numberLine].SetTargetSlotPosition
                                   (
                                   this.slots[i, numberLine].positionInField.x - 1,
                                   this.slots[i, numberLine].positionInField.y,
                                   MAX_TIME_SHIFT
                                   );

                            this.slotsForShift[slotsForShiftCounter] = this.slots[i, numberLine];
                            slotsForShiftCounter++;
                        }
                        break;
                    }
                case FieldSide.top:
                    {
                        lastCell = this.slots[numberLine, 0];
                        positionLastCell = lastCell.positionInField.y;
                        for (Int32 i = Field.FIELD_SIZE - 1; i > -1; i--)
                        {
                            this.slots[numberLine, i].SetTargetSlotPosition
                                   (
                                   this.slots[numberLine, i].positionInField.x,
                                   this.slots[numberLine, i].positionInField.y + 1,
                                   MAX_TIME_SHIFT
                                   );

                            this.slotsForShift[slotsForShiftCounter] = this.slots[numberLine, i];
                            slotsForShiftCounter++;
                        }
                        break;
                    }
                case FieldSide.bottom:
                    {
                        lastCell = this.slots[numberLine, Field.FIELD_SIZE - 1];
                        positionLastCell = lastCell.positionInField.y;
                        for (Int32 i = 0; i < Field.FIELD_SIZE; i++)
                        {
                            this.slots[numberLine, i].SetTargetSlotPosition
                                   (
                                   this.slots[numberLine, i].positionInField.x,
                                   this.slots[numberLine, i].positionInField.y - 1,
                                   MAX_TIME_SHIFT
                                   );

                            this.slotsForShift[slotsForShiftCounter] = this.slots[numberLine, i];
                            slotsForShiftCounter++;
                        }
                        break;
                    }
            }
            CellSlotFill oldFreeSlot = this.freeCellSlot;
            ClearFreeCellSlot();
            oldFreeSlot.transform.parent = this.slotForFieldSlots;
            ArrowForFreeCellSlotFill oldArrow = oldFreeSlot.arrowForFreeCellSlotFill as ArrowForFreeCellSlotFill;
            oldArrow.freeCellSlot = null;
            oldFreeSlot.arrowForFreeCellSlotFill = null;

            if (this.sideWhereToMove == FieldSide.left || this.sideWhereToMove == FieldSide.right)
            {
                oldFreeSlot.SetTargetSlotPosition(positionLastCell, numberLine, MAX_TIME_SHIFT);
            }
            else
            {
                oldFreeSlot.SetTargetSlotPosition(numberLine, positionLastCell, MAX_TIME_SHIFT);
            }
            this.slotsForShift[Field.FIELD_SIZE] = oldFreeSlot;
        }

        /// <summary>
        /// Выполнить Передвижение ячеек.
        /// </summary>
        private void PerformShift()
        {
            foreach (CellSlotFill slot in this.slotsForShift)
            {
                slot.MoveToTargetLocalPosition();
            }
        }
        /// <summary>
        /// ВЫполнить окончание передвижение ячеек.
        /// </summary>
        private void EndPerformShift()
        {
            if (this.sideWhereToMove != FieldSide.unknow)
            {

                Int32 numberLine = this.numberLineForShift;
                SetFreeCell(this.slotsForShift[0]);
                Int32 slotsForShiftCounter = 1;

                Action<Int32, Int32> setNewPosition = (Int32 i, Int32 j) =>
                {
                    this.slots[i, j] = this.slotsForShift[slotsForShiftCounter];
                    slotsForShiftCounter++;
                    this.slots[i, j].SetSlotPosition(i, j);
                };

                switch (this.sideWhereToMove)
                {
                    case FieldSide.right:
                        {

                            for (Int32 i = Field.FIELD_SIZE - 1; i > -1; i--)
                            {
                                setNewPosition(i, numberLine);
                            }

                            break;
                        }
                    case FieldSide.left:
                        {
                            for (Int32 i = 0; i < Field.FIELD_SIZE; i++)
                            {
                                setNewPosition(i, numberLine);
                            }

                            break;
                        }
                    case FieldSide.top:
                        {
                            for (Int32 i = Field.FIELD_SIZE - 1; i > -1; i--)
                            {
                                setNewPosition(numberLine, i);
                            }

                            break;
                        }
                    case FieldSide.bottom:
                        {
                            for (Int32 i = 0; i < Field.FIELD_SIZE; i++)
                            {
                                setNewPosition(numberLine, i);
                            }

                            break;
                        }
                }

                ToCheckIfViewOfFieldCorrespondsToItModel();
            }
            else
            {
                LogError("In " + nameof(EndPerformShift) + " " + nameof(FieldSide) + " is " + nameof(FieldSide.unknow));
            }

            this.isShifting = false;
            ShowCellsWhereCanMoveIfNeed();
        }

        /// <summary>
        /// Проверить соответсвие пердсталения поля его модели.
        /// Если типы ячеек у поля и его медоле не совпадают, то сигнализировать.
        /// </summary>
        /// <returns></returns>
        private void ToCheckIfViewOfFieldCorrespondsToItModel()
        {
            Boolean isCorresponds = true;
            if (this.freeCellSlot.cellType != this.playingField.freeFieldCell.CellType)
            {
                isCorresponds = false;
            }

            for (Int32 i = 0; i < Field.FIELD_SIZE; i++)
            {
                for (Int32 j = 0; j < Field.FIELD_SIZE; j++)
                {
                    if (!this.slots[i, j].IsEqualWithModelCell(this.playingField[i, j]))
                    {
                        LogInfo(this.slots[i, j].ToString() + "\n\n" + this.playingField[i, j].ToString()+"\n");
                        isCorresponds = false;
                    }
                }
            }

            if (!isCorresponds)
            {
                LogError("Field's view is inconsistent with field's model!");
            }
        }
        /// <summary>
        /// Выполнить подготовку к сдвигу ячеек.
        /// </summary>
        /// <param name="slotWithFreeSlot">Слот, куда была помещена свободная ячейка при начале движения.</param>
        private void BeginPerformShift(ArrowForFreeCellSlotFill slotWithFreeSlot)
        {
            if (!this.isShifting && this.gameModel.currentPhase == TurnPhase.movingCell)
            {
                FieldSide side = slotWithFreeSlot.side;
                if (side != FieldSide.unknow)
                {
                    //Скрыть стрелочку, которая может "отменить" ход и показать предыдущую скрытую.
                    if (this.hiddenArrow != slotWithFreeSlot.arrowOnOpositeSide)
                    {
                        if (this.hiddenArrow != null)
                        {
                            this.hiddenArrow.ShowArrow();
                        }
                        this.hiddenArrow = slotWithFreeSlot.arrowOnOpositeSide;
                        this.hiddenArrow.HideArrow();
                    }

                    this.isShifting = true;
                    this.timeShift = 0;

                    switch (side)
                    {
                        case FieldSide.right:
                            {
                                this.numberLineForShift = slotWithFreeSlot.positionInField.y;
                                this.sideWhereToMove = FieldSide.left;
                                break;
                            }
                        case FieldSide.left:
                            {
                                this.numberLineForShift = slotWithFreeSlot.positionInField.y;
                                this.sideWhereToMove = FieldSide.right;
                                break;
                            }
                        case FieldSide.top:
                            {
                                this.numberLineForShift = slotWithFreeSlot.positionInField.x;
                                this.sideWhereToMove = FieldSide.bottom;
                                break;
                            }
                        case FieldSide.bottom:
                            {
                                this.numberLineForShift = slotWithFreeSlot.positionInField.x;
                                this.sideWhereToMove = FieldSide.top;
                                break;
                            }
                    }
                    this.gameModel.SetFreeCellToField(this.numberLineForShift, this.sideWhereToMove);
                    BeginAnimationOfPerformShift();
                }
                else
                {
                    LogError("Side is unknow!");
                }
            }
        }

        #endregion Сдвиг линии при толчке свободной ячейкой.

        #region Показать ячейки, куда можно ходить.

        /// <summary>
        /// Надо показать ячейки, куда можно ходить.
        /// <br/>Переменая нужна для того, чтобы можно было выставить режим
        /// показывать/непоказывать, в зависимости от предпочтений игрока.
        /// </summary>
        [SerializeField]
        private Boolean isNeedShowCellsWhereCanMove = true;
        /// <summary>
        /// Сбросить высоту всем ячейкам в игре на изначальную.
        /// </summary>
        private void ResetHeightAllCells()
        {
            foreach (CellSlotFill cellSlot in this.slots)
            {
                cellSlot.ResetHeight();
            }
            this.freeCellSlot.ResetHeight();
        }
        /// <summary>
        /// Показать ячейки, куда можно ходить, если надо.
        /// </summary>
        private void ShowCellsWhereCanMoveIfNeed()
        {
            if (this.isNeedShowCellsWhereCanMove)
            {
                if (this.gameModel.currentPhase == TurnPhase.movingAvatar)
                {
                    ResetHeightAllCells();

                    HashSet<System.Drawing.Point> pointsCellsForMove = this.gameModel.field.GetPointsForMove(this.gameModel.currentPlayer);
                    foreach (System.Drawing.Point point in pointsCellsForMove)
                    {
                        this.slots[point.X, point.Y].height = 0.5f;
                    }
                }
            }
        }

        #endregion Показать ячейки, куда можно ходить.

        protected override void Subscribe()
        {

            foreach (GamePlayer player in this.gameModel.players)
            {
                player.onAvatarMoved += MoveAvatarView;
            }

            this.gameModel.onPhaseChange += ResetHeightAllCells;
            base.Subscribe();
        }
        /// <summary>
        /// Воссоздать всю визуальную часть поля на основе глобально известной модели игры.
        /// </summary>
        public new void Initialize()
        {
            FillFreeCellSlot();
            FillFieldWithCell();
            ToCheckIfViewOfFieldCorrespondsToItModel();

            AddArrowSlotsForFreeCell();
            ShowCellsWhereCanMoveIfNeed();
            Subscribe();
        }
        private void Update()
        {
            if (this.isShifting)
            {
                this.timeShift += Time.deltaTime;

                PerformShift();

                if (this.timeShift > MAX_TIME_SHIFT)
                {
                    EndPerformShift();
                }
            }
        }

        protected override void Unsubscribe()
        {
            base.Unsubscribe();
            foreach (CellSlotFill slot in this.slots)
            {
                if (slot != null)
                    slot.OnCellSlotClicked -= MoveAvatarInModel;
            }

            foreach (GamePlayer player in this.gameModel.players)
            {
                if (player != null)
                    player.onAvatarMoved -= MoveAvatarView;
            }

            if (this.gameModel != null)
                this.gameModel.onPhaseChange -= ResetHeightAllCells;
        }
    }
}
