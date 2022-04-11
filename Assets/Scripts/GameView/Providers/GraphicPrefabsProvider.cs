using System;
using System.Collections.Generic;
using UnityEngine;

public class GraphicPrefabsProvider : MonoBehaviour
{
    #region Данные провайдера.

    public String name
    {
        get => this.gameObject.name;
    }

    #endregion Данные провайдера.

    #region Объекты в провайдере.

    [SerializeField]
    private GameObject[] prefabsList = new GameObject[0];
    private Dictionary<String, GameObject> prefabsDictionaryField= null;
    private Dictionary<String, GameObject> prefabsDictionary
    {
        get
        {
            if (this.prefabsDictionaryField == null)
            {
                this.prefabsDictionaryField = new Dictionary<string, GameObject>(this.prefabsList.Length);
                foreach (GameObject obj in this.prefabsList)
                {
                    this.prefabsDictionaryField.Add(obj.name, obj);
                }
            }
            return this.prefabsDictionaryField;
        }
    }

    #endregion Объекты в провайдере.

    #region Действия.

    public GameObject GetPrefabClone(String prefabName)
    {
        return Instantiate(this.prefabsDictionary[prefabName]);
    }

    #endregion Действия.
}
