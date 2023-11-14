using System;
using DG.Tweening;
using UIManagementDemo.Core.ViewModel;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Utilities.ExtensionMethods;
using Logger = Utilities.Logger;

namespace UIManagementDemo.Core.View
{
    public class TimerView : BindableView<TimerViewModel>, ITimerView
    {
        [SerializeField] private Text _timerText;
        [SerializeField] private Button _startButton;
        [SerializeField] private Button _increaseButton;
        [SerializeField] private Button _decreaseButton;
        [SerializeField] private Button _backButton;
        [SerializeField] private float _duration = 0.5f;

        private Vector3 _initialScale;
        private Vector3 _defaultScale;

        private Tween _tween;

        private void Start()
        {
            _defaultScale = transform.localScale;
            transform.localScale = _initialScale;
        }

        protected override void OnBind(CompositeDisposable disposables)
        {
            Logger.DebugLog(this, "OnBind");

            ViewModel.Time
                .Subscribe(UpdateTimerText)
                .AddTo(disposables);
            
            _startButton.OnClickAsObservable()
                .Subscribe(_ => ViewModel.StartClickCommand.Execute())
                .AddTo(disposables);

            _increaseButton.OnClickAsObservable()
                .Subscribe(_ => ViewModel.IncreaseClickCommand.Execute())
                .AddTo(disposables);

            _decreaseButton.OnClickAsObservable()
                .Subscribe(_ => ViewModel.DecreaseClickCommand.Execute())
                .AddTo(disposables);
            
            _backButton.OnClickAsObservable()
                .Subscribe(_ => ViewModel.BackClickCommand.Execute())
                .AddTo(disposables);
        }

        private void UpdateTimerText(int time)
        {
            _timerText.text = TimeSpan.FromSeconds(time).ToMinutesAndSeconds();
        }
        
        public void Show()
        {
            Logger.DebugLog(this, "Show");

            _tween?.Kill();
            _tween = transform.DOScale(_defaultScale, _duration);
        }

        public void Hide()
        {
            Logger.DebugLog(this, "Hide");
            
            _tween?.Kill();
            _tween = transform.DOScale(_initialScale, _duration);
        }
    }
}