using UnityEngine;
using Assets.Scripts.GameModel.PlayingField.FieldCells;
using System;
using UnityEngine.UI;
using Assets.Scripts.GameModel.PlayingField;

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
        /// Модель игрового поля.
        /// </summary>
        private Field fieldCells
        {
            get => this.gameModel.field;
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
        /// <summary>
        /// Место для картинки ячейки.
        /// </summary>
        [SerializeField]
        private Image slotForCellUISprite = null;
        /// <summary>
        /// Изменить спрайт ячейки.
        /// </summary>
        private void ChageSprite()
        {
            ResetRotation();
            TurnClockwise(this.freeCellModel.turnsClockwiseCount);
            switch (this.freeCellModel.CellType)
            {
                case CellType.line:
                    {
                        this.slotForCellUISprite.sprite = this.lineCellUISprite;
                        break;
                    }
                case CellType.corner:
                    {
                        this.slotForCellUISprite.sprite = this.cornerCellUISprite;
                        break;
                    }
                case CellType.threeDirection:
                    {
                        this.slotForCellUISprite.sprite = this.threeDirectionCellUISprite;
                        break;
                    }
            }
        }

        private void Awake()
        {
            if (this.lineCellUISprite == null)
            {
                LogError(nameof(this.lineCellUISprite) + " not found!");
            }
            if (this.lineCellUISprite == null)
            {
                LogError(nameof(this.cornerCellUISprite) + " not found!");
            }
            if (this.lineCellUISprite == null)
            {
                LogError(nameof(this.threeDirectionCellUISprite) + " not found!");
            }
            ChageSprite();

            this.fieldCells.OnFreeCellChange += ChageSprite;
        }
        private void OnDestroy()
        {
            this.fieldCells.OnFreeCellChange -= ChageSprite;
        }

        #region Поворот.

        /// <summary>
        /// Количество поворотов по часовой стрелке.
        /// <br/>По сути определяет направление.
        /// <br/>Значение может быть с 0 по 3.
        /// </summary>
        private Int32 turnsClockwiseCount = 2;
        /// <summary>
        /// Прямоугольник слота картинки ячейки, который нужен для вращения.
        /// </summary>
        [SerializeField]
        private RectTransform cellUIImageSlot = null;
        /// <summary>
        /// Сбросить поворот слота спрайта.
        /// </summary>
        private void ResetRotation()
        {
            TurnCounterclockwise(this.turnsClockwiseCount);
        }
        /// <summary>
        /// Повернуть по часовой стрелке.
        /// </summary>
        public void TurnClockwise(Int32 count = 1)
        {
            count = count % 4;
            this.turnsClockwiseCount = (this.turnsClockwiseCount + count) % 4;
            Single newAngle = this.cellUIImageSlot.eulerAngles.z - 90 * count;
            Single correctAngle = (Single)((Int32)(newAngle) % 360);

            this.cellUIImageSlot.eulerAngles = new Vector3
                (
                this.cellUIImageSlot.eulerAngles.x,
                this.cellUIImageSlot.eulerAngles.y,
                correctAngle
                );

            this.freeCellModel.TurnClockwise(count);
        }
        /// <summary>
        /// Повернуть против часовой стрелке.
        /// </summary>
        public void TurnCounterclockwise(Int32 count = 1)
        {
            count = count % 4;
            this.turnsClockwiseCount = 4 - ((this.turnsClockwiseCount + count) % 4);
            Single newAngle = this.cellUIImageSlot.eulerAngles.z + 90 * count;
            Single correctAngle = (Single)((Int32)(newAngle) % 360);

            this.cellUIImageSlot.eulerAngles = new Vector3
                (
                this.cellUIImageSlot.eulerAngles.x,
                this.cellUIImageSlot.eulerAngles.y,
                correctAngle
                );

            this.freeCellModel.TurnCounterClockwise(count);
        }

        #endregion Поворот.
    }
}
