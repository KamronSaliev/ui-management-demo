using System;
using Cysharp.Threading.Tasks;
using TMPro;
using UIManagementDemo.Core.Config;
using UIManagementDemo.Core.Mono;
using UIManagementDemo.Core.View.Interfaces;
using UIManagementDemo.Core.ViewModel;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;
using Utilities.ExtensionMethods;
using Utilities.ExtensionMethods.RX;
using Logger = Utilities.Logger;

namespace UIManagementDemo.Core.View
{
    public class TimerView : BindableView<TimerViewModel>, ITimerView
    {
        public ShowHideTimer ShowHideTimer => _showHideTimer;

        [SerializeField] private TMP_Text _timerText;
        [SerializeField] private TMP_Text _labelText;
        [SerializeField] private TMP_Text _startButtonText;
        [SerializeField] private Button _startButton;
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _resetButton;
        [SerializeField] private Button _increaseButton;
        [SerializeField] private Button _decreaseButton;

        [SerializeField] private ShowHideTimer _showHideTimer;
        [SerializeField] private TimerControlConfig _timerControlConfig;

        private readonly CompositeDisposable _increaseButtonSubscriptions = new();
        private readonly CompositeDisposable _decreaseButtonSubscriptions = new();

        private const string StartButtonTextDefault = "start";
        private const string StartButtonTextOnPause = "pause";

        private bool _isPointerDownActive;

        protected override void OnBind(CompositeDisposable disposables)
        {
            RemoveTriggerSubscriptions();

            ViewModel.Name.Subscribe(OnNameChanged).AddTo(disposables);
            ViewModel.Time.Subscribe(OnTimeChanged).AddTo(disposables);
            ViewModel.State.Subscribe(OnStateChanged).AddTo(disposables);

            _startButton.OnClickAsObservable().Subscribe(OnStartButtonClicked).AddTo(disposables);
            _backButton.OnClickAsObservable().Subscribe(OnBackButtonClicked).AddTo(disposables);
            _resetButton.OnClickAsObservable().Subscribe(OnResetButtonClicked).AddTo(disposables);

            _increaseButtonSubscriptions.Add(_increaseButton.OnPointerUpAsObservable()
                .Subscribe(_ => OnButtonPointerUp())
                .AddTo(this));
            _increaseButtonSubscriptions.Add(_increaseButton.OnPointerClickAsObservable()
                .Subscribe(_ => OnButtonPointerClick(ViewModel.IncreaseCommand))
                .AddTo(disposables));
            _increaseButtonSubscriptions.Add(_increaseButton.OnLongPointerDownAsObservable()
                .Subscribe(_ => OnButtonPointerDown(ViewModel.CanIncrease, ViewModel.IncreaseCommand))
                .AddTo(this));

            _decreaseButtonSubscriptions.Add(_decreaseButton.OnPointerUpAsObservable()
                .Subscribe(_ => OnButtonPointerUp())
                .AddTo(this));
            _decreaseButtonSubscriptions.Add(_decreaseButton.OnPointerClickAsObservable()
                .Subscribe(_ => OnButtonPointerClick(ViewModel.DecreaseCommand))
                .AddTo(disposables));
            _decreaseButtonSubscriptions.Add(_decreaseButton.OnLongPointerDownAsObservable()
                .Subscribe(_ => OnButtonPointerDown(ViewModel.CanDecrease, ViewModel.DecreaseCommand))
                .AddTo(this));
        }

        public void ChangeViewModel(TimerViewModel newViewModel)
        {
            UnbindViewModel();
            BindTo(newViewModel);
        }
        
        private void OnNameChanged(string newName)
        {
            _labelText.text = newName;
        }

        private void OnTimeChanged(int time)
        {
            _timerText.text = TimeSpan.FromSeconds(time).ToHoursMinutesSeconds();
        }

        private void OnStateChanged(bool state)
        {
            _startButtonText.text = state ? StartButtonTextOnPause : StartButtonTextDefault;
        }

        private void OnStartButtonClicked(Unit unit)
        {
            if (ViewModel.Time.Value == 0)
            {
                Logger.DebugLogError(this,
                    $"Cannot start with Time: {TimeSpan.FromSeconds(0).ToHoursMinutesSeconds()}");
                return;
            }

            ViewModel.StartClickCommand.Execute();
        }

        private void OnBackButtonClicked(Unit unit)
        {
            _showHideTimer.Hide().Forget();
            ViewModel.BackClickCommand.Execute();
        }

        private void OnResetButtonClicked(Unit unit)
        {
            ViewModel.ResetClickCommand.Execute();
        }

        private void OnButtonPointerUp()
        {
            _isPointerDownActive = false;
        }

        private void OnButtonPointerClick(ReactiveCommand command)
        {
            command.Execute();
        }

        private async void OnButtonPointerDown(Func<bool> canExecute, ReactiveCommand command)
        {
            _isPointerDownActive = true;
            var currentStep = _timerControlConfig.DefaultStep;

            while (canExecute() && _isPointerDownActive)
            {
                await UniTask.Delay(_timerControlConfig.MillisecondsToIncreaseStep);

                if (currentStep < _timerControlConfig.MaxStep)
                {
                    currentStep++;
                }

                for (var i = 0; i < currentStep; i++)
                {
                    command.Execute();
                }
            }
        }

        private void RemoveTriggerSubscriptions()
        {
            foreach (var subscription in _increaseButtonSubscriptions)
            {
                subscription?.Dispose();
            }

            foreach (var subscription in _decreaseButtonSubscriptions)
            {
                subscription?.Dispose();
            }

            _increaseButtonSubscriptions.Clear();
            _decreaseButtonSubscriptions.Clear();
        }
    }
}