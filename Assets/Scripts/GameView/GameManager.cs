using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField]
    private GraphicPrefabsProvider prefabsProviderPrivate;
    public GraphicPrefabsProvider prefabsProvider
    {
        get=>this.prefabsProviderPrivate;
    }
}
