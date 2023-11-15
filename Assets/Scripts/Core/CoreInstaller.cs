using UIManagementDemo.Core.Config;
using UIManagementDemo.Core.View;
using UIManagementDemo.Core.ViewModel;
using UnityEngine;
using Zenject;

namespace UIManagementDemo.Core
{
    public class CoreInstaller : MonoInstaller
    {
        [SerializeField] private Transform _callButtonViewsContainer;
        [SerializeField] private CallButtonView _callButtonPrefab;
        [SerializeField] private SpawnButtonView _spawnButtonView;
        [SerializeField] private TimerView _timerView;
        [SerializeField] private ShowHideButtonsContainerConfig _showHideButtonsContainerConfig;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<CoreTimerSpawner>().AsSingle();

            Container.BindInterfacesAndSelfTo<SpawnButtonViewModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<SpawnButtonView>().FromInstance(_spawnButtonView).AsSingle();

            Container.BindInterfacesAndSelfTo<TimerView>().FromInstance(_timerView).AsSingle();

            Container.BindInterfacesAndSelfTo<ShowHideButtonsContainer>().AsSingle();
            Container.BindInstance(_showHideButtonsContainerConfig).AsSingle();
            
            Container.BindFactory<CallButtonView, CallButtonView.Factory>()
                .FromComponentInNewPrefab(_callButtonPrefab)
                .UnderTransform(_callButtonViewsContainer);
        }
    }
}