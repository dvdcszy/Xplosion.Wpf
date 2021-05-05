using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Xplosion.Wpf.Core;

namespace Xplosion.Wpf
{
    public class WindowAdapter : IWindow
    {
        private readonly Window _wpfWindow;

        public WindowAdapter(Window wpfWindow)
        {
            if (wpfWindow == null)
            {
                throw new ArgumentNullException("window");
            }

            this._wpfWindow = wpfWindow;
        }

        #region IWindow Members

        public virtual void Close()
        {
            this._wpfWindow.Close();
        }

        public virtual IWindow CreateChild(object viewModel)
        {
            var cw = new MainWindow();
            cw.Owner = this._wpfWindow;
            cw.DataContext = viewModel;
            //WindowAdapter.ConfigureBehavior(cw);

            return new WindowAdapter(cw);
        }

        public virtual void Show()
        {
            this._wpfWindow.Show();
        }

        public virtual bool? ShowDialog()
        {
            return this._wpfWindow.ShowDialog();
        }

        public void Minimize()
        {
            this._wpfWindow.WindowState = WindowState.Minimized;
        }

        public void Maximize()
        {
            this._wpfWindow.WindowState ^= WindowState.Maximized;
        }

        #endregion

        protected Window WpfWindow
        {
            get { return this._wpfWindow; }
        }

        private static void ConfigureBehavior(MainWindow cw)
        {
            cw.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            //cw.CommandBindings.Add(new CommandBinding(PresentationCommands.Accept, (sender, e) => cw.DialogResult = true));
        }
    }
}
