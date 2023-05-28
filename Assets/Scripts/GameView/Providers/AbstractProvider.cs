using System;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.GameView
{
    [Serializable]
    public abstract class AbstractProvider : ScriptableObject
    {
        [Inject] private DiContainer diContainer;
        protected T InstantiateWithInject<T>(T objectTemplate) where T : UnityEngine.Object
        {
            return diContainer.InstantiatePrefabForComponent<T>(objectTemplate);
        }
    }
}