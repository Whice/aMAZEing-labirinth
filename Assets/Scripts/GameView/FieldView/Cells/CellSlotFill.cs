using Assets.Scripts.GameModel.PlayingField.FieldCells;
using System;
using UnityEngine;

namespace Assets.Scripts.GameView
{
    /// <summary>
    /// Слот для ячейки поля.
    /// <br/>Размер и позиция слота зависят от соответствующих параметров ячейки ячейки.
    /// </summary>
    public class CellSlotFill : GameViewOriginScript
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
        private CellType cellTypePrivate = CellType.unknown;
        /// <summary>
        /// Тип ячейки.
        /// </summary>
        public CellType cellType
        {
            get => this.cellTypePrivate;
        }


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
        public void SetSlotPosition(Int32 x, Int32 y, Single positionMultiplier)
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
        public void SetTargetSlotPosition(Int32 x, Int32 y, Single positionMultiplier, Single requiredTimeForMove)
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

        /// <summary>
        /// Получить клон указанного по имени префаба из провайдер.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected GameObject GetPrefabClone(String name)
        {
            return GameManager.instance.prefabsProvider.GetPrefabClone(name);
        }
        /// <summary>
        /// Заполнить слот ячейкой заданного типа.
        /// </summary>
        /// <param name="type"></param>
        private void SetCellType(GameObject cellObject, CellType type)
        {
            this.cellTypePrivate = type;
            this.cellObject = cellObject;

            this.cellTransform.parent = this.transform;
            this.cellTransform.position = Vector3.zero;

            //Подгонка размеры ячейки под размер слота
            Single sizeRatio = this.transform.localScale.x / this.cellTransform.localScale.x;
            this.cellTransform.localScale *= sizeRatio;
        }
        /// <summary>
        /// Заполнить слот ячейкой заданного типа.
        /// </summary>
        /// <param name="type"></param>
        public void SetCellType(CellType type)
        {

            switch (type)
            {
                case CellType.corner:
                    {
                        SetCellType(GetPrefabClone("CornerCell"), type);
                        break;
                    }
                case CellType.line:
                    {
                        SetCellType(GetPrefabClone("LineCell"), type);
                        break;
                    }
                case CellType.threeDirection:
                    {
                        SetCellType(GetPrefabClone("ThreeDirectionCell"), type);
                        break;
                    }
            }
        }
        /// <summary>
        /// Установить ячейку для этого слота из указанного слота.
        /// </summary>
        /// <param name="slot"></param>
        public void SetCellFromSlot(CellSlotFill slot)
        {
            SetCellType(slot.cellObject, slot.cellTypePrivate);
        }
        /// <summary>
        /// Повернуть слот по часовой стрелке.
        /// </summary>
        /// <param name="count"></param>
        public void TurnClockwise(Int32 count)
        {
            count = count % 4;
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
        /// <param name="count"></param>
        public void TurnCounterclockwise(Int32 count)
        {
            count = count % 4;
            Single newAngle = this.transform.eulerAngles.y - 90 * count;
            Single correctAngle = (Single)((Int32)(newAngle) % 360);
            this.transform.eulerAngles = new Vector3
                (
                this.transform.eulerAngles.x,
                correctAngle,
                this.transform.eulerAngles.z
                );
        }

        /// <summary>
        /// Ссылка на слот для слота свободной ячейки.
        /// </summary>
        public CellSlotFill arrowForFreeCellSlotFill = null;

        private void Awake()
        {
            this.cellObject = this.gameObject;
        }

        public override string ToString()
        {
            return "Slot with " + nameof(this.positionInField) + ": "
                + this.positionInField.x.ToString() + "; " + this.positionInField.y.ToString();
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

            return true;
        }
    }
}
