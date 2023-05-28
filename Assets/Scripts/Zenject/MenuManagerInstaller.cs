using UnityEngine;

namespace Zenject.Installers
{
    public class MenuManagerInstaller : MonoInstaller
    {
        [SerializeField] private MenuManager menuManagerTemplate;
        public override void InstallBindings()
        {
            MenuManager instance = this.Container
                .InstantiatePrefabForComponent<MenuManager>(this.menuManagerTemplate);
            this.Container.Bind<MenuManager>()
                .FromInstance(instance)
                .AsSingle()
                .NonLazy();
        }
    }
}