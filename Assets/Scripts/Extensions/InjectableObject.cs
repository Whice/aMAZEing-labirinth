using Zenject;

namespace UnityEngine
{
    /// <summary>
    /// Объект для инстанцирования со специальными методами
    /// для zenject.
    /// </summary>
    public class InjectableObject : MonoBehaviour
    {
        [Inject] private DiContainer diContainer;
        /// <summary>
        /// Создать объект с 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objectTemplate"></param>
        /// <returns></returns>
        protected T InstantiateWithInject<T>(T objectTemplate) where T : Object
        {
            return diContainer.InstantiatePrefabForComponent<T>(objectTemplate);
        }
        protected T InstantiateWithInject<T>(T objectTemplate, Transform parent) where T : Object
        {
            return diContainer.InstantiatePrefabForComponent<T>(objectTemplate, parent);
        }
    }
}