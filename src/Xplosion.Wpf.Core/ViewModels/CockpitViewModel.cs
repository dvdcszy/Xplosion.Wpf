using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xplosion.Core.FrameworkLibrary;
using LiveCharts;


namespace Xplosion.Wpf.Core
{
    public class CockpitViewModel : BaseViewModel
    {
        private IComponentBuilder _componentBuilder;

        /// <summary>
        /// Collection of <see cref="XplodedFormulaParentModel"/> available for UI list bindings
        /// </summary>
        public ObservableCollection<XplodedFormulaParentModel> PartsList { get; set; }

        public ChartValues<double> Amounts { get; set; }

        public string[] PartNames { get; set; }

        public CockpitViewModel(IComponentBuilder builder)
        {
            _componentBuilder = builder;

            PartsList = new ObservableCollection<XplodedFormulaParentModel>(_componentBuilder.RunXplosion());

            Amounts = new ChartValues<double>(PartsList.Select(p => p.FormulaValue));

            PartNames = PartsList.Select(p => p.FormulaCode).ToArray();
        }
    }
}
