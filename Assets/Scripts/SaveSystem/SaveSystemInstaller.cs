using Zenject;

namespace UIManagementDemo.SaveSystem
{
    public class SaveSystemInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<PlayerPrefsSaveSystem>().AsSingle();
        }
    }
}