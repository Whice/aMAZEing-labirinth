using System;
using UnityEngine;

namespace Assets.Scripts.GameView
{
    /// <summary>
    /// Слот стрелочки, куда будет помещена свободная ячейка.
    /// <br/>Размер и позиция слота зависят от соответствующих параметров ячейки ячейки.
    /// </summary>
    public class ArrowForFreeCellSlotFill : CellSlotFill
    {
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

            //Подгонка размеры ячейки под размер слота
            Single sizeRatio = this.transform.localScale.x / this.cellTransform.localScale.x;
            this.cellTransform.localScale *= sizeRatio;
            sizeRatio = this.transform.localScale.x / this.arrowTransform.localScale.x;
            this.arrowTransform.localScale *= sizeRatio;
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
        


        private void Awake()
        {
            this.arrowObject = this.cellObject = this.gameObject;
        }

        /// <summary>
        /// Событие клика на стрелочку.
        /// </summary>
        public Action<Transform> OnArrowClicked;
        private void OnMouseUp()
        {
            this.OnArrowClicked?.Invoke(this.transform);
            LogInfo(this.name);
        }
        private void OnMouseDown()
        {
        }
    }
}
