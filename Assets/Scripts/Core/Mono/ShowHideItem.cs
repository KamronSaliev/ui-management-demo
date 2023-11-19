using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UIManagementDemo.Core.Config;
using UnityEngine;
using UniTaskExtensions = Utilities.ExtensionMethods.UniTaskExtensions;

namespace UIManagementDemo.Core.Mono
{
    public class ShowHideItem : MonoBehaviour
    {
        [SerializeField] private Transform _targetTransform;
        [SerializeField] private GameObject[] _buttonObjects;
        [SerializeField] private ShowHideConfig _config;

        private CancellationTokenSource _cts = new();

        private void Start()
        {
            _targetTransform.localPosition = _config.OnHideParameter;
        }

        private void OnDestroy()
        {
            UniTaskExtensions.Stop(ref _cts);
        }

        public async UniTask Show()
        {
            for (var i = 0; i < _buttonObjects.Length; i++)
            {
                _buttonObjects[i].SetActive(true);
            }

            await MoveButton(_config.OnShowParameter);
        }

        public async UniTask Hide()
        {
            await MoveButton(_config.OnHideParameter);
            for (var i = 0; i < _buttonObjects.Length; i++)
            {
                _buttonObjects[i].SetActive(false);
            }
        }

        private async UniTask MoveButton(Vector3 targetPosition)
        {
            UniTaskExtensions.Stop(ref _cts);
            _cts = new CancellationTokenSource();

            await _targetTransform
                .DOLocalMove(targetPosition, _config.TweenDuration)
                .SetEase(Ease.InOutSine)
                .WithCancellation(_cts.Token);
        }
    }
}