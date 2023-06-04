using UnityEngine;

namespace Zenject.Installers
{
    public class GameManagerInstaller : MonoInstaller
    {
        [SerializeField] private GameManager gameManagerTemplate = null;
        public override void InstallBindings()
        {
            GameManager instance = this.Container
                .InstantiatePrefabForComponent<GameManager>(this.gameManagerTemplate);

            this.Container.Bind<GameManager>()
                .FromInstance(instance)
                .AsSingle()
                .NonLazy();
        }
    }
}