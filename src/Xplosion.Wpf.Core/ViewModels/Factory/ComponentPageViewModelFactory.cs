using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xplosion.Core;
using Xplosion.Core.FrameworkLibrary;

namespace Xplosion.Wpf.Core
{
    public class ComponentPageViewModelFactory : IViewModelFactory<ComponentViewModel>
    {

        private readonly IComponentBuilder _compBuilder;

        public ComponentPageViewModelFactory(IComponentBuilder compBuilder)
        {
            _compBuilder = compBuilder;
        }

        public ComponentViewModel CreateViewModel()
        {
            return new ComponentViewModel(_compBuilder);
        }
    }
}
