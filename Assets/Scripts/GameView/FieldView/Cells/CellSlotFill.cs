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
        /// Transform ячейкив в слоте.
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
        public void SetCellType(CellType type)
        {
            this.cellTypePrivate = type;

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

            //Подгонка размеры ячейки под размер слота
            Single sizeRatio = this.transform.localScale.x / this.cellTransform.localScale.x;
            this.cellTransform.localScale *= sizeRatio;
        }


        private void Awake()
        {
            this.cellObject = this.gameObject;
        }
    }
}
