using Assets.Scripts.GameModel.PlayingField.FieldCells;
using System;
using UnityEngine;

namespace Assets.Scripts.GameView.Cells
{
    public class CellSlotFill : MonoBehaviour
    {
        public GameObject cellObject = null;

        public CellType cellType = CellType.unknown;
        public Vector3 size
        {
            set=>this.cellObject.transform.localScale = value;
            get => this.cellObject.transform.localScale;
        }
        public Vector3 position
        {
            get=> this.cellObject.transform.localPosition;
            set=> this.cellObject.transform.localPosition = value;
        }

        private GameObject GetPrefabClone(String name)
        {
            return GameManager.instance.prefabsProvider.GetPrefabClone(name);
        }
        public void SetCellType(CellType type)
        {
            this.cellType = type;

            switch (type)
            {
                case CellType.corner:
                    {
                        this.cellObject = GetPrefabClone("CornerCell");
                        break;
                    }
                case CellType.line:
                    {
                        this.cellObject = GetPrefabClone("LineCell");
                        break;
                    }
                case CellType.threeDirection:
                    {
                        this.cellObject = GetPrefabClone("ThreeDirectionCell");
                        break;
                    }
            }
        }
    }
}
