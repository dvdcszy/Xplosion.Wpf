using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Xplosion.Wpf.Core
{
    internal class RelayCommand : ICommand
    {
        private Action mAction;

        #region Constructor

        public RelayCommand(Action action)
        {
            mAction = action;
        }

        public event EventHandler CanExecuteChanged = (sender, e) => { };

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            mAction();
        }

        #endregion
    }
}
