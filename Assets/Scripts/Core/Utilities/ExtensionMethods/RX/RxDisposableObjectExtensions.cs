using System;
using UniRx;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Utilities.ExtensionMethods.RX
{
    public class DisposableObject : IDisposable, DisposableObject.IAdd
    {
        public interface IAdd
        {
            public void Add(IDisposable disposable);
        }

        public bool IsDisposed { get; private set; }

        private CompositeDisposable _disposables = new();

        public virtual void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }

            IsDisposed = true;

            _disposables.Dispose();
        }

        void IAdd.Add(IDisposable disposable)
        {
            _disposables.Add(disposable);
        }

        protected void Recover()
        {
            _disposables = new CompositeDisposable();
        }
    }

    public static class RxDisposableObjectExtensions
    {
        public static T AddTo<T>(this T disposable, DisposableObject.IAdd disposableObject) where T : IDisposable
        {
            disposableObject.Add(disposable);
            return disposable;
        }

        public static IDisposable CreateDestroyDisposable(this GameObject gameObject)
        {
            return Disposable.CreateWithState(gameObject, obj =>
            {
                if (obj != null)
                {
                    Object.Destroy(obj);
                }
            });
        }
    }
}