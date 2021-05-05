using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Xplosion.Wpf.Core
{
    public class MainWindowViewModel : BaseViewModel
    {
        private IBaseViewModelAbstractFactory _viewModelFactory;
        private IWindow _window;

        public ICommand MinimizeCommand { get; set; }
        public ICommand MaximizeCommand { get; set; }
        public ICommand CloseCommand { get; set; }
        /// <summary>
        /// The smallest width the window can go to
        /// </summary>
        public double WindowWidth { get; set; } = 880;

        /// <summary>
        /// The smallest height the window can go to
        /// </summary>
        public double WindowHeight { get; set; } = 550;

        private BaseViewModel _currentViewModel;

        public BaseViewModel CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                _currentViewModel = value;
                OnPropertyChanged(nameof(CurrentViewModel));
            }
        }

        public ICommand UpdateViewModelCommand { get; set; }

        public MainWindowViewModel(IBaseViewModelAbstractFactory vmFactory, IWindow window)
        {
            _viewModelFactory = vmFactory;
            _currentViewModel = vmFactory.CreateViewModel(ApplicationPage.ComponentPage);
            _window = window;
            UpdateViewModelCommand = new RelayParameterizedCommand((parameter) => UpdateVm(parameter));

            MinimizeCommand = new RelayCommand(Minimize);
            MaximizeCommand = new RelayCommand(Maximize);
            CloseCommand = new RelayCommand(Close);

        }

        private void Close()
        {
            _window.Close();
        }

        private void Maximize()
        {
            _window.Maximize();
        }

        private void Minimize()
        {
            _window.Minimize();
        }

        private void UpdateVm(object parameter)
        {
            if (parameter is ApplicationPage)
            {
                ApplicationPage appPage = (ApplicationPage)parameter;

                CurrentViewModel = _viewModelFactory.CreateViewModel(appPage);
            }
        }
    }
}
