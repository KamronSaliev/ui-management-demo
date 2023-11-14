using UIManagementDemo.Core.Model;
using UIManagementDemo.Core.View;
using UniRx;
using Utilities;
using Utilities.ExtensionMethods.RX;
using Zenject;

namespace UIManagementDemo.Core.ViewModel
{
    public class CallButtonViewModel : DisposableObject, IInitializable
    {
        public ReactiveCommand ClickCommand { get; } = new();

        private readonly ReactiveProperty<string> _buttonText = new();
        public IReadOnlyReactiveProperty<string> ButtonText => _buttonText.ToReadOnlyReactiveProperty();

        private readonly int _id;
        private readonly TimerSpawnerView _timerSpawnerView;
        private readonly TimerView _timerView;
        private readonly CallButtonView _view;
        private readonly TimerModel _timerModel;
        private readonly CallButtonModel _model;

        public CallButtonViewModel
        (
            int id,
            TimerSpawnerView timerSpawnerView,
            TimerView timerView,
            CallButtonView view,
            TimerModel timerModel,
            CallButtonModel model
        )
        {
            _id = id;
            _timerSpawnerView = timerSpawnerView;
            _timerView = timerView;
            _view = view;
            _timerModel = timerModel;
            _model = model;

            _buttonText.Value = model.ButtonName;
        }

        public void Initialize()
        {
            Logger.DebugLog(this, "Initialize");

            ClickCommand
                .Subscribe(OnClick)
                .AddTo(this);

            if (_timerModel.State)
            {
                MakeActive();
            }
            else
            {
                MakeInactive();
            }
        }

        private void OnClick(Unit unit)
        {
            Logger.DebugLogWarning(this, $"OnClick {_model.ButtonName}");

            _timerSpawnerView.Hide();
            _timerView.Show();

            _timerView.ChangeViewModel(_timerSpawnerView.GetTimerViewModelById(_id)); // TODO: refactor
        }

        public void MakeActive()
        {
            Logger.DebugLogWarning(this, $"MakeActive {_model.ButtonName}");

            _view.ColorButton.ColorOnActive();
        }

        public void MakeInactive()
        {
            Logger.DebugLogWarning(this, $"MakeInactive {_model.ButtonName}");

            _view.ColorButton.ColorOnInactive();
        }
    }
}