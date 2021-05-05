namespace Xplosion.Wpf.Core
{
    public interface IBaseViewModelAbstractFactory
    {
        BaseViewModel CreateViewModel(ApplicationPage viewType);
    }
}