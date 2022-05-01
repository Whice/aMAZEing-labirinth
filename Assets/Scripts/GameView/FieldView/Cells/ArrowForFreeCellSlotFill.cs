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
        private Vector3 originRotate = new Vector3(999, 0, 0);
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
            this.cellObject = GetPrefabClone("ArrowForFreeCell");


            this.cellTransform.parent = this.transform;
            this.cellTransform.position = Vector3.zero;

            //Подгонка размеры ячейки под размер слота
            Single sizeRatio = this.transform.localScale.x / this.cellTransform.localScale.x;
            this.cellTransform.localScale *= sizeRatio;
        }
        /// <summary>
        /// Объект стрелочки из провайдера для слота.
        /// </summary>
        [SerializeField]
        private GameObject arrowObject = null;
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


        private void Awake()
        {
            this.arrowObject = this.cellObject = this.gameObject;
        }
    }
}
