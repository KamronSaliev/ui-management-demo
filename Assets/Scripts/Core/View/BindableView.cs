using System;
using UniRx;
using UnityEngine;

namespace UIManagementDemo.Core.View
{
    public abstract class BindableView<TViewModel> : MonoBehaviour where TViewModel : class
    {
        public TViewModel ViewModel => _viewModel.Value;

        private readonly ReactiveProperty<TViewModel> _viewModel = new();

        private CompositeDisposable _disposables = new();

        public IDisposable BindTo(TViewModel viewModel)
        {
            _disposables.Dispose();
            _disposables = new CompositeDisposable();
            _disposables.AddTo(this);

            _viewModel.Value = viewModel;

            OnBind(_disposables);

            return _disposables;
        }

        public IObservable<TViewModel> ObserveViewModel(bool includeCurrent = true)
        {
            return includeCurrent ? _viewModel.StartWith(ViewModel) : _viewModel;
        }

        public void ChangeViewModel(TViewModel newViewModel)
        {
            UnbindViewModel();
            BindTo(newViewModel);
        }

        private void UnbindViewModel()
        {
            _viewModel.Value = null;
            _disposables.Dispose();
            _disposables = new CompositeDisposable();
        }

        protected abstract void OnBind(CompositeDisposable disposables);
    }
}