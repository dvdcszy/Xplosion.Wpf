using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xplosion.Wpf.Core;

namespace Xplosion.Wpf
{
    public class MainWindowViewModelFactory : IMainWindowViewModelFactory
    {
        private readonly IBaseViewModelAbstractFactory _navigator;

        public MainWindowViewModelFactory(IBaseViewModelAbstractFactory navigator)
        {
            if (navigator == null)
            {
                throw new ArgumentNullException("agent");
            }

            this._navigator = navigator;
        }

        #region IViewModelFactory Members


        public MainWindowViewModel Create(IWindow window)
        {
            if (window == null)
            {
                throw new ArgumentNullException("window");
            }

            return new MainWindowViewModel(this._navigator, window);
        }
    }

    #endregion
}
