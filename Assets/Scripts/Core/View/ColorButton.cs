using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UIManagementDemo.Core.Config;
using UnityEngine;
using UnityEngine.UI;
using UniTaskExtensions = Utilities.ExtensionMethods.UniTaskExtensions;

namespace UIManagementDemo.Core.View
{
    public class ColorButton : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private ColorButtonConfig _config;
        
        private CancellationTokenSource _cts = new();
        
        public void ColorOnActive()
        {
            UpdateColor(_config.ColorOnActive).Forget();
        }

        public void ColorOnInactive()
        {
            UpdateColor(_config.ColorOnInactive).Forget();
        }

        private async UniTask UpdateColor(Color color)
        {
            UniTaskExtensions.Stop(ref _cts);
            _cts = new CancellationTokenSource();
            
            await _image
                .DOColor(color, _config.TweenDuration)
                .SetEase(Ease.InOutSine)
                .WithCancellation(_cts.Token);
        }
    }
}