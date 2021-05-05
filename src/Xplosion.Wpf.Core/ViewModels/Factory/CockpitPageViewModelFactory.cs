using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xplosion.Core.FrameworkLibrary;

namespace Xplosion.Wpf.Core
{
    public class CockpitPageViewModelFactory : IViewModelFactory<CockpitViewModel>
    {

        private readonly IComponentBuilder _compBuilder;

        public CockpitPageViewModelFactory(IComponentBuilder compBuilder)
        {
            _compBuilder = compBuilder;
        }

        public CockpitViewModel CreateViewModel()
        {
            return new CockpitViewModel(_compBuilder);
        }
    }
}
