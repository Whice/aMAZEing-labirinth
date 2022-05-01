using Assets.Scripts.GameModel.PlayingField;
using Assets.Scripts.GameModel.PlayingField.FieldCells;
using System;
using UnityEngine;

namespace Assets.Scripts.GameView
{
    /// <summary>
    /// Представление игрового поля.
    /// </summary>
    public class FieldView: MonoBehaviour
    {
        /// <summary>
        /// Слот, куда будет помещена ячейка.
        /// </summary>
        [SerializeField]
        private CellSlotFill cellSlotPrefab = null;
        /// <summary>
        /// Слот, куда будут помещены ячейк поля.
        /// </summary>
        [SerializeField]
        private Transform slotForFieldSlots = null;
        /// <summary>
        /// Слот, куда будет помещеная свободная ячейка, а пока ее нет, то там будет стрелка.
        /// </summary>
        [SerializeField]
        private ArrowForFreeCellSlotFill arrowForFreeCellSlotPrefab = null;
        /// <summary>
        /// Размер слота ячейки.
        /// </summary>
        [SerializeField]
        private Single sizeCellSlot = 1f;
        /// <summary>
        /// Размер слота ячейки.
        /// Зависит от размера ячейки.
        /// </summary>
        [SerializeField]
        private Single spacingBetweenCellSlot
        {
            get => this.sizeCellSlot * 0.2f;
        }

        /// <summary>
        /// Все слоты поля.
        /// </summary>
        private CellSlotFill[,] slots = new CellSlotFill[Field.FIELD_SIZE, Field.FIELD_SIZE];
        /// <summary>
        /// 4 линии для стрелочек-мест для свободных ячеек.
        /// <br/>1е несколько - верхняя сторона.
        /// <br/>2е несколько - правая сторона.
        /// <br/>3е несколько - нижняя сторона.
        /// <br/>4е несколько - левая сторона.
        /// </summary>
        private ArrowForFreeCellSlotFill[] arrowForFreeCellSlot = new ArrowForFreeCellSlotFill[4 * Field.FIELD_SIZE / 2];

        /// <summary>
        /// Добавить слоты для стрелочек - мест, куда будет помещена свободная ячейка.
        /// </summary>
        private void AddArrowSlotForFreeCell()
        {
            Single positionMultiplier = this.sizeCellSlot + this.spacingBetweenCellSlot;
            Int32 currentCellNumber = 0;
            const Int32 COUNT_SLOTS_IN_LINE = Field.FIELD_SIZE / 2;
            Vector3 leftLineStart = new Vector3(0, 0, 0);
            for (Int32 i = 0; i < COUNT_SLOTS_IN_LINE; i++)
            {
                //левая линия
                currentCellNumber = i + COUNT_SLOTS_IN_LINE * 3;
                this.arrowForFreeCellSlot[currentCellNumber] = Instantiate(this.arrowForFreeCellSlotPrefab);
                this.arrowForFreeCellSlot[currentCellNumber].SetArrowForFreeCell();
                this.arrowForFreeCellSlot[currentCellNumber].position = new Vector3
                        (
                        leftLineStart.x - positionMultiplier,
                        leftLineStart.y,
                        leftLineStart.z + (i * 2 + 1) * positionMultiplier
                        );
                //нижяя линия
                currentCellNumber = i + COUNT_SLOTS_IN_LINE * 2;
                this.arrowForFreeCellSlot[currentCellNumber] = Instantiate(this.arrowForFreeCellSlotPrefab);
                this.arrowForFreeCellSlot[currentCellNumber].SetArrowForFreeCell();
                this.arrowForFreeCellSlot[currentCellNumber].TurnClockwise(3);
                this.arrowForFreeCellSlot[currentCellNumber].position = new Vector3
                        (
                        leftLineStart.x + (i * 2 + 1) * positionMultiplier,
                        leftLineStart.y,
                        leftLineStart.z - positionMultiplier
                        );
                //правая линия
                currentCellNumber = i + COUNT_SLOTS_IN_LINE;
                this.arrowForFreeCellSlot[currentCellNumber] = Instantiate(this.arrowForFreeCellSlotPrefab);
                this.arrowForFreeCellSlot[currentCellNumber].SetArrowForFreeCell();
                this.arrowForFreeCellSlot[currentCellNumber].TurnClockwise(2);
                this.arrowForFreeCellSlot[currentCellNumber].position = new Vector3
                        (
                        leftLineStart.x + positionMultiplier * Field.FIELD_SIZE,
                        leftLineStart.y,
                        leftLineStart.z + (i * 2 + 1) * positionMultiplier
                        );
                //верхняя линия
                currentCellNumber = i;
                this.arrowForFreeCellSlot[currentCellNumber] = Instantiate(this.arrowForFreeCellSlotPrefab);
                this.arrowForFreeCellSlot[currentCellNumber].SetArrowForFreeCell();
                this.arrowForFreeCellSlot[currentCellNumber].TurnClockwise(1);
                this.arrowForFreeCellSlot[currentCellNumber].position = new Vector3
                        (
                        leftLineStart.x + (i * 2 + 1) * positionMultiplier,
                        leftLineStart.y,
                        leftLineStart.z + positionMultiplier * Field.FIELD_SIZE
                        );
            }
        }
        /// <summary>
        /// Заполнить игровое поле ячейками.
        /// </summary>
        private void FillFieldWithCell()
        {
            //Заполнение ячейками.
            Single positionMultiplier = this.sizeCellSlot + this.spacingBetweenCellSlot;
            for (Int32 i = 0; i < Field.FIELD_SIZE; i++)
                for (Int32 j = 0; j < Field.FIELD_SIZE; j++)
                {
                    this.slots[i, j] = Instantiate(this.cellSlotPrefab);
                    this.slots[i, j].SetCellType((CellType)UnityEngine.Random.Range(1, 4));
                    this.slots[i, j].position = new Vector3
                        (
                        i * positionMultiplier,
                        slots[i, j].position.y,
                        j * positionMultiplier
                        );
                    this.slots[i, j].transform.parent = this.slotForFieldSlots;
                }


        }


        private void Awake()
        {
            //Задать слотам размер
            this.cellSlotPrefab.transform.localScale = new Vector3(sizeCellSlot, sizeCellSlot, sizeCellSlot);
            this.arrowForFreeCellSlotPrefab.transform.localScale = new Vector3(sizeCellSlot, sizeCellSlot, sizeCellSlot);

            FillFieldWithCell();
            AddArrowSlotForFreeCell();
        }
    }
}
