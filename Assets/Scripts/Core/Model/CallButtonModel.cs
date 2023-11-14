namespace UIManagementDemo.Core.Model
{
    public class CallButtonModel
    {
        public string ButtonName { get; private set; }

        public CallButtonModel(string buttonName)
        {
            ButtonName = buttonName;
        }
    }
}