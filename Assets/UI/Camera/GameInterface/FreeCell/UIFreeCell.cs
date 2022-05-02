using UnityEngine;
using Assets.Scripts.GameModel.PlayingField.FieldCells;

namespace UI
{
    /// <summary>
    /// ������ ��� 
    /// </summary>
    public class UIFreeCell : GameViewOriginScript
    {

        /// <summary>
        /// ������ �� ��������� ������ � ������.
        /// </summary>
        private FieldCell freeCellModel
        {
            get => this.gameModel.freeCell;
        }
        private Sprite lineCellUISprite = null;
        /// <summary>
        /// ������ � ������������ �������.
        /// </summary>
        [SerializeField]
        private Sprite cornerCellUISprite = null;
        /// <summary>
        /// ������ � ������������� ����� ������.
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
        /// ������������� ����� �������� ������, ������� ����� ��� ��������.
        /// </summary>
        [SerializeField]
        private RectTransform cellUIImageSlot = null;
        /// <summary>
        /// ��������� �� ������� �������.
        /// </summary>
        /// <param name="count">���������� ���������.</param>
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
        /// ��������� ������ ������� �������.
        /// </summary>
        /// <param name="count">���������� ���������.</param>
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
