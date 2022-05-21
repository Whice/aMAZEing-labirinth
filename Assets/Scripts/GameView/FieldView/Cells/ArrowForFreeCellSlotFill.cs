using System;
using UnityEngine;

namespace Assets.Scripts.GameView
{
    /// <summary>
    /// Сторона игрового поля.
    /// </summary>
    public enum FieldSide
    {
        /// <summary>
        /// Неопознано.
        /// </summary>
        unknow=0,
        /// <summary>
        /// Левая.
        /// </summary>
        left=1,
        /// <summary>
        /// Правая.
        /// </summary>
        right=2,
        /// <summary>
        /// Верхняя.
        /// </summary>
        top=3,
        /// <summary>
        /// Нижняя.
        /// </summary>
        bottom=4,
    }

    /// <summary>
    /// Слот стрелочки, куда будет помещена свободная ячейка.
    /// <br/>Размер и позиция слота зависят от соответствующих параметров ячейки ячейки.
    /// </summary>
    public class ArrowForFreeCellSlotFill : CellSlotFill
    {
        /// <summary>
        /// Высота слота стрелочки.
        /// </summary>
        public const Single HEIGHT_FOR_ARROW_SLOT = 0.0f;
        /// <summary>
        /// Изначальный поворот ячейки.
        /// </summary>
        private Vector3 originRotate = new Vector3(999, 0, 0);
        /// <summary>
        /// Задать изначальный 
        /// </summary>
        public void SetOriginRotate()
        {
            if (this.originRotate.x < 360)
            {
                this.originRotate = this.transform.rotation.eulerAngles;
            }
        }
        /// <summary>
        /// Заполнить слот ячейкой состреллочкой.
        /// </summary>
        /// <param name="type"></param>
        public void SetArrowForFreeCell()
        {
            this.arrowObject = GetPrefabClone("ArrowForFreeCell");

            this.cellTransform.parent = this.transform;
            this.arrowTransform.parent = this.transform;
            this.cellTransform.position = Vector3.zero;
            this.arrowTransform.position = Vector3.zero;
        }
        /// <summary>
        /// Объект стрелочки из провайдера для слота.
        /// </summary>
        [SerializeField]
        private GameObject arrowObject = null;
        /// <summary>
        /// Transform объекта стрелочки.
        /// </summary>
        private Transform arrowTransform
        {
            get => this.arrowObject.transform;
        }
        /// <summary>
        /// Принудительно установить позицию на поле.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void SetPositionInField(Int32 x, Int32 y)
        {
            this.positionInFieldPrivate.x = x;
            this.positionInFieldPrivate.y = y;
        }
        /// <summary>
        /// Слот стрелочки с другой стороны поля.
        /// </summary>
        private ArrowForFreeCellSlotFill arrowOnOpositeSidePrivate = null;
        /// <summary>
        /// Слот стрелочки с другой стороны поля.
        /// </summary>
        public ArrowForFreeCellSlotFill arrowOnOpositeSide
        {
            get => this.arrowOnOpositeSidePrivate;
        }
        /// <summary>
        /// Установить слот стрелочки с другой стороны поля.
        /// </summary>
        public void SetArrowOnOpositeSide(ArrowForFreeCellSlotFill slot)
        {
            if (this.arrowOnOpositeSidePrivate == null)
            {
                this.arrowOnOpositeSidePrivate = slot;
            }
            else
            {
                LogError(nameof(this.arrowOnOpositeSidePrivate) + "has already been set!");
            }
        }

        /// <summary>
        /// Свободная ячейка в этой стрелочке.
        /// </summary>
        [HideInInspector]
        public CellSlotFill freeCellSlot = null;

        /// <summary>
        /// Сторона поля, на которой находится эта стрелочка.
        /// </summary>
        private FieldSide sidePrivate = FieldSide.unknow;
        /// <summary>
        /// Сторона поля, на которой находится эта стрелочка.
        /// </summary>
        public FieldSide side
        {
            get => this.sidePrivate;
        }

        /// <summary>
        /// Задать сторону поля, на которой находится эта стрелочка, если она еще не была задана.
        /// </summary>
        /// <param name="side">Сторона, которая должна быть опознаной для того, чтобы она должна была быть установалена.</param>
        public void SetFieldSide(FieldSide side)
        {
            if (side == FieldSide.unknow)
            {
                LogError("Side is unknow!");
                return;
            }
            if (this.sidePrivate != FieldSide.unknow)
            {
                LogError("Side has already been set!");
                return;
            }

            if (this.sidePrivate == FieldSide.unknow)
            {
                this.sidePrivate = side;
            }
        }


        private void Awake()
        {
            this.arrowObject = this.cellObject = this.gameObject;
        }

        #region Симуляция/считывание клика по слоту.

        /// <summary>
        /// Событие клика на стрелочку.
        /// </summary>
        public event Action<Transform, ArrowForFreeCellSlotFill> OnArrowClicked;
        public override void SimulateOnClick()
        {
            base.SimulateOnClick();
            this.OnArrowClicked?.Invoke(this.transform, this);
        }

        #endregion Симуляция/считывание клика по слоту.

    }
}
