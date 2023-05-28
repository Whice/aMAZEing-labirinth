using UnityEngine;
using Zenject;

public class InjectableObject : MonoBehaviour
{
    [Inject] private DiContainer diContainer;
    protected T InstantiateWithInject<T>(T objectTemplate) where T : UnityEngine.Object
    {
        return diContainer.InstantiatePrefabForComponent<T>(objectTemplate);
    }
    protected T InstantiateWithInject<T>(T objectTemplate, Transform parent) where T : UnityEngine.Object
    {
        return diContainer.InstantiatePrefabForComponent<T>(objectTemplate, parent);
    }
}
