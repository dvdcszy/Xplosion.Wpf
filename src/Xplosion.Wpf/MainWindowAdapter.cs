using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Xplosion.Wpf
{
    /// <summary>
    /// WindowAdapter implementation on MainWindow
    /// </summary>
    public class MainWindowAdapter : WindowAdapter
    {

        #region Private Members

        /// <summary>
        /// Specific ViewModelFactory storage for DataContext injection
        /// </summary>
        private readonly IMainWindowViewModelFactory vmFactory;

        /// <summary>
        /// Initializd property
        /// </summary>
        private bool initialized;
        #endregion

        #region Default Constructor

        /// <summary>
        /// Default constructor for MainWindowAdapter
        /// </summary>
        /// <param name="wpfWindow"> A Window object for initialization </param>
        /// <param name="viewModelFactory"> The specific ViewModelFactory for the Window.DataContext </param>
        public MainWindowAdapter(Window wpfWindow, IMainWindowViewModelFactory viewModelFactory)
        : base(wpfWindow)
        {
            this.vmFactory = viewModelFactory ?? throw new ArgumentNullException("viewModelFactory");
        }
        #endregion

        #region IWindow Members / Implementation

        public override void Close()
        {
            this.EnsureInitialized();
            base.Close();
        }

        //public override IWindow CreateChild(object viewModel)
        //{
        //    this.EnsureInitialized();
        //    return (IWindow)base.CreateChild(viewModel);
        //}


        public override void Show()
        {
            this.EnsureInitialized();
            base.Show();
        }

        public override bool? ShowDialog()
        {
            this.EnsureInitialized();
            return base.ShowDialog();
        }

        #endregion

        #region Private Helpers

        //private void DeclareKeyBindings(MainWindowViewModel vm)
        //{
        //    //this.WpfWindow.InputBindings.Add(new KeyBinding(vm.RefreshCommand, new KeyGesture(Key.F5)));
        //    //this.WpfWindow.InputBindings.Add(new KeyBinding(vm.InsertProductCommand, new KeyGesture(Key.Insert)));
        //    //this.WpfWindow.InputBindings.Add(new KeyBinding(vm.EditProductCommand, new KeyGesture(Key.Enter)));
        //    //this.WpfWindow.InputBindings.Add(new KeyBinding(vm.DeleteProductCommand, new KeyGesture(Key.Delete)));
        //}

        private void EnsureInitialized()
        {
            if (this.initialized)
            {
                return;
            }

            var vm = this.vmFactory.Create(this);
            this.WpfWindow.DataContext = vm;
            //this.DeclareKeyBindings(vm);

            this.initialized = true;
        }
        #endregion
    }
}
