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
        
        private int _currentStep;
        private bool _isLongIncreaseActive;
        private bool _isLongDecreaseActive;

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
                .Subscribe(_ => IncreaseOnPointerUp())
                .AddTo(this));
            _increaseButtonSubscriptions.Add(_increaseButton.OnPointerClickAsObservable()
                .Subscribe(_ => IncreaseOnPointerClick())
                .AddTo(disposables));
            _increaseButtonSubscriptions.Add(_increaseButton.OnLongPointerDownAsObservable()
                .Subscribe(_ => IncreaseOnLongPointerDown())
                .AddTo(this));

            _decreaseButtonSubscriptions.Add(_decreaseButton.OnPointerUpAsObservable()
                .Subscribe(_ => DecreaseOnPointerUp())
                .AddTo(this));
            _decreaseButtonSubscriptions.Add(_decreaseButton.OnPointerClickAsObservable()
                .Subscribe(_ => DecreaseOnPointerClick())
                .AddTo(disposables));
            _decreaseButtonSubscriptions.Add(_decreaseButton.OnLongPointerDownAsObservable()
                .Subscribe(_ => DecreaseOnLongPointerDown())
                .AddTo(this));
            
            _stopButton.OnClickAsObservable()
                .Subscribe(_ => ViewModel.StopClickCommand.Execute())
                .AddTo(disposables);
            
            _resetButton.OnClickAsObservable()
                .Subscribe(_ => ViewModel.ResetClickCommand.Execute())
                .AddTo(disposables);
        }

        private void IncreaseOnPointerClick()
        {
            Logger.DebugLog(this, "IncreaseButton OnPointerClick");
            ViewModel.IncreaseCommand.Execute();
        }

        private async void IncreaseOnLongPointerDown()
        {
            Logger.DebugLog(this, "IncreaseButton LONG OnPointerDown");

            _currentStep = _timerControlConfig.DefaultStep;
            _isLongIncreaseActive = true;

            while (ViewModel.CanIncrease() && _isLongIncreaseActive)
            {
                await UniTask.Delay(_timerControlConfig.MillisecondsToIncreaseStep);

                if (_currentStep < _timerControlConfig.MaxStep)
                {
                    _currentStep++;
                }

                for (var i = 0; i < _currentStep; i++)
                {
                    ViewModel.IncreaseCommand.Execute();
                }
            }
        }

        private void IncreaseOnPointerUp()
        {
            Logger.DebugLog(this, "IncreaseButton OnPointerUp");
            _isLongIncreaseActive = false;
        }

        private void DecreaseOnPointerClick()
        {
            Logger.DebugLog(this, "DecreaseButton OnPointerClick");
            ViewModel.DecreaseCommand.Execute();
        }

        private async void DecreaseOnLongPointerDown()
        {
            Logger.DebugLog(this, "DecreaseButton LONG OnPointerDown");

            _currentStep = _timerControlConfig.DefaultStep;
            _isLongDecreaseActive = true;

            while (ViewModel.CanIncrease() && _isLongDecreaseActive)
            {
                await UniTask.Delay(_timerControlConfig.MillisecondsToIncreaseStep);

                if (_currentStep < _timerControlConfig.MaxStep)
                {
                    _currentStep++;
                }
                
                for (var i = 0; i < _currentStep; i++)
                {
                    ViewModel.IncreaseCommand.Execute();
                }
            }
        }

        private void DecreaseOnPointerUp()
        {
            Logger.DebugLog(this, "DecreaseButton OnPointerUp");
            _isLongDecreaseActive = false;
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

        private void UpdateTimerText(int time)
        {
            _timerText.text = TimeSpan.FromSeconds(time).ToMinutesAndSeconds();
        }
    }
}