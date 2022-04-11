using Assets.Scripts.GameModel.PlayingField;
using Assets.Scripts.GameModel.PlayingField.FieldCells;
using Assets.Scripts.GameView.Cells;
using System;
using UnityEngine;

namespace Assets.Scripts.GameView.FieldView
{
    public class FieldView: MonoBehaviour
    {
        [SerializeField]
        private CellSlotFill cellSlotPrefab = null;
        private CellSlotFill[,] slots = new CellSlotFill[Field.FIELD_SIZE, Field.FIELD_SIZE];
        private void FillField()
        {
            Vector3 size = this.cellSlotPrefab.size;
            for (Int32 i = 0; i < Field.FIELD_SIZE; i++)
                for (Int32 j = 0; j < Field.FIELD_SIZE; j++)
                {
                    this.slots[i, j] = Instantiate(this.cellSlotPrefab);
                    this.slots[i, j].SetCellType((CellType)UnityEngine.Random.Range(1, 4));
                    size = this.slots[i, j].size;
                    this.slots[i, j].position = new Vector3(i * (size.x + 1), slots[i, j].position.y, j * (size.z + 1));
                    this.slots[i, j].transform.parent = this.transform;
                }
        }
        private void Awake()
        {
            FillField();
        }
    }
}
