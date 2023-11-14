using System;
using UIManagementDemo.Core.Model;
using UIManagementDemo.Core.View;
using UniRx;
using Utilities.ExtensionMethods.RX;
using Zenject;
using Logger = Utilities.Logger;

namespace UIManagementDemo.Core.ViewModel
{
    public class TimerViewModel : DisposableObject, IInitializable
    {
        public ReactiveCommand StartClickCommand { get; } = new();
        public ReactiveCommand IncreaseClickCommand { get; } = new();
        public ReactiveCommand DecreaseClickCommand { get; } = new();
        public ReactiveCommand BackClickCommand { get; } = new();

        private readonly ReactiveProperty<int> _time = new();
        public IReadOnlyReactiveProperty<int> Time => _time.ToReadOnlyReactiveProperty();

        private readonly int _id;
        private readonly TimerCallButtonViewModel _timerCallButtonViewModel;
        private readonly ITimerView _timerView;
        private readonly TimerModel _model;

        private const int DefaultTimeSpan = 1;

        public TimerViewModel
        (
            int id,
            TimerCallButtonViewModel timerCallButtonViewModel,
            ITimerView timerView,
            TimerModel model
        )
        {
            _id = id;
            _timerCallButtonViewModel = timerCallButtonViewModel;
            _timerView = timerView;
            _model = model;
        }

        public void Initialize()
        {
            Logger.DebugLog(this, $"Initialize Timer{_id}");

            StartClickCommand
                .Subscribe(OnStartClick)
                .AddTo(this);

            IncreaseClickCommand
                .Subscribe(OnIncreaseClick)
                .AddTo(this);

            DecreaseClickCommand
                .Subscribe(OnDecreaseClick)
                .AddTo(this);

            BackClickCommand
                .Subscribe(OnBackClick)
                .AddTo(this);

            Observable.Interval(TimeSpan.FromSeconds(DefaultTimeSpan))
                .Subscribe(_ => { OnEveryTimerTick(); })
                .AddTo(this);

            _time.Value = _model.Time;
        }

        private void OnStartClick(Unit unit)
        {
            Logger.DebugLogWarning(this, $"Start Timer{_id}");

            if (_model.Time == 0)
            {
                Logger.DebugLogError(this, "Cannot start with Time: 00:00:00");
                return;
            }

            _model.UpdateState(true);
            _timerView.Hide();
            _timerCallButtonViewModel.MakeActive();
        }

        private void OnIncreaseClick(Unit unit)
        {
            _model.UpdateTime(DefaultTimeSpan);
            _time.Value = _model.Time;

            Logger.DebugLog(this, $"Increased Timer{_id} {_model.Time}");
        }

        private void OnDecreaseClick(Unit unit)
        {
            _model.UpdateTime(-DefaultTimeSpan);
            _time.Value = _model.Time;

            Logger.DebugLog(this, $"Decreased Timer{_id} {_model.Time}");
        }

        private void OnBackClick(Unit unit)
        {
            Logger.DebugLog(this, $"Back Timer{_id}");

            _timerView.Hide();
        }

        private void OnEveryTimerTick()
        {
            if (!_model.State)
            {
                return;
            }

            _model.UpdateTime(-DefaultTimeSpan);
            _time.Value = _model.Time;

            if (_model.Time == 0)
            {
                Logger.DebugLogWarning(this, $"Timer{_id} Expired");
                _model.UpdateState(false);
                _timerCallButtonViewModel.MakeInactive();
            }
        }
    }
}