namespace UIManagementDemo.Core.View.Interfaces
{
    public interface IChangeableView<in TViewModel> where TViewModel : class
    {
        public void ChangeViewModel(TViewModel newViewModel);
    }
}