using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xplosion.Wpf.Core
{
    public interface IWindow
    {
        void Close();
        IWindow CreateChild(object viewModel);
        void Maximize();
        void Minimize();
        void Show();
        bool? ShowDialog();
    }
}
