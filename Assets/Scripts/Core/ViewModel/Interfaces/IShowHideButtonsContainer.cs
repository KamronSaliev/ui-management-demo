using UIManagementDemo.Core.Mono;

namespace UIManagementDemo.Core.ViewModel.Interfaces
{
    public interface IShowHideButtonsContainer
    {
        public void Add(ShowHideButton button, bool show = false);

        public void Show();

        public void Hide();
    }
}