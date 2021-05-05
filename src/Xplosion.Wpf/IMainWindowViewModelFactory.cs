using Xplosion.Wpf.Core;

namespace Xplosion.Wpf
{
    public interface IMainWindowViewModelFactory
    {
        MainWindowViewModel Create(IWindow window);
    }
}