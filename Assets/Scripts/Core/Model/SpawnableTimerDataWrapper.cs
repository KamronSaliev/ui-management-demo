using UIManagementDemo.Core.ViewModel;

namespace UIManagementDemo.Core.Model
{
    public class SpawnableTimerDataWrapper
    {
        public readonly TimerViewModel TimerViewModel;
        public readonly CallButtonViewModel CallButtonViewModel;

        public SpawnableTimerDataWrapper(TimerViewModel timerViewModel, CallButtonViewModel callButtonViewModel)
        {
            TimerViewModel = timerViewModel;
            CallButtonViewModel = callButtonViewModel;
        }
    }
}