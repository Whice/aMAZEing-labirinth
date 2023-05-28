using System;
using System.Collections.Generic;
using UnityEngine;


namespace Assets.Scripts.GameView
{
    [Serializable]
    [CreateAssetMenu(fileName = "Treasure Provider", menuName = "Game view/Treasure Provider")]
    public class TreasureProvider : AbstractProvider
    {
        [Serializable]
        private class ObjectWithID
        {
#pragma warning disable CS0649

            public GameObject treasure;
            public Int32 ID;

#pragma warning restore CS0649
        }

        #region Объекты в провайдере.

        [SerializeField]
        private ObjectWithID[] resourcesList = new ObjectWithID[0];
        private Dictionary<Int32, GameObject> resourcesDictionaryField = null;
        private Dictionary<Int32, GameObject> resourcesDictionary
        {
            get
            {
                if (this.resourcesDictionaryField == null)
                {
                    this.resourcesDictionaryField = new Dictionary<Int32, GameObject>(this.resourcesList.Length);
                    foreach (ObjectWithID obj in this.resourcesList)
                    {
                        this.resourcesDictionaryField.Add(obj.ID, obj.treasure);
                    }
                }
                return this.resourcesDictionaryField;
            }
        }

        #endregion Объекты в провайдере.

        #region Действия.


        /// <summary>
        /// Получить клон сокровища с указанным id.
        /// </summary>
        /// <param name="treasureID"></param>
        /// <returns></returns>
        public GameObject GetPrefabClone(Int32 treasureID)
        {
            if (this.resourcesDictionary.ContainsKey(treasureID))
            {
                return InstantiateWithInject(this.resourcesDictionary[treasureID]);
            }
            else
            {
                Debug.LogError("In " + nameof(TreasureProvider) + " not found treasure with ID: " + treasureID + "!");
                return null;
            }
        }

        #endregion Действия.
    }
}
