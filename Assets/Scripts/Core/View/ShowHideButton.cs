using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UniTaskExtensions = Utilities.ExtensionMethods.UniTaskExtensions;

namespace UIManagementDemo.Core.View
{
    [RequireComponent(typeof(Button))]
    public class ShowHideButton : MonoBehaviour
    {
        [SerializeField] private Button _button;

        // TODO: Use SO
        private Vector3 _showPosition;
        private readonly Vector3 _hidePosition = new(-700, 0, 0);
        private readonly float _tweenDuration = 0.5f;

        private CancellationTokenSource _cts = new();

        private void Start()
        {
            _button.transform.localPosition = _hidePosition;
        }

        public async UniTask Show()
        {
            await MoveButton(_showPosition);
        }

        public async UniTask Hide()
        {
            await MoveButton(_hidePosition);
        }

        private async UniTask MoveButton(Vector3 targetPosition)
        {
            UniTaskExtensions.Stop(ref _cts);
            _cts = new CancellationTokenSource();
            
            await _button.transform
                .DOLocalMove(targetPosition, _tweenDuration)
                .SetEase(Ease.InOutSine)
                .WithCancellation(_cts.Token);
        }
    }
}