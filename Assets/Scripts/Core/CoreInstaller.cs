using UIManagementDemo.Core.Config;
using UIManagementDemo.Core.View;
using UIManagementDemo.Core.ViewModel;
using UnityEngine;
using Zenject;

namespace UIManagementDemo.Core
{
    public class CoreInstaller : MonoInstaller
    {
        [SerializeField] private TimerSpawnerView _timerButtonSpawnerView;
        [SerializeField] private TimerCallButtonView _timerCallButtonPrefab;
        [SerializeField] private TimerView _timerView;
        [SerializeField] private ShowHideButtonsContainerConfig _showHideButtonsContainerConfig;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<TimerSpawnerViewModel>()
                .AsSingle();

            Container.BindInterfacesAndSelfTo<TimerSpawnerView>()
                .FromInstance(_timerButtonSpawnerView)
                .AsSingle();

            Container.BindInterfacesAndSelfTo<TimerView>()
                .FromInstance(_timerView)
                .AsSingle();

            Container.BindFactory<TimerCallButtonView, TimerCallButtonView.Factory>()
                .FromComponentInNewPrefab(_timerCallButtonPrefab)
                .UnderTransform(_timerButtonSpawnerView.transform);

            Container.BindInstance(_showHideButtonsContainerConfig).AsSingle();
            Container.BindInterfacesAndSelfTo<ShowHideButtonsContainer>().AsSingle();
        }
    }
}