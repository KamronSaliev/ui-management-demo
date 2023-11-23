namespace UIManagementDemo.Core.View.Interfaces
{
    public interface IChangeableView<in TViewModel> where TViewModel : class
    {
        void ChangeViewModel(TViewModel newViewModel);
    }
}