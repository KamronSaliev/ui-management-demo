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

        private SpawnButtonViewModel _spawnButtonViewModel;

        [Inject]
        public void Construct
        (
            SpawnButtonViewModel spawnButtonViewModel
        )
        {
            _spawnButtonViewModel = spawnButtonViewModel;
        }

        private void Start()
        {
            _spawnButtonView.BindTo(_spawnButtonViewModel).AddTo(this);
        }
    }
}