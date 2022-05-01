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
        /// Слот, куда будет помещеная ячейка.
        /// </summary>
        [SerializeField]
        private CellSlotFill cellSlotPrefab = null;
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
        /// Добавить слоты для стрелочек - мест, куда будет помещена свободная ячейка.
        /// </summary>
        private void AddArrowSlotForFreeCell()
        {
            Single size = this.sizeCellSlot;
            for (Int32 i=1;i<Field.FIELD_SIZE;i++)
            {

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
                        i * (positionMultiplier),
                        slots[i, j].position.y,
                        j * (positionMultiplier)
                        );
                    this.slots[i, j].transform.parent = this.transform;
                }


        }
        private void Awake()
        {
            //Задать слотам размер
            this.cellSlotPrefab.transform.localScale = new Vector3(sizeCellSlot, sizeCellSlot, sizeCellSlot);

            FillFieldWithCell();
            AddArrowSlotForFreeCell();
        }
    }
}
