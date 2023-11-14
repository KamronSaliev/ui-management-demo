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
        public ReactiveCommand BackClickCommand { get; } = new();
        public ReactiveCommand IncreaseCommand { get; } = new();
        public ReactiveCommand DecreaseCommand { get; } = new();
        public ReactiveCommand StopClickCommand { get; } = new();
        public ReactiveCommand ResetClickCommand { get; } = new();

        private readonly ReactiveProperty<int> _time = new();
        public IReadOnlyReactiveProperty<int> Time => _time.ToReadOnlyReactiveProperty();

        private readonly int _id;
        private readonly TimerSpawnerView _timerSpawnerView;
        private readonly TimerCallButtonViewModel _timerCallButtonViewModel;
        private readonly ITimerView _timerView;
        private readonly TimerModel _model;

        private const int DefaultTimeSpan = 1;

        public TimerViewModel
        (
            int id,
            TimerSpawnerView timerSpawnerView,
            TimerCallButtonViewModel timerCallButtonViewModel,
            ITimerView timerView,
            TimerModel model
        )
        {
            _id = id;
            _timerSpawnerView = timerSpawnerView;
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

            BackClickCommand
                .Subscribe(OnBackClick)
                .AddTo(this);

            IncreaseCommand
                .Subscribe(OnIncrease)
                .AddTo(this);

            DecreaseCommand
                .Subscribe(OnDecrease)
                .AddTo(this);

            StopClickCommand
                .Subscribe(OnStopClick)
                .AddTo(this);

            ResetClickCommand
                .Subscribe(OnResetClick)
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
            _timerSpawnerView.Show();
            _timerCallButtonViewModel.MakeActive();
        }

        private void OnBackClick(Unit unit)
        {
            Logger.DebugLog(this, $"Back Timer{_id}");

            _timerView.Hide();
            _timerSpawnerView.Show();
        }

        private void OnIncrease(Unit unit)
        {
            Logger.DebugLog(this, $"Increase Called for Timer{_id}");

            if (!CanIncrease())
            {
                return;
            }

            _model.UpdateTime(DefaultTimeSpan);
            _time.Value = _model.Time;

            Logger.DebugLog(this, $"Increased Timer{_id} {_model.Time}");
        }

        private void OnDecrease(Unit unit)
        {
            Logger.DebugLog(this, $"Decrease Called for Timer{_id}");

            if (!CanDecrease())
            {
                return;
            }

            _model.UpdateTime(-DefaultTimeSpan);
            _time.Value = _model.Time;

            Logger.DebugLog(this, $"Decreased Timer{_id} {_model.Time}");
        }

        public bool CanIncrease()
        {
            return _model.Time < int.MaxValue;
        }

        public bool CanDecrease()
        {
            return _model.Time > int.MinValue;
        }

        private void OnStopClick(Unit unit)
        {
            Logger.DebugLog(this, $"Stop Timer{_id}");

            _model.UpdateState(false);
            _timerCallButtonViewModel.MakeInactive();
        }

        private void OnResetClick(Unit unit)
        {
            Logger.DebugLog(this, $"Reset Timer{_id}");

            _model.UpdateState(false);
            _model.ResetTime();
            _timerCallButtonViewModel.MakeInactive();
            _time.Value = _model.Time;
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