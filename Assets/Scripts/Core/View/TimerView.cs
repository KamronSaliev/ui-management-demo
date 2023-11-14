using System;
using Cysharp.Threading.Tasks;
using UIManagementDemo.Core.Config;
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
    public class TimerView : BindableView<TimerViewModel>
    {
        public ShowHideTimer ShowHideTimer => _showHideTimer;

        [SerializeField] private Text _timerText;
        [SerializeField] private Button _startButton;
        [SerializeField] private Button _increaseButton;
        [SerializeField] private Button _decreaseButton;
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _stopButton;
        [SerializeField] private Button _resetButton;

        [SerializeField] private ShowHideTimer _showHideTimer;
        [SerializeField] private TimerControlConfig _timerControlConfig;

        private readonly CompositeDisposable _increaseButtonSubscriptions = new();
        private readonly CompositeDisposable _decreaseButtonSubscriptions = new();

        private bool _isPointerDownActive;

        protected override void OnBind(CompositeDisposable disposables)
        {
            Logger.DebugLog(this, "OnBind");

            RemoveSubscriptions();

            ViewModel.Time
                .Subscribe(UpdateTimerText)
                .AddTo(disposables);

            _startButton.OnClickAsObservable()
                .Subscribe(_ => ViewModel.StartClickCommand.Execute())
                .AddTo(disposables);

            _backButton.OnClickAsObservable()
                .Subscribe(_ => ViewModel.BackClickCommand.Execute())
                .AddTo(disposables);

            _increaseButtonSubscriptions.Add(_increaseButton.OnPointerUpAsObservable()
                .Subscribe(_ => OnButtonPointerUp())
                .AddTo(this));
            _increaseButtonSubscriptions.Add(_increaseButton.OnPointerClickAsObservable()
                .Subscribe(_ => OnButtonPointerClick(ViewModel.IncreaseCommand))
                .AddTo(disposables));
            _increaseButtonSubscriptions.Add(_increaseButton.OnLongPointerDownAsObservable()
                .Subscribe(_ => OnButtonPointerDown(() => ViewModel.CanIncrease(), ViewModel.IncreaseCommand))
                .AddTo(this));

            _decreaseButtonSubscriptions.Add(_decreaseButton.OnPointerUpAsObservable()
                .Subscribe(_ => OnButtonPointerUp())
                .AddTo(this));
            _decreaseButtonSubscriptions.Add(_decreaseButton.OnPointerClickAsObservable()
                .Subscribe(_ => OnButtonPointerClick(ViewModel.DecreaseCommand))
                .AddTo(disposables));
            _decreaseButtonSubscriptions.Add(_decreaseButton.OnLongPointerDownAsObservable()
                .Subscribe(_ => OnButtonPointerDown(() => ViewModel.CanDecrease(), ViewModel.DecreaseCommand))
                .AddTo(this));

            _stopButton.OnClickAsObservable()
                .Subscribe(_ => ViewModel.StopClickCommand.Execute())
                .AddTo(disposables);

            _resetButton.OnClickAsObservable()
                .Subscribe(_ => ViewModel.ResetClickCommand.Execute())
                .AddTo(disposables);
        }

        private void UpdateTimerText(int time)
        {
            _timerText.text = TimeSpan.FromSeconds(time).ToHoursMinutesSeconds();
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

        private void RemoveSubscriptions()
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