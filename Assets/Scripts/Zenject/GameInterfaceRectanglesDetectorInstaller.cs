using UI;
using UnityEngine;

namespace Zenject.Installers
{
    public class GameInterfaceRectanglesDetectorInstaller : MonoInstaller
    {
        [SerializeField] private GameInterfaceRectanglesDetector gameInterfaceRectanglesDetectorTemplate;
        public override void InstallBindings()
        {
            GameInterfaceRectanglesDetector instance = this.Container
                .InstantiatePrefabForComponent<GameInterfaceRectanglesDetector>(this.gameInterfaceRectanglesDetectorTemplate);

            this.Container.Bind<GameInterfaceRectanglesDetector>()
                .FromInstance(instance)
                .AsSingle()
                .NonLazy();
        }
    }
}