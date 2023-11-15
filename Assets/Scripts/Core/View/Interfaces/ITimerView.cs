using UIManagementDemo.Core.Mono;
using UIManagementDemo.Core.ViewModel;

namespace UIManagementDemo.Core.View.Interfaces
{
    public interface ITimerView : IChangeableView<TimerViewModel>
    {
        ShowHideTimer ShowHideTimer { get; }
    }
}