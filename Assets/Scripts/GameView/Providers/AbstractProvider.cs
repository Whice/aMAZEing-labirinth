using System;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.GameView
{
    [Serializable]
    public abstract class AbstractProvider : ScriptableObject
    {
        public DiContainer diContainer;
        protected GameObject InstantiateWithInject(GameObject objectTemplate)
        {
            return diContainer.InstantiatePrefab(objectTemplate);
        }
    }
}