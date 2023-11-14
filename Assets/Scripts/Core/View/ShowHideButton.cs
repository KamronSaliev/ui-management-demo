using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UIManagementDemo.Core.Config;
using UnityEngine;
using UnityEngine.UI;
using UniTaskExtensions = Utilities.ExtensionMethods.UniTaskExtensions;

namespace UIManagementDemo.Core.View
{
    public class ShowHideButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private ShowHideButtonConfig _config;
        
        private CancellationTokenSource _cts = new();

        private void Start()
        {
            _button.transform.localPosition = _config.HidePosition;
        }

        public async UniTask Show()
        {
            await MoveButton(_config.ShowPosition);
        }

        public async UniTask Hide()
        {
            await MoveButton(_config.HidePosition);
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