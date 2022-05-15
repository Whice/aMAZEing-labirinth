using System;
using System.Collections.Generic;
using UnityEngine;


namespace Assets.Scripts.GameView
{
    [Serializable]
    public class GameObjectProvider : ScriptableObject
    {
        #region Объекты в провайдере.

        [SerializeField]
        protected GameObject[] resourcesList = new GameObject[0];
        private Dictionary<String, GameObject> resourcesDictionaryField = null;
        protected Dictionary<String, GameObject> resourcesDictionary
        {
            get
            {
                if (this.resourcesDictionaryField == null)
                {
                    this.resourcesDictionaryField = new Dictionary<string, GameObject>(this.resourcesList.Length);
                    foreach (GameObject obj in this.resourcesList)
                    {
                        this.resourcesDictionaryField.Add(obj.name, obj);
                    }
                }
                return this.resourcesDictionaryField;
            }
        }

        #endregion Объекты в провайдере.

        #region Действия.


        /// <summary>
        /// Получить клон префаба с указанным именем.
        /// </summary>
        /// <param name="prefabName"></param>
        /// <returns></returns>
        public GameObject GetPrefabClone(String prefabName)
        {
            if (this.resourcesDictionary.ContainsKey(prefabName))
            {
                return Instantiate(this.resourcesDictionary[prefabName]);
            }
            else
            {
                Debug.Log("In " + nameof(this.name) + " not found prefab: " + prefabName + "!");
                return null;
            }
        }

        #endregion Действия.
    }
}
