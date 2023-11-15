using UIManagementDemo.Core.ViewModel;

namespace UIManagementDemo.Core.Model
{
    public class CallButtonOptions
    {
        public readonly string Name;
        public readonly TimerViewModel TimerViewModel;

        public CallButtonOptions(string name, TimerViewModel timerViewModel)
        {
            Name = name;
            TimerViewModel = timerViewModel;
        }
    }
}