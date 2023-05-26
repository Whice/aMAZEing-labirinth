using UnityEngine;
using Zenject;

public class MenuManagerInstaller : MonoInstaller
{
    [SerializeField] private MenuManager menuManagerTemplate;
    public override void InstallBindings()
    {
        MenuManager instance = Container
            .InstantiatePrefabForComponent<MenuManager>(this.menuManagerTemplate);
        Container.Bind<MenuManager>().FromInstance(instance).AsSingle().NonLazy();
    }
}