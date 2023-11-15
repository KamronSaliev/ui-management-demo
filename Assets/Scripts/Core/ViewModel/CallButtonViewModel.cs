using Cysharp.Threading.Tasks;
using UIManagementDemo.Core.Model;
using UIManagementDemo.Core.View.Interfaces;
using UIManagementDemo.Core.ViewModel.Interfaces;
using UniRx;
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

        private readonly CallButtonOptions _callButtonOptions;
        private readonly ITimerView _timerView;
        private readonly IShowHideButtonsContainer _showHideButtonsContainer;

        public CallButtonViewModel
        (
            CallButtonOptions callButtonOptions,
            ITimerView timerView,
            IShowHideButtonsContainer showHideButtonsContainer
        )
        {
            _callButtonOptions = callButtonOptions;
            _timerView = timerView;
            _showHideButtonsContainer = showHideButtonsContainer;
        }

        public void Initialize()
        {
            _buttonName.Value = _callButtonOptions.Name;
            _buttonName.Subscribe(OnButtonNameChanged).AddTo(this);

            _callButtonOptions.TimerViewModel.State.Subscribe(OnStateChanged).AddTo(this);

            ClickCommand.Subscribe(OnClick).AddTo(this);
        }

        private void OnClick(Unit unit)
        {
            _showHideButtonsContainer.Hide();
            _timerView.ShowHideTimer.Show().Forget();
            _timerView.ChangeViewModel(_callButtonOptions.TimerViewModel);
        }

        private void OnButtonNameChanged(string buttonName)
        {
            _buttonName.Value = buttonName;
        }

        private void OnStateChanged(bool state)
        {
            _state.Value = state;
        }

        public class Factory : PlaceholderFactory<CallButtonOptions, CallButtonViewModel>
        {
        }
    }
}