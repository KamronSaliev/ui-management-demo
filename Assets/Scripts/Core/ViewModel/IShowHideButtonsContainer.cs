using UIManagementDemo.Core.View;

namespace UIManagementDemo.Core.ViewModel
{
    public interface IShowHideButtonsContainer
    {
        public void Add(ShowHideButton button, bool show = false);

        public void Show();

        public void Hide();
    }
}