using UIManagementDemo.Core.Mono;

namespace UIManagementDemo.Core.ViewModel.Interfaces
{
    public interface IShowHideButtonsContainer
    {
        void Add(ShowHideButton button, bool show = false);

        void Show();

        void Hide();
    }
}