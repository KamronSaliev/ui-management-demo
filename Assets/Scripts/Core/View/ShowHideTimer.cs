using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UIManagementDemo.Core.Config;
using UnityEngine;
using UniTaskExtensions = Utilities.ExtensionMethods.UniTaskExtensions;

namespace UIManagementDemo.Core.View
{
    public class ShowHideTimer : MonoBehaviour
    {
        [SerializeField] private Transform _targetTransform;
        [SerializeField] private ShowHideConfig _config;

        private CancellationTokenSource _cts = new();

        private void Start()
        {
            _targetTransform.localScale = _config.OnHideParameter;
        }

        public async UniTask Show()
        {
            await Scale(_config.OnShowParameter);
        }

        public async UniTask Hide()
        {
            await Scale(_config.OnHideParameter);
        }

        private async UniTask Scale(Vector3 scale)
        {
            UniTaskExtensions.Stop(ref _cts);
            _cts = new CancellationTokenSource();

            await _targetTransform
                .DOScale(scale, _config.TweenDuration)
                .SetEase(Ease.InOutSine)
                .WithCancellation(_cts.Token);
        }
    }
}