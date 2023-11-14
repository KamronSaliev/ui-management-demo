using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace UIManagementDemo.Core.View
{
    public class ShowHideButtonsContainer : MonoBehaviour
    {
        [SerializeField] private float _delay = 0.01f;
        
        private readonly List<ShowHideButton> _buttons = new();

        public void Add(ShowHideButton button, bool show = false)
        {
            _buttons.Add(button);

            if (show)
            {
                button.Show().Forget();
            }
        }
        
        public async void Show()
        {
            for (var i = 0; i < _buttons.Count; i++)
            {
                _buttons[i].Show().Forget();
                
                await UniTask.Delay(TimeSpan.FromSeconds(_delay));
            }
        }
        
        public async void Hide()
        {
            for (var i = 0; i < _buttons.Count; i++)
            {
                _buttons[i].Hide().Forget();
                
                await UniTask.Delay(TimeSpan.FromSeconds(_delay));
            }
        }
    }
}