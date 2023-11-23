using UIManagementDemo.Core.View;
using UIManagementDemo.Core.ViewModel;

namespace UIManagementDemo.Core.Model
{
    public class TimerWrapper
    {
        public readonly CallButtonView CallButtonView;
        public readonly CallButtonViewModel CallButtonViewModel;
        public readonly TimerViewModel TimerViewModel;

        public TimerWrapper
        (
            CallButtonView callButtonView,
            CallButtonViewModel callButtonViewModel,
            TimerViewModel timerViewModel
        )
        {
            CallButtonView = callButtonView;
            CallButtonViewModel = callButtonViewModel;
            TimerViewModel = timerViewModel;
        }
    }
}