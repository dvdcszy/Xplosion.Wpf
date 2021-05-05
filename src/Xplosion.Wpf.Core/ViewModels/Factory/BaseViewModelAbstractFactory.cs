using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xplosion.Wpf.Core
{
    public class BaseViewModelAbstractFactory : IBaseViewModelAbstractFactory
    {
        private readonly IViewModelFactory<ComponentViewModel> _componentPageViewModelFactory;
        private readonly IViewModelFactory<CockpitViewModel> _cockpitPageViewModelFactory;

        public BaseViewModelAbstractFactory(IViewModelFactory<ComponentViewModel> componentPageViewModelFactory, IViewModelFactory<CockpitViewModel> cockpitPageViewModel)
        {
            _componentPageViewModelFactory = componentPageViewModelFactory;
            _cockpitPageViewModelFactory = cockpitPageViewModel;
        }

        public BaseViewModel CreateViewModel(ApplicationPage viewType)
        {
            switch (viewType)
            {
                case ApplicationPage.CockPitPage:
                    return _cockpitPageViewModelFactory.CreateViewModel();
                case ApplicationPage.ComponentPage:
                    return _componentPageViewModelFactory.CreateViewModel();
                default:
                    throw new ArgumentException("The View does not have a ViewModel", "viewType");
            }

        }
    }
}
