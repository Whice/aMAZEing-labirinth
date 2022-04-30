using System;
using UnityEngine;

/// <summary>
/// ����� ������ ��� ������ �����.
/// </summary>
public class CellSlotView : GameViewOriginScript
{
    /// <summary>
    /// �������� ���� ���������� �� ����� ������� �� ���������.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    protected GameObject GetPrefabClone(String name)
    {
        return GameManager.instance.prefabsProvider.GetPrefabClone(name);
    }
}
