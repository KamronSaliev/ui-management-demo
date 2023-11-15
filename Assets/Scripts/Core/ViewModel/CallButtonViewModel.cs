using Cysharp.Threading.Tasks;
using UIManagementDemo.Core.View.Interfaces;
using UIManagementDemo.Core.ViewModel.Interfaces;
using UniRx;
using Utilities;
using Utilities.ExtensionMethods.RX;
using Zenject;

namespace UIManagementDemo.Core.ViewModel
{
    public class CallButtonViewModel : DisposableObject, IInitializable
    {
        public IReadOnlyReactiveProperty<string> ButtonName => _buttonName;
        public IReadOnlyReactiveProperty<bool> State => _state;
        public ReactiveCommand ClickCommand { get; } = new();
        
        private readonly ReactiveProperty<string> _buttonName = new();
        private readonly ReactiveProperty<bool> _state = new();

        private readonly string _name;
        private readonly TimerViewModel _timerViewModel;
        private readonly ITimerView _timerView;
        private readonly IShowHideButtonsContainer _showHideButtonsContainer;

        public CallButtonViewModel
        (
            string name,
            TimerViewModel timerViewModel,
            ITimerView timerView,
            IShowHideButtonsContainer showHideButtonsContainer
        )
        {
            _name = name;
            _timerViewModel = timerViewModel;
            _timerView = timerView;
            _showHideButtonsContainer = showHideButtonsContainer;
        }

        public void Initialize()
        {
            _buttonName.Value = _name;
            _buttonName.Subscribe(OnButtonNameChanged).AddTo(this);

            _timerViewModel.State.Subscribe(OnStateChanged).AddTo(this);
            
            ClickCommand.Subscribe(OnClick).AddTo(this);
        }

        private void OnClick(Unit unit)
        {
            _showHideButtonsContainer.Hide();
            _timerView.ShowHideTimer.Show().Forget();
            _timerView.ChangeViewModel(_timerViewModel);
            Logger.DebugLog(this, $"ChangeViewModel to {_timerViewModel.Id}");
        }

        private void OnButtonNameChanged(string buttonName)
        {
            _buttonName.Value = buttonName;
        }
        
        private void OnStateChanged(bool state)
        {
            _state.Value = state;
        }
    }
}