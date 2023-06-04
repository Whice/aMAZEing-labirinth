using Settings;
using UnityEngine;

namespace Zenject.Installers
{
    public class GeneralSettingsInstaller : MonoInstaller
    {
        [SerializeField] private GeneralSettings generalSettingsTemplate;
        public override void InstallBindings()
        {
            GeneralSettings instance = this.Container
                .InstantiatePrefabForComponent<GeneralSettings>(this.generalSettingsTemplate);

            this.Container.Bind<GeneralSettings>()
                .FromInstance(instance)
                .AsSingle()
                .NonLazy();
        }
    }
}