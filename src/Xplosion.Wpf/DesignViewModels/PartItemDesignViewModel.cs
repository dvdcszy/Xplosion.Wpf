using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xplosion.Wpf
{
    public class PartItemDesignViewModel : BaseDesignViewModel
    {
        public string FormulaCode { get; set; }

        public static PartListDesignViewModel ItemInstance = new PartListDesignViewModel();

        public string FormulaDescription { get; set; }

        public double FormulaValue { get; set; }

        public double PartMhr { get; set; }

        public PartItemDesignViewModel()
        {
            FormulaCode = "PP-PPF-HA_ACM";
            FormulaDescription = "Pressure Parts Harps - Fab / Assembly";
            FormulaValue = 498709;
            PartMhr = 17685;
        }
    }
}
