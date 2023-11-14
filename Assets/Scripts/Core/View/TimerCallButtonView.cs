using DG.Tweening;
using UIManagementDemo.Core.Model;
using UIManagementDemo.Core.ViewModel;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Logger = Utilities.Logger;

namespace UIManagementDemo.Core.View
{
    public class TimerCallButtonView : BindableView<TimerCallButtonViewModel>
    {
        [SerializeField] private Button _button;
        [SerializeField] private Text _text;
        [SerializeField] private Image _image;
        [SerializeField] private Color _colorOnInactive;
        [SerializeField] private Color _colorOnActive;
        [SerializeField] private float _colorDuration = 0.5f;

        private Tween _tween;
        
        protected override void OnBind(CompositeDisposable disposables)
        {
            Logger.DebugLog(this, $"OnBind");
            
            ViewModel.ButtonText
                .Subscribe(text => _text.text = text)
                .AddTo(disposables);
            
            _button.OnClickAsObservable()
                .Subscribe(_ => ViewModel.ClickCommand.Execute())
                .AddTo(disposables);
        }

        public void ColorButtonOnActive()
        {
            _tween?.Kill();
            _tween = _image.DOColor(_colorOnActive, _colorDuration);
        }
        
        public void ColorButtonOnInactive()
        {
            _tween?.Kill();
            _tween = _image.DOColor(_colorOnInactive, _colorDuration);
        }
        
        public class Factory : PlaceholderFactory<TimerCallButtonView>
        {
        }
    }
}