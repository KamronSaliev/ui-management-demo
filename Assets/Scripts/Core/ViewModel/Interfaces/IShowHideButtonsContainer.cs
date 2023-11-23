using UIManagementDemo.Core.Mono;

namespace UIManagementDemo.Core.ViewModel.Interfaces
{
    public interface IShowHideButtonsContainer
    {
        void Push(ShowHideItem item, bool show = false);

        void Pop();

        void Show();

        void Hide();
    }
}