using TMPro;
using UIManagementDemo.Core.Mono;
using UIManagementDemo.Core.ViewModel;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UIManagementDemo.Core.View
{
    public class CallButtonView : BindableView<CallButtonViewModel>
    {
        public ShowHideItem ShowHideItem => _showHideItem;

        [SerializeField] private Button _button;
        [SerializeField] private ShowHideItem _showHideItem;
        [SerializeField] private ColorButton _colorButton;
        [SerializeField] private TMP_Text _text;

        protected override void OnBind(CompositeDisposable disposables)
        {
            ViewModel.ButtonName
                .Subscribe(OnButtonNameChanged)
                .AddTo(disposables);
            
            ViewModel.State
                .Subscribe(OnStateChanged)
                .AddTo(disposables);

            _button.OnClickAsObservable()
                .Subscribe(_ => ViewModel.ClickCommand.Execute())
                .AddTo(disposables);
        }

        private void OnButtonNameChanged(string buttonName)
        {
            _text.text = buttonName;
        }
        
        private void OnStateChanged(bool state)
        {
            if (state)
            {
                _colorButton.ColorOnActive();
            }
            else
            {
                _colorButton.ColorOnInactive();
            }
        }

        public class Factory : PlaceholderFactory<CallButtonView>
        {
        }
    }
}