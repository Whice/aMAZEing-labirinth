using UnityEngine;
using Assets.Scripts.GameModel.PlayingField.FieldCells;
using System;
using UnityEngine.UI;
using Assets.Scripts.GameModel.PlayingField;
using Assets.Scripts.GameModel.TurnPhaseAndExtensions;
using DG.Tweening;
using System.Collections.Generic;

namespace UI
{
    /// <summary>
    /// Скрипт для управления свободной ячейкой из UI.
    /// </summary>
    public class UIFreeCell : GameUIOriginScript
    {
        /// <summary>
        /// Кнопка поворота по часовой стрелке.
        /// </summary>
        [SerializeField] private Button turnClockwiseButton;
        /// <summary>
        /// Кнопка поворота против часовой стрелки.
        /// </summary>
        [SerializeField] private Button turnCounterClockwiseButton;
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

        #region Видимость ячейки.

        [SerializeField] private RectTransform selfRectTransform = null;
        /// <summary>
        /// Считать ячейку скрытой.
        /// </summary>
        private Boolean isNeedShow
        {
            get => this.gameModel.currentPhase == TurnPhase.movingCell;
        }    
        /// <summary>
        /// Включить объект этого скрипта.
        /// </summary>
        public void EnableObject()
        {
            SetActive(true);
        }
        /// <summary>
        /// Изменить видимость ячейки.
        /// </summary>
        private void ChangeVisibility()
        {
            SetEnableObjectAnimationBegin();
        }
        private const float HIDE_SPEED = 1f;
        /// <summary>
        /// Начать анимацию включения или отключения объекта этого скрипта.
        /// </summary>
        public void SetEnableObjectAnimationBegin()
        {
            if (this.isNeedShow)
            {
                this.selfRectTransform.DOScale(1, HIDE_SPEED * Time.timeScale);
            }
            else
            {
                this.selfRectTransform.DOScale(0, HIDE_SPEED * Time.timeScale);
            }
        }

        #endregion Видимость ячейки.

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
        private Dictionary<CellType, Sprite> cellUISprites;
        /// <summary>
        /// Изменить спрайт ячейки.
        /// </summary>
        private void ChangeSprite()
        {
            if (this.isNeedShow)
            {
                ResetRotation();
                TurnClockwise(this.freeCellModel.turnsClockwiseCount);
                this.slotForCellUISprite.sprite = this.cellUISprites[this.freeCellModel.CellType];
            }
        }
        protected override void Subscribe()
        {
            base.Subscribe();
            this.gameModel.onPhaseChange += ChangeVisibility;
            this.gameModel.onPhaseChange += ChangeSprite;
        }
        protected override void Unsubscribe()
        {
            this.fieldCells.OnFreeCellChange -= ChangeSprite;
            this.gameModel.onPhaseChange -= ChangeVisibility;
            base.Unsubscribe();
        }
        public override void Initialize()
        {
            base.Initialize();

            if (this.lineCellUISprite == null)
            {
                LogError(nameof(this.lineCellUISprite) + " not found!");
            }
            if (this.cornerCellUISprite == null)
            {
                LogError(nameof(this.cornerCellUISprite) + " not found!");
            }
            if (this.threeDirectionCellUISprite == null)
            {
                LogError(nameof(this.threeDirectionCellUISprite) + " not found!");
            }
            if (this.cellUISprites == null)
            {
                this.cellUISprites = new Dictionary<CellType, Sprite>
                {
                    { CellType.line, this.lineCellUISprite },
                    { CellType.corner, this.cornerCellUISprite },
                    { CellType.threeDirection, this.threeDirectionCellUISprite }
                };
            }
            ChangeSprite();
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
            this.cellUIImageSlot.rotation = Quaternion.identity;
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
        private void TurnCounterClockwise(Int32 count = 1)
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

        protected override void Awake()
        {
            base.Awake();
            this.turnClockwiseButton.onClick.AddListener(() => TurnClockwise());
            this.turnCounterClockwiseButton.onClick.AddListener(() => TurnCounterClockwise());
            SetEnableObjectAnimationBegin();
        }
    }
}
