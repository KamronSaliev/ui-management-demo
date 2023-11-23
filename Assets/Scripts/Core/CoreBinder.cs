using UIManagementDemo.Core.View;
using UIManagementDemo.Core.ViewModel;
using UniRx;
using UnityEngine;
using Zenject;

namespace UIManagementDemo.Core
{
    public class CoreBinder : MonoBehaviour
    {
        [SerializeField] private SpawnButtonView _spawnButtonView;
        [SerializeField] private DestroyButtonView _destroyButtonView;

        private SpawnButtonViewModel _spawnButtonViewModel;
        private DestroyButtonViewModel _destroyButtonViewModel;

        [Inject]
        public void Construct
        (
            SpawnButtonViewModel spawnButtonViewModel,
            DestroyButtonViewModel destroyButtonViewModel
        )
        {
            _spawnButtonViewModel = spawnButtonViewModel;
            _destroyButtonViewModel = destroyButtonViewModel;
        }

        private void Start()
        {
            _spawnButtonView.BindTo(_spawnButtonViewModel).AddTo(this);
            _destroyButtonView.BindTo(_destroyButtonViewModel).AddTo(this);
        }
    }
}