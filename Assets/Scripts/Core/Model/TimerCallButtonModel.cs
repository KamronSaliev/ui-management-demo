namespace UIManagementDemo.Core.Model
{
    public class TimerCallButtonModel
    {
        public string ButtonText { get; private set; }

        public TimerCallButtonModel(string buttonText)
        {
            ButtonText = buttonText;
        }
    }
}