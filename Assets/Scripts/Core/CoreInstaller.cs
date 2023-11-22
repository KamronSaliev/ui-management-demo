using UIManagementDemo.Core.Config;
using UIManagementDemo.Core.Model;
using UIManagementDemo.Core.Mono;
using UIManagementDemo.Core.View;
using UIManagementDemo.Core.ViewModel;
using UIManagementDemo.SaveSystem;
using UnityEngine;
using Zenject;

namespace UIManagementDemo.Core
{
    public class CoreInstaller : MonoInstaller
    {
        [SerializeField] private Transform _callButtonViewsContainer;
        [SerializeField] private CallButtonView _callButtonPrefab;
        [SerializeField] private ControlButtonsShowHideProvider _controlButtonsShowHideProvider;
        [SerializeField] private SpawnButtonView _spawnButtonView;
        [SerializeField] private DestroyButtonView _destroyButtonView;
        [SerializeField] private TimerView _timerView;
        [SerializeField] private ShowHideButtonsContainerConfig _showHideButtonsContainerConfig;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<CoreTimerSpawner>().AsSingle();

            Container.BindInterfacesAndSelfTo<ControlButtonsShowHideProvider>().FromInstance(_controlButtonsShowHideProvider).AsSingle();

            Container.BindInterfacesAndSelfTo<SpawnButtonViewModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<SpawnButtonView>().FromInstance(_spawnButtonView).AsSingle();
            
            Container.BindInterfacesAndSelfTo<DestroyButtonViewModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<DestroyButtonView>().FromInstance(_destroyButtonView).AsSingle();

            Container.BindInterfacesAndSelfTo<TimerView>().FromInstance(_timerView).AsSingle();

            Container.BindInterfacesAndSelfTo<ShowHideButtonsContainer>().AsSingle();
            Container.BindInstance(_showHideButtonsContainerConfig).AsSingle();
            
            Container.BindFactory<CallButtonView, CallButtonView.Factory>()
                .FromComponentInNewPrefab(_callButtonPrefab)
                .UnderTransform(_callButtonViewsContainer);

            Container.BindFactory<CallButtonOptions, CallButtonViewModel, CallButtonViewModel.Factory>();
            Container.BindFactory<TimerData, TimerViewModel, TimerViewModel.Factory>();
        }
    }
}