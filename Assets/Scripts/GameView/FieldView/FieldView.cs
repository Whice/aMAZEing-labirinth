using Assets.Scripts.GameModel.PlayingField;
using Assets.Scripts.GameModel.PlayingField.FieldCells;
using System;
using UnityEngine;

namespace Assets.Scripts.GameView
{
    /// <summary>
    /// Представление игрового поля.
    /// </summary>
    public class FieldView: GameViewOriginScript
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
        private Single sizeCellSlot = 1f;
        /// <summary>
        /// Размер слота ячейки.
        /// Зависит от размера ячейки.
        /// </summary>
        [SerializeField]
        private Single spacingBetweenCellSlot
        {
            get => this.sizeCellSlot * 0.2f;
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
        /// Заполнить игровое поле ячейками.
        /// </summary>
        private void FillFieldWithCell()
        {
            //Заполнение ячейками.
            Single positionMultiplier = this.positionMultiplier;
            for (Int32 i = 0; i < Field.FIELD_SIZE; i++)
                for (Int32 j = 0; j < Field.FIELD_SIZE; j++)
                {
                    this.slots[i, j] = Instantiate(this.cellSlotPrefab);
                    this.slots[i, j].SetCellType(this.playingField[i,j].CellType);
                    this.slots[i, j].TurnClockwise(this.playingField[i, j].countTurnClockwiseFromDefaultRotateToCurrentRotate);
                    this.slots[i, j].SetSlotPosition(i, j, positionMultiplier);
                    this.slots[i, j].transform.parent = this.slotForFieldSlots;
                }


        }

        #endregion Ячейки игрового поля.

        private void Update()
        {
            if (this.isShifting)
            {
                this.timeShift += Time.deltaTime;

                PerformShift();

                if (this.timeShift > MAX_TIME_SHIFT)
                {
                    EndPerformShift();
                    this.isShifting = false;
                }
            }
        }

        #region Стрелочки игрового поля.

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
                slot = Instantiate(this.arrowForFreeCellSlotPrefab);
                slot.SetArrowForFreeCell();
                slot.transform.parent = this.arrowsForFreeCellSlots;
                slot.OnArrowClicked += SetPlaceForFreeCellSlot;
                slot.TurnClockwise(turnClockwiseCount);
                slot.SetFieldSide(side);
            }

            Single positionMultiplier = this.sizeCellSlot + this.spacingBetweenCellSlot;
            Int32 currentCellNumber = 0;
            const Int32 COUNT_SLOTS_IN_LINE = Field.FIELD_SIZE / 2;
            Vector3 leftLineStart = new Vector3(0, 0, 0);
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
        /// Направления для сдвига.
        /// </summary>
        private enum DirectionShift
        {
            /// <summary>
            /// Неизвестно.
            /// </summary>
            unknow=0,
            /// <summary>
            /// Вправо.
            /// </summary>
            toRight=1,
            /// <summary>
            /// Вниз.
            /// </summary>
            toBottom=2,
            /// <summary>
            /// Влево.
            /// </summary>
            toLeft=3,
            /// <summary>
            /// Вверх.
            /// </summary>
            toTop=4,
        }
        /// <summary>
        /// Напрвление, куда производится сдвиг.
        /// </summary>
        private DirectionShift directionShift;
        /// <summary>
        /// Номер линии, по которой выполняется сдвиг.
        /// </summary>
        private Int32 numberLineForShift;
        /// <summary>
        /// Происходить сдвиг.
        /// </summary>
        private Boolean isShifting = false;
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
            Single positionMultiplier = this.positionMultiplier;
            Int32 slotsForShiftCounter = 0;

            switch (this.directionShift)
            {
                case DirectionShift.toRight:
                    {
                        positionLastCell = this.slots[0, numberLine].positionInField.x;
                        for (Int32 i = Field.FIELD_SIZE - 1; i > -1; i--)
                        {
                            this.slots[i, numberLine].SetTargetSlotPosition
                                   (
                                   this.slots[i, numberLine].positionInField.x + 1,
                                   this.slots[i, numberLine].positionInField.y,
                                   positionMultiplier,
                                   MAX_TIME_SHIFT
                                   );

                            this.slotsForShift[slotsForShiftCounter] = this.slots[i, numberLine];
                            slotsForShiftCounter++;
                        }
                        break;
                    }
                case DirectionShift.toLeft:
                    {
                        positionLastCell = this.slots[Field.FIELD_SIZE - 1, numberLine].positionInField.x;
                        for (Int32 i = 0; i < Field.FIELD_SIZE; i++)
                        {
                            this.slots[i, numberLine].SetTargetSlotPosition
                                   (
                                   this.slots[i, numberLine].positionInField.x - 1,
                                   this.slots[i, numberLine].positionInField.y,
                                   positionMultiplier,
                                   MAX_TIME_SHIFT
                                   );

                            this.slotsForShift[slotsForShiftCounter] = this.slots[i, numberLine];
                            slotsForShiftCounter++;
                        }
                        break;
                    }
                case DirectionShift.toTop:
                    {
                        positionLastCell = this.slots[numberLine, 0].positionInField.y;
                        for (Int32 i = Field.FIELD_SIZE - 1; i > -1; i--)
                        {
                            this.slots[ numberLine, i].SetTargetSlotPosition
                                   (
                                   this.slots[numberLine, i].positionInField.x,
                                   this.slots[numberLine, i].positionInField.y+1,
                                   positionMultiplier,
                                   MAX_TIME_SHIFT
                                   );

                            this.slotsForShift[slotsForShiftCounter] = this.slots[ numberLine, i];
                            slotsForShiftCounter++;
                        }
                        break;
                    }
                case DirectionShift.toBottom:
                    {
                        positionLastCell = this.slots[ numberLine, Field.FIELD_SIZE - 1].positionInField.y;
                        for (Int32 i = 0; i < Field.FIELD_SIZE; i++)
                        {
                            this.slots[numberLine, i].SetTargetSlotPosition
                                   (
                                   this.slots[ numberLine, i].positionInField.x ,
                                   this.slots[ numberLine, i].positionInField.y-1,
                                   positionMultiplier,
                                   MAX_TIME_SHIFT
                                   );

                            this.slotsForShift[slotsForShiftCounter] = this.slots[ numberLine, i];
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

            if (this.directionShift == DirectionShift.toLeft || this.directionShift == DirectionShift.toRight)
            {
                oldFreeSlot.SetTargetSlotPosition(positionLastCell, numberLine, positionMultiplier, MAX_TIME_SHIFT);
            }
            else
            {
                oldFreeSlot.SetTargetSlotPosition(numberLine, positionLastCell, positionMultiplier, MAX_TIME_SHIFT);
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
            Int32 numberLine = this.numberLineForShift;
            SetFreeCell(this.slotsForShift[0]);
            Int32 slotsForShiftCounter = 1;

            switch (this.directionShift)
            {
                case DirectionShift.toRight:
                    {

                        for (Int32 i = Field.FIELD_SIZE - 1; i >-1; i--)
                        {
                            this.slots[i, numberLine] = this.slotsForShift[slotsForShiftCounter];
                            slotsForShiftCounter++;
                            this.slots[i, numberLine].SetSlotPosition(i, numberLine, this.positionMultiplier);
                        }

                        break;
                    }
                case DirectionShift.toLeft:
                    {
                        for (Int32 i = 0; i < Field.FIELD_SIZE; i++)
                        {
                            this.slots[i, numberLine] = this.slotsForShift[slotsForShiftCounter];
                            slotsForShiftCounter++;
                            this.slots[i, numberLine].SetSlotPosition(i, numberLine, this.positionMultiplier);
                        }

                        break;
                    }
                case DirectionShift.toTop:
                    {

                        for (Int32 i = Field.FIELD_SIZE - 1; i >-1; i--)
                        {
                            this.slots[numberLine, i] = this.slotsForShift[slotsForShiftCounter];
                            slotsForShiftCounter++;
                            this.slots[ numberLine, i].SetSlotPosition(numberLine, i, this.positionMultiplier);
                        }

                        break;
                    }
                case DirectionShift.toBottom:
                    {
                        for (Int32 i = 0; i < Field.FIELD_SIZE; i++)
                        {
                            this.slots[numberLine, i] = this.slotsForShift[slotsForShiftCounter];
                            slotsForShiftCounter++;
                            this.slots[numberLine, i].SetSlotPosition(numberLine, i, this.positionMultiplier);
                        }

                        break;
                    }
            }
            
        }

        /// <summary>
        /// Выполнить подготовку к сдвигу ячеек.
        /// </summary>
        /// <param name="slotWithFreeSlot">Слот, куда была помещена свободная ячейка при начале движения.</param>
        private void BeginPerformShift(ArrowForFreeCellSlotFill slotWithFreeSlot)
        {
            if (slotWithFreeSlot.side != FieldSide.unknow)
            {
                this.isShifting = true;
                this.timeShift = 0;

                switch (slotWithFreeSlot.side)
                {
                    case FieldSide.right:
                        {
                            this.numberLineForShift = slotWithFreeSlot.positionInField.y;
                            this.directionShift = DirectionShift.toLeft;
                            BeginAnimationOfPerformShift();
                            break;
                        }
                    case FieldSide.left:
                        {
                            this.numberLineForShift = slotWithFreeSlot.positionInField.y;
                            this.directionShift = DirectionShift.toRight;
                            BeginAnimationOfPerformShift();
                            break;
                        }
                    case FieldSide.top:
                        {
                            this.numberLineForShift = slotWithFreeSlot.positionInField.x;
                            this.directionShift = DirectionShift.toBottom;
                            BeginAnimationOfPerformShift();
                            break;
                        }
                    case FieldSide.bottom:
                        {
                            this.numberLineForShift = slotWithFreeSlot.positionInField.x;
                            this.directionShift = DirectionShift.toTop;
                            BeginAnimationOfPerformShift();
                            break;
                        }
                }
            }
            else
            {
                LogError("Side is unknow!");
            }
        }

        #endregion Сдвиг линии при толчке свободной ячейкой.

        #region Свободная ячейка.

        /// <summary>
        /// Слот для запоминания свободной ячейки.
        /// </summary>
        private CellSlotFill freeCellSlot;
        /// <summary>
        /// Слот для слота свободной ячейки.
        /// </summary>
        [SerializeField]
        private Transform slotForFreeCellSlot=null;
        /// <summary>
        /// Задать местоположение для слота свободной ячейки через положение слота для нее.
        /// </summary>
        /// <param name="parent"></param>
        private void SetPlaceForFreeCellSlot(Transform parent, ArrowForFreeCellSlotFill slot)
        {
            //Если слот остался прежним, то должен произойти сдвиг.
            if (slot == this.freeCellSlot.arrowForFreeCellSlotFill)
            {
                BeginPerformShift(slot);
            }
            else
            {
                this.freeCellSlot.transform.parent = parent;
                this.freeCellSlot.transform.localPosition = Vector3.zero;

                ArrowForFreeCellSlotFill oldSlot = this.freeCellSlot.arrowForFreeCellSlotFill as ArrowForFreeCellSlotFill;
                slot.freeCellSlot = this.freeCellSlot;
                this.freeCellSlot.arrowForFreeCellSlotFill = slot;
                if (oldSlot != null)
                {
                    oldSlot.freeCellSlot = null;
                }
            }
        }
        /// <summary>
        /// Заполнить слот для свободной ячейкой.
        /// </summary>
        private void FillFreeCellSlot()
        {
            CellSlotFill freeCellSlot = Instantiate(this.cellSlotPrefab);
            freeCellSlot.SetCellType(this.playingField.freeFieldCell.CellType);
            SetFreeCell(freeCellSlot);
        }
        /// <summary>
        /// Очистить слот свободной ячейки. Переменная после этого равна null.
        /// </summary>
        private void ClearFreeCellSlot()
        {
            this.playingField.freeFieldCell.OnTurnedClockwise -= this.freeCellSlot.TurnClockwise;
            this.playingField.freeFieldCell.OnTurnedCountclockwise -= this.freeCellSlot.TurnCounterclockwise;
            this.freeCellSlot = null;
        }
        /// <summary>
        /// Задачть указанную ячейку как свободную и посметить в слот.
        /// </summary>
        /// <param name="slot"></param>
        private void SetFreeCell(CellSlotFill slot)
        {
            this.freeCellSlot = slot;
            this.freeCellSlot.transform.parent = this.slotForFreeCellSlot;
            this.freeCellSlot.transform.localPosition = Vector3.zero;
            this.playingField.freeFieldCell.OnTurnedClockwise += this.freeCellSlot.TurnClockwise;
            this.playingField.freeFieldCell.OnTurnedCountclockwise += this.freeCellSlot.TurnCounterclockwise;
        }

        #endregion Свободная ячейка.


        private void Awake()
        {
            //Задать слотам размер
            this.cellSlotPrefab.transform.localScale = new Vector3(sizeCellSlot, sizeCellSlot, sizeCellSlot);
            this.arrowForFreeCellSlotPrefab.transform.localScale = new Vector3(sizeCellSlot, sizeCellSlot, sizeCellSlot);

            FillFreeCellSlot();
            FillFieldWithCell();
            AddArrowSlotsForFreeCell();
        }
    }
}
