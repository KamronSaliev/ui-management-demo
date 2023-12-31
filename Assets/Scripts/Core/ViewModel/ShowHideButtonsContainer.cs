using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UIManagementDemo.Core.Config;
using UIManagementDemo.Core.Mono;
using UIManagementDemo.Core.ViewModel.Interfaces;
using Utilities.ExtensionMethods.RX;

namespace UIManagementDemo.Core.ViewModel
{
    public class ShowHideButtonsContainer : DisposableObject, IShowHideButtonsContainer
    {
        private readonly ShowHideButtonsContainerConfig _config;

        private readonly List<ShowHideItem> _buttons = new();

        public ShowHideButtonsContainer(ShowHideButtonsContainerConfig config)
        {
            _config = config;
        }

        public void Push(ShowHideItem item, bool show = false)
        {
            _buttons.Add(item);

            if (show)
            {
                item.Show().Forget();
            }
        }

        public async void Pop()
        {
            var peek = _buttons[^1];
            await peek.Hide();
            _buttons.Remove(peek);
        }

        public async void Show()
        {
            await ShowHideButtons(true);
        }

        public async void Hide()
        {
            await ShowHideButtons(false);
        }

        private async UniTask ShowHideButtons(bool show)
        {
            foreach (var button in _buttons)
            {
                if (show)
                {
                    button.Show().Forget();
                }
                else
                {
                    button.Hide().Forget();
                }

                await UniTask.Delay(TimeSpan.FromSeconds(_config.Delay));
            }
        }
    }
}