using UIManagementDemo.Core.View;
using UIManagementDemo.Core.ViewModel;
using UniRx;
using UnityEngine;
using Zenject;
using Logger = Utilities.Logger;

namespace UIManagementDemo.Core
{
    public class CoreBinder : MonoBehaviour
    {
        [SerializeField] private TimerSpawnerView _timerSpawnerView;

        private TimerSpawnerViewModel _timerSpawnerViewModel;

        [Inject]
        public void Construct
        (
            TimerSpawnerViewModel timerSpawnerViewModel
        )
        {
            _timerSpawnerViewModel = timerSpawnerViewModel;
        }

        private void Start()
        {
            _timerSpawnerView.BindTo(_timerSpawnerViewModel).AddTo(this);
        }
    }
}