using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UIManagementDemo.Core.ViewModel;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Logger = Utilities.Logger;
using UniTaskExtensions = Utilities.ExtensionMethods.UniTaskExtensions;

namespace UIManagementDemo.Core.View
{
    public class TimerCallButtonView : BindableView<TimerCallButtonViewModel>
    {
        public ShowHideButton ShowHideButton => _showHideButton;

        [SerializeField] private Button _button;
        [SerializeField] private ShowHideButton _showHideButton;
        [SerializeField] private Text _text;
        [SerializeField] private Image _image;

        [SerializeField] private Color _colorOnInactive;
        [SerializeField] private Color _colorOnActive;
        [SerializeField] private float _colorDuration = 0.5f;

        private CancellationTokenSource _cts = new();

        protected override void OnBind(CompositeDisposable disposables)
        {
            Logger.DebugLog(this, "OnBind");

            ViewModel.ButtonText
                .Subscribe(text => _text.text = text)
                .AddTo(disposables);

            _button.OnClickAsObservable()
                .Subscribe(_ => ViewModel.ClickCommand.Execute())
                .AddTo(disposables);
        }

        public void ColorButtonOnActive()
        {
            ColorButton(_colorOnActive).Forget();
        }

        public void ColorButtonOnInactive()
        {
            ColorButton(_colorOnInactive).Forget();
        }

        private async UniTask ColorButton(Color color)
        {
            UniTaskExtensions.Stop(ref _cts);
            _cts = new CancellationTokenSource();
            
            await _image
                .DOColor(color, _colorDuration)
                .SetEase(Ease.InOutSine)
                .WithCancellation(_cts.Token);
        }

        public class Factory : PlaceholderFactory<TimerCallButtonView>
        {
        }
    }
}