using System;
using System.Collections.Generic;
using UnityEngine;

public class GraphicPrefabsProvider : MonoBehaviour
{
    #region ������ ����������.

    public String name
    {
        get => this.gameObject.name;
    }

    #endregion ������ ����������.

    #region ������� � ����������.

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

    #endregion ������� � ����������.

    #region ��������.

    public GameObject GetPrefabClone(String prefabName)
    {
        return Instantiate(this.prefabsDictionary[prefabName]);
    }

    #endregion ��������.
}
