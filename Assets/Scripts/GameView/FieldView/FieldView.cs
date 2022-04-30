using Assets.Scripts.GameModel.PlayingField;
using Assets.Scripts.GameModel.PlayingField.FieldCells;
using Assets.Scripts.GameView.Cells;
using System;
using UnityEngine;

namespace Assets.Scripts.GameView.FieldView
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
        private Vector3 sizeCellSlot;
        /// <summary>
        /// Все слоты поля.
        /// </summary>
        private CellSlotFill[,] slots = new CellSlotFill[Field.FIELD_SIZE, Field.FIELD_SIZE];
        /// <summary>
        /// Добавить слоты для стрелочек - мест, куда будет помещена свободная ячейка.
        /// </summary>
        private void AddArrowSlotForFreeCell()
        {
            Vector3 size = this.sizeCellSlot;
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
            Vector3 size = this.sizeCellSlot;
            for (Int32 i = 0; i < Field.FIELD_SIZE; i++)
                for (Int32 j = 0; j < Field.FIELD_SIZE; j++)
                {
                    this.slots[i, j] = Instantiate(this.cellSlotPrefab);
                    this.slots[i, j].SetCellType((CellType)UnityEngine.Random.Range(1, 4));
                    this.slots[i, j].position = new Vector3(i * (size.x + 1), slots[i, j].position.y, j * (size.z + 1));
                    this.slots[i, j].transform.parent = this.transform;
                }


        }
        private void Awake()
        {
            //Задать слоту ячейку и потом считать из нее размер, т.к. размер слота зависит от размера ячейки.
            {
                CellSlotFill fillingSlot = Instantiate(this.cellSlotPrefab);
                fillingSlot.SetCellType(CellType.line);
                this.sizeCellSlot = fillingSlot.size;
            }

            FillFieldWithCell();
            AddArrowSlotForFreeCell();
        }
    }
}
