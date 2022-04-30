using System;
using UnityEngine;

/// <summary>
/// Общий скрипт для слотов ячеек.
/// </summary>
public class CellSlotView : GameViewOriginScript
{
    /// <summary>
    /// Получить клон указанного по имени префаба из провайдер.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    protected GameObject GetPrefabClone(String name)
    {
        return GameManager.instance.prefabsProvider.GetPrefabClone(name);
    }
}
