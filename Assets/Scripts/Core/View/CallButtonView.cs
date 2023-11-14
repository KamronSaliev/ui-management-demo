using UIManagementDemo.Core.ViewModel;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Logger = Utilities.Logger;

namespace UIManagementDemo.Core.View
{
    public class CallButtonView : BindableView<CallButtonViewModel>
    {
        public ShowHideButton ShowHideButton => _showHideButton;

        public ColorButton ColorButton => _colorButton;

        [SerializeField] private Button _button;
        [SerializeField] private ShowHideButton _showHideButton;
        [SerializeField] private ColorButton _colorButton;
        [SerializeField] private Text _text;

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

        public class Factory : PlaceholderFactory<CallButtonView>
        {
        }
    }
}