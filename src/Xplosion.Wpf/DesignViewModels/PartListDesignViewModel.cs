using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xplosion.Wpf
{
    public class PartListDesignViewModel : PartItemDesignViewModel
    {
        public ObservableCollection<PartItemDesignViewModel> PartsList { get; set; }
        /// <summary>
        /// Singleton static insance for design time ViewModel
        /// </summary>
        public static PartListDesignViewModel Instance = new PartListDesignViewModel();

        public PartListDesignViewModel()
        {
            PartsList = new ObservableCollection<PartItemDesignViewModel>
            {
                new PartItemDesignViewModel
                {
                    FormulaCode = "PP-PPF-HA_ACM",
                    FormulaDescription = "Pressure Parts Harps - Fab / Assembly",
                    FormulaValue = 498709,
                    PartMhr = 17685
                },

                new PartItemDesignViewModel
                {
                    FormulaCode = "PP-PPF-HD_ACM",
                    FormulaDescription = "Pressure Parts Headers - Fab / Assembly",
                    FormulaValue = 346116,
                    PartMhr = 12274
                },

                new PartItemDesignViewModel
                {
                    FormulaCode = "PP-PPF-HA_ACM",
                    FormulaDescription = "Pressure Parts Harps - Fab / Assembly",
                    FormulaValue = 498709,
                    PartMhr = 17685
                },

                new PartItemDesignViewModel
                {
                    FormulaCode = "PP-PPF-HD_ACM",
                    FormulaDescription = "Pressure Parts Headers - Fab / Assembly",
                    FormulaValue = 346116,
                    PartMhr = 12274
                },
            };
        }
    }
}
