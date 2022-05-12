using UnityEngine;
using Assets.Scripts.GameModel.PlayingField.FieldCells;

namespace UI
{
    /// <summary>
    /// Скрипт для 
    /// </summary>
    public class UIFreeCell : GameViewOriginScript
    {

        /// <summary>
        /// Ссылка на свободную ячейку в модели.
        /// </summary>
        private FieldCell freeCellModel
        {
            get => this.gameModel.freeCell;
        }
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

        /// <summary>
        /// Прямоугольник слота картинки ячейки, который нужен для вращения.
        /// </summary>
        [SerializeField]
        private RectTransform cellUIImageSlot = null;
        /// <summary>
        /// Повернуть по часовой стрелке.
        /// </summary>
        public void TurnClockwise()
        {
            this.cellUIImageSlot.eulerAngles = new Vector3
                (
                this.cellUIImageSlot.eulerAngles.x,
                this.cellUIImageSlot.eulerAngles.y,
                this.cellUIImageSlot.eulerAngles.z-90
                );
            this.freeCellModel.TurnClockwise();
        }
        /// <summary>
        /// Повернуть против часовой стрелке.
        /// </summary>
        public void TurnCounterclockwise()
        {
            this.cellUIImageSlot.eulerAngles = new Vector3
                   (
                   this.cellUIImageSlot.eulerAngles.x,
                   this.cellUIImageSlot.eulerAngles.y,
                   this.cellUIImageSlot.eulerAngles.z + 90
                   );
            this.freeCellModel.TurnCounterClockwise();
        }
    }
}
