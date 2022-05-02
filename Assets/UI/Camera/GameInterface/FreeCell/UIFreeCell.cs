using UnityEngine;

namespace UI
{
    /// <summary>
    /// Скрипт для 
    /// </summary>
    public class UIFreeCell : GameViewOriginScript
    {
        /// <summary>
        /// Спрайт с нарисованной линией.
        /// </summary>
        [SerializeField]
        private Sprite lineCellUISprite = null;
        /// <summary>
        /// Спрайт с нарисованным уголком.
        /// </summary>
        [SerializeField]
        private Sprite cornerCellUISprite = null;
        /// <summary>
        /// Спрайт с нарисованными тремя путями.
        /// </summary>
        [SerializeField]
        private Sprite threeDirectionCellUISprite = null;

        private void Awake()
        {
            if(this.lineCellUISprite==null)
            {
                LogError(nameof(this.lineCellUISprite) + " not found!");
            }
            if(this.lineCellUISprite==null)
            {
                LogError(nameof(this.cornerCellUISprite) + " not found!");
            }
            if(this.lineCellUISprite==null)
            {
                LogError(nameof(this.threeDirectionCellUISprite) + " not found!");
            }
        }


        [SerializeField]
        private RectTransform cellUISlot = null;
        public void TurnClockwise()
        {
            this.cellUISlot.eulerAngles = new Vector3
                (
                this.cellUISlot.eulerAngles.x,
                this.cellUISlot.eulerAngles.y,
                this.cellUISlot.eulerAngles.z-90
                );
        }
        public void TurnCounterclockwise()
        {
            this.cellUISlot.eulerAngles = new Vector3
                   (
                   this.cellUISlot.eulerAngles.x,
                   this.cellUISlot.eulerAngles.y,
                   this.cellUISlot.eulerAngles.z + 90
                   );
        }
    }
}
