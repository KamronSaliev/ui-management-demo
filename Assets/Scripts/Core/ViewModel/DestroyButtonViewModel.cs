using UniRx;
using Utilities.ExtensionMethods.RX;
using Zenject;

namespace UIManagementDemo.Core.ViewModel
{
    public class DestroyButtonViewModel : DisposableObject, IInitializable
    {
        public ReactiveCommand ClickCommand { get; } = new();

        private readonly ICoreTimerSpawner _coreTimerSpawner;

        public DestroyButtonViewModel(ICoreTimerSpawner coreTimerSpawner)
        {
            _coreTimerSpawner = coreTimerSpawner;
        }

        public void Initialize()
        {
            ClickCommand.Subscribe(OnClick).AddTo(this);
        }

        private void OnClick(Unit unit)
        {
            _coreTimerSpawner.Destroy();
        }
    }
}