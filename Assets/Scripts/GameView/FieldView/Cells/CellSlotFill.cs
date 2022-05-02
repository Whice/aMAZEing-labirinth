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
        public Vector3 position
        {
            get => this.transform.localPosition;
            set => this.transform.localPosition = value;
        }

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


        private void Awake()
        {
            this.cellObject = this.gameObject;
        }
    }
}
