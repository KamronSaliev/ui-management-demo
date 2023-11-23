using UIManagementDemo.Core.ViewModel;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UIManagementDemo.Core.View
{
    public class SpawnButtonView : BindableView<SpawnButtonViewModel>
    {
        [SerializeField] private Button _button;

        protected override void OnBind(CompositeDisposable disposables)
        {
            _button.OnClickAsObservable()
                .Subscribe(_ => ViewModel.ClickCommand.Execute())
                .AddTo(disposables);
        }
    }
}