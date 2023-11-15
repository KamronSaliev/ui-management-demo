using System;
using UIManagementDemo.Core.ViewModel.Interfaces;
using UIManagementDemo.SaveSystem;
using UniRx;
using UnityEngine;
using Utilities.ExtensionMethods;
using Utilities.ExtensionMethods.RX;
using Zenject;
using Logger = Utilities.Logger;

namespace UIManagementDemo.Core.ViewModel
{
    public class TimerViewModel : DisposableObject, IInitializable
    {
        public int Id => _timerData.Id;
        public IReadOnlyReactiveProperty<int> Time => _time;
        public IReadOnlyReactiveProperty<bool> State => _state;
        public ReactiveCommand StartClickCommand { get; } = new();
        public ReactiveCommand BackClickCommand { get; } = new();
        public ReactiveCommand StopClickCommand { get; } = new();
        public ReactiveCommand ResetClickCommand { get; } = new();
        public ReactiveCommand IncreaseCommand { get; } = new();
        public ReactiveCommand DecreaseCommand { get; } = new();

        private readonly ReactiveProperty<int> _time = new();
        private readonly ReactiveProperty<bool> _state = new();

        private readonly TimerData _timerData;
        private readonly IShowHideButtonsContainer _showHideButtonsContainer;

        private const int DefaultTimeSpan = 1;

        public TimerViewModel
        (
            TimerData timerData,
            IShowHideButtonsContainer showHideButtonsContainer
        )
        {
            _timerData = timerData;
            _showHideButtonsContainer = showHideButtonsContainer;
        }

        public void Initialize()
        {
            _time.Value = _timerData.Time;
            _state.Value = _timerData.State;

            StartClickCommand.Subscribe(OnStartButtonClicked).AddTo(this);
            BackClickCommand.Subscribe(OnBackButtonClicked).AddTo(this);
            StopClickCommand.Subscribe(OnStopButtonClicked).AddTo(this);
            ResetClickCommand.Subscribe(OnResetButtonClicked).AddTo(this);
            IncreaseCommand.Subscribe(OnIncreaseButtonClicked).AddTo(this);
            DecreaseCommand.Subscribe(OnDecreaseButtonClicked).AddTo(this);

            Observable.Interval(TimeSpan.FromSeconds(DefaultTimeSpan))
                .Subscribe(_ => { OnEveryTimerTick(); })
                .AddTo(this);
        }

        private void OnStartButtonClicked(Unit unit)
        {
            _state.Value = true;
            _showHideButtonsContainer.Show();

            Logger.DebugLog(this, $"Timer {Id} Started: {TimeSpan.FromSeconds(_time.Value).ToHoursMinutesSeconds()}");
        }

        private void OnBackButtonClicked(Unit unit)
        {
            _showHideButtonsContainer.Show();
        }

        private void OnStopButtonClicked(Unit unit)
        {
            ChangeState(false);
        }

        private void OnResetButtonClicked(Unit unit)
        {
            ChangeState(false);
            ResetTime();
        }

        private void OnIncreaseButtonClicked(Unit unit)
        {
            if (!CanIncrease())
            {
                return;
            }

            ChangeTimeByDelta(DefaultTimeSpan);
        }

        private void OnDecreaseButtonClicked(Unit unit)
        {
            if (!CanDecrease())
            {
                return;
            }

            ChangeTimeByDelta(-DefaultTimeSpan);
        }

        public bool CanIncrease()
        {
            return _time.Value < int.MaxValue;
        }

        public bool CanDecrease()
        {
            return _time.Value > int.MinValue;
        }

        private void OnEveryTimerTick()
        {
            if (!_state.Value)
            {
                return;
            }

            ChangeTimeByDelta(-DefaultTimeSpan);

            if (_time.Value != 0)
            {
                return;
            }

            ChangeState(false);
            
            Logger.DebugLog(this, $"Timer {Id} Expired");
        }

        private void ChangeTimeByDelta(int delta)
        {
            var newValue = Mathf.Max(0.0f, _time.Value + delta);
            _time.Value = (int)newValue;
        }

        private void ChangeState(bool state)
        {
            _state.Value = state;
            
            Logger.DebugLog(this, $"Timer {Id} State: {state}");
        }

        private void ResetTime()
        {
            _time.Value = 0;
        }
    }
}