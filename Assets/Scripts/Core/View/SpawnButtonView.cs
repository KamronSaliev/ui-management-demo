using UIManagementDemo.Core.Mono;
using UIManagementDemo.Core.View.Interfaces;
using UIManagementDemo.Core.ViewModel;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UIManagementDemo.Core.View
{
    public class SpawnButtonView : BindableView<SpawnButtonViewModel>, ISpawnButtonView
    {
        public ShowHideButton ShowHideButton => _showHideButton;

        [SerializeField] private Button _button;
        [SerializeField] private ShowHideButton _showHideButton;
        
        protected override void OnBind(CompositeDisposable disposables)
        {
            _button.OnClickAsObservable()
                .Subscribe(_ => ViewModel.ClickCommand.Execute())
                .AddTo(disposables);
        }
    }
}