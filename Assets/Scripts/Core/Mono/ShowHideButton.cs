using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UIManagementDemo.Core.Config;
using UnityEngine;
using UnityEngine.UI;
using UniTaskExtensions = Utilities.ExtensionMethods.UniTaskExtensions;

namespace UIManagementDemo.Core.Mono
{
    public class ShowHideButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private ShowHideConfig _config;

        private CancellationTokenSource _cts = new();

        private void Start()
        {
            _button.transform.localPosition = _config.OnHideParameter;
        }

        public async UniTask Show()
        {
            _button.gameObject.SetActive(true);
            await MoveButton(_config.OnShowParameter);
        }

        public async UniTask Hide()
        {
            await MoveButton(_config.OnHideParameter);
            _button.gameObject.SetActive(false);
        }

        private async UniTask MoveButton(Vector3 targetPosition)
        {
            UniTaskExtensions.Stop(ref _cts);
            _cts = new CancellationTokenSource();

            await _button.transform
                .DOLocalMove(targetPosition, _config.TweenDuration)
                .SetEase(Ease.InOutSine)
                .WithCancellation(_cts.Token);
        }
    }
}