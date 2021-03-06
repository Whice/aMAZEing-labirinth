using Assets.Scripts.GameModel.PlayingField.FieldCells;
using Assets.Scripts.GameModel.PlayingField.Treasures;
using System;
using UnityEngine;

namespace Assets.Scripts.GameView
{
    /// <summary>
    /// Слот для ячейки поля.
    /// <br/>Размер и позиция слота зависят от соответствующих параметров ячейки ячейки.
    /// </summary>
    public class CellSlotFill : GameWorldViewOriginScript
    {
        /// <summary>
        /// Объект ячейки в слоте.
        /// </summary>
        protected GameObject cellObject = null;
        /// <summary>
        /// Transform ячейки в слоте.
        /// </summary>
        protected Transform cellTransform
        {
            get => this.cellObject.transform;
        }

        /// <summary>
        /// Тип ячейки.
        /// </summary>
        public CellType cellType
        {
            get => this.modelCell.CellType;
        }

        #region Слоты для аватаров игроков.

        /// <summary>
        /// Слоты для игроков.
        /// </summary>
        [SerializeField]
        private PlayerAvatarSlot[] avatarSlots = new PlayerAvatarSlot[4];
        /// <summary>
        /// Содежит слот аватара игрока под указанным номером.
        /// </summary>
        /// <param name="playerNumber"></param>
        /// <returns></returns>
        public Boolean IsContainsPlayerAvatar(Int32 playerNumber)
        {
            if (avatarSlots[playerNumber].currentPlayer == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        /// <summary>
        /// Получить игрока под указанным номером.
        /// </summary>
        /// <param name="playerNumber"></param>
        /// <returns></returns>
        public PlayerAvatarSlot GetPlayerAvatarSlot(Int32 playerNumber)
        {
            return avatarSlots[playerNumber];
        }
        /// <summary>
        /// Установить аватар игрока по номеру игрока.
        /// </summary>
        public void SetPlayerAvatarSlot(Int32 playerNumber)
        {
            this.avatarSlots[playerNumber].SetPlayer(playerNumber);
        }
        /// <summary>
        /// Поменять местами слоты аватара игрока для указанного игрока.
        /// </summary>
        /// <param name="cellSlot">Слот, с которым будет происходить обмен.</param>
        /// <param name="playerNumber">Номер игрока.</param>
        public void SwapAvatarSlot(CellSlotFill cellSlot, Int32 playerNumber)
        {
            (this.avatarSlots[playerNumber], cellSlot.avatarSlots[playerNumber]) = (cellSlot.avatarSlots[playerNumber], this.avatarSlots[playerNumber]);

            (this.avatarSlots[playerNumber].transform.parent, cellSlot.avatarSlots[playerNumber].transform.parent) 
                = (cellSlot.avatarSlots[playerNumber].transform.parent, this.avatarSlots[playerNumber].transform.parent);


            this.avatarSlots[playerNumber].transform.localPosition = Vector3.zero;
            cellSlot.avatarSlots[playerNumber].transform.localPosition = Vector3.zero;
        }

        #endregion Слоты для аватаров игроков.

        #region Положение слота в пространстве.

        #region Положение по высоте.

        /// <summary>
        /// Изначальная высота слота ячейки.
        /// </summary>
        private Single originalHeight;
        /// <summary>
        /// Высота слота.
        /// </summary>
        public Single height
        {
            get => this.transform.localPosition.y;
            set => this.transform.localPosition = new Vector3(this.transform.localPosition.x, value, this.transform.localPosition.z);
        }
        /// <summary>
        /// Сбросить значение высоты на изначальное.
        /// </summary>
        public void ResetHeight()
        {
            this.height = this.originalHeight;
        }
        #endregion Положение по высоте.

        #region Положение по горизонтали.

        /// <summary>
        /// Множитель расположения, чтобы было небольшое расстояние между слотами.
        /// </summary>
        public static Single positionMultiplier;
        /// <summary>
        /// Размер слота.
        /// </summary>
        public Vector3 size
        {
            set => this.transform.localScale = value;
            get => this.transform.localScale;
        }
        /// <summary>
        /// Положение слота.
        /// </summary>
        public Vector3 localPosition
        {
            get => this.transform.localPosition;
            set => this.transform.localPosition = value;
        }
        /// <summary>
        /// Позиция на поле.
        /// </summary>
        protected Vector2Int positionInFieldPrivate;
        /// <summary>
        /// Позиция на поле.
        /// </summary>
        public Vector2Int positionInField
        {
            get => this.positionInFieldPrivate;
        }
        /// <summary>
        /// Установить позицию слота в сетке поля.
        /// </summary>
        /// <param name="x">Позиция с лева на право.</param>
        /// <param name="y">Позиция в глубину.</param>
        /// <param name="positionMultiplier">Множитель расположения, чтобы было небольшое расстояние между слотами.</param>
        public void SetSlotPosition(Int32 x, Int32 y)
        {
            this.positionInFieldPrivate.x = x;
            this.positionInFieldPrivate.y = y;
            this.localPosition = new Vector3
                        (
                        x * positionMultiplier,
                        this.localPosition.y,
                        y * positionMultiplier
                        );
        }

        #endregion Положение по горизонтали.

        #endregion Положение слота в пространстве.

        #region Перемещение слота со временем.

        /// <summary>
        /// Максимальное, требуемое время перемещения.
        /// </summary>
        private Single requiredTimeForMove;
        /// <summary>
        /// Общее время перемещения.
        /// </summary>
        private Single timeForMove;
        /// <summary>
        /// Целевое положение слота.
        /// </summary>
        private Vector3 targetLocalPosition;
        /// <summary>
        /// Изаначальное положение слота перед началом движения.
        /// </summary>
        private Vector3 originLocalPosition;
        /// <summary>
        /// Задать цель - новое положение слота.
        /// </summary>
        /// <param name="x">Позиция в поле с лева на право.</param>
        /// <param name="y">Позиция в поле в глубину.</param>
        /// <param name="positionMultiplier">Множитель позиции.</param>
        /// <param name="requiredTimeForMove">Требуемое время на перемещение.</param>
        public void SetTargetSlotPosition(Int32 x, Int32 y, Single requiredTimeForMove)
        {
            this.targetLocalPosition = new Vector3
                        (
                        x * positionMultiplier,
                        this.localPosition.y,
                        y * positionMultiplier
                        );
            this.originLocalPosition = this.localPosition;
            this.requiredTimeForMove = requiredTimeForMove;
            this.timeForMove = 0;
        }
        /// <summary>
        /// Осуществлять движение слота к заданной цели - новой позиции.
        /// </summary>
        public void MoveToTargetLocalPosition()
        {
            this.timeForMove += Time.deltaTime;
            Single percentOfMove = this.timeForMove / this.requiredTimeForMove;
            if (percentOfMove > 1)
            {
                percentOfMove = 1;
            }
            this.localPosition = Vector3.Lerp(this.originLocalPosition, this.targetLocalPosition, percentOfMove);
        }

        #endregion Перемещение слота со временем.

        #region Поворот.

        /// <summary>
        /// Количество поворотов по часовой стрелке.
        /// <br/>По сути определяет направление.
        /// <br/>Значение может быть с 0 по 3.
        /// </summary>
        private Int32 turnsClockwiseCount = 0;
        /// <summary>
        /// Повернуть слот по часовой стрелке.
        /// </summary>
        /// <param name="count">Количество вращений.</param>
        public void TurnClockwise(Int32 count)
        {
            count = count % 4;
            this.turnsClockwiseCount = (this.turnsClockwiseCount + count) % 4;
            Single newAngle = this.transform.eulerAngles.y + 90 * count;
            Single correctAngle = (Single)((Int32)(newAngle) % 360);
            this.transform.eulerAngles = new Vector3
                (
                this.transform.eulerAngles.x,
                correctAngle,
                this.transform.eulerAngles.z
                );
        }
        /// <summary>
        /// Повернуть слот против часовой стрелке.
        /// </summary>
        /// <param name="count">Количество вращений.</param>
        public void TurnCounterclockwise(Int32 count)
        {
            count = count % 4;
            this.turnsClockwiseCount = 4 - ((this.turnsClockwiseCount + count) % 4);
            Single newAngle = this.transform.eulerAngles.y - 90 * count;
            Single correctAngle = (Single)((Int32)(newAngle) % 360);
            this.transform.eulerAngles = new Vector3
                (
                this.transform.eulerAngles.x,
                correctAngle,
                this.transform.eulerAngles.z
                );
        }

        #endregion Поворот.

        #region Ячейка модели.

        /// <summary>
        /// Заполнить слот ячейкой заданного типа.
        /// </summary>
        /// <param name="type"></param>
        private void SetCellType(CellType type)
        {

            switch (type)
            {
                case CellType.corner:
                    {
                        this.cellObject = GetPrefabClone("CornerCell");
                        break;
                    }
                case CellType.line:
                    {
                        this.cellObject = GetPrefabClone("LineCell");
                        break;
                    }
                case CellType.threeDirection:
                    {
                        this.cellObject = GetPrefabClone("ThreeDirectionCell");
                        break;
                    }
            }

            this.cellTransform.parent = this.transform;
            this.cellTransform.position = Vector3.zero;
        }
        /// <summary>
        /// Установить ячейку для этого слота из указанного слота.
        /// </summary>
        /// <param name="slot"></param>
        public void SetCellFromSlot(CellSlotFill slot)
        {
            this.cellObject = slot.cellObject;
            SetCellFromModelCell(slot.modelCell);
        }
        /// <summary>
        /// Ячейка поля из модели.
        /// </summary>
        private FieldCell modelCell;
        /// <summary>
        /// Задать параметры для слота через ячейку поля из модели.
        /// </summary>
        /// <param name="cell">Ячейка поля из модели.</param>
        public void SetCellFromModelCell(FieldCell cell)
        {
            if (this.modelCell != null)
            {
                this.modelCell.OnTurnedClockwise -= this.TurnClockwise;
                this.modelCell.OnTurnedCountclockwise -= this.TurnCounterclockwise;
            }

            this.modelCell = cell;
            SetCellType(cell.CellType);
            TurnClockwise(cell.turnsClockwiseCount);

            this.modelCell.OnTurnedClockwise += TurnClockwise;
            this.modelCell.OnTurnedCountclockwise += TurnCounterclockwise;

            FillTreasureSlot();
        }

        #endregion Ячейка модели.

        #region Сокровище ячейки.

        /// <summary>
        /// Ячейка для сокровища. Може быть null, не забывать проверять.
        /// </summary>
        [SerializeField]
        private TreasureSlotFill treasureSlot = null;
        /// <summary>
        /// Заполнить слот с сокровищем.
        /// </summary>
        private void FillTreasureSlot()
        {
            if (this.treasureSlot != null && this.modelCell!=null)
            {
                TreasureAndStartPointsType type = this.modelCell.treasureOrStartPoints;
                if (type != TreasureAndStartPointsType.empty)
                {
                    this.treasureSlot.SetTreasure(this.modelCell.treasureOrStartPoints);
                }
            }
        }

        #endregion Сокровище ячейки.

        #region Стартовые точки.

        /// <summary>
        /// Объект слота-точки старта игрока.
        /// </summary>
        [SerializeField]
        private GameObject objectPointSlot = null;
        /// <summary>
        /// Свет на точке старта.
        /// </summary>
        [SerializeField]
        private Light objectPointSlotLight = null;
        /// <summary>
        /// Установить точку старта игроку.
        /// </summary>
        /// <param name="playerColor">Цвет игрока.</param>
        public void SetPlayerStartPoint(Color playerColor)
        {
            this.objectPointSlot.SetActive(true);
            this.objectPointSlotLight.color = playerColor;
        }

        #endregion Стартовые точки.

        /// <summary>
        /// Ссылка на слот для слота свободной ячейки.
        /// </summary>
        public CellSlotFill arrowForFreeCellSlotFill = null;

        protected override void Awake()
        {
            this.cellObject = this.gameObject;
            this.originalHeight = this.transform.localPosition.y;
        }

        public override string ToString()
        {
            return "Slot with " + nameof(this.positionInField) + ": "
                + this.positionInField.x.ToString() + "; " + this.positionInField.y.ToString()
                + "\nRotate count: " + this.turnsClockwiseCount.ToString()
                + "; TypeCell: " + this.cellType.ToString();
        }

        /// <summary>
        /// Напечатать инфо о позиции ячейки.
        /// </summary>
        private void PrintLogInfoCell(FieldCell cell)
        {
            try
            {
                LogInfo(nameof(CellSlotFill) + " " + this.turnsClockwiseCount.ToString() + "; " +
                        nameof(FieldCell) + " " + cell.turnsClockwiseCount.ToString() + ";" +
                        nameof(this.positionInField) + ": " + this.positionInField.x.ToString() + "; "
                        + this.positionInField.y.ToString() + ";");
            }
            catch
            {

            }
        }

        /// <summary>
        /// Проверить соответствие ячеки этого слота указанной ячейке модели.
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        public Boolean IsEqualWithModelCell(FieldCell cell)
        {
            if (cell.CellType != this.cellType)
            {
                return false;
            }
            if (cell.turnsClockwiseCount != this.turnsClockwiseCount)
            {
                //PrintLogInfoCell(cell);
                return false;
            }

            return true;
        }


        #region Симуляция/считывание клика по объекту.

        /// <summary>
        /// Событие клика на этот слот.
        /// </summary>
        public event Action<CellSlotFill> OnCellSlotClicked;
        public override void SimulateOnClick()
        {
            //PrintLogInfoCell(this.modelCell);
            base.SimulateOnClick();
            this.OnCellSlotClicked?.Invoke(this);
        }

        #endregion Симуляция/считывание клика по объекту.
    }
}