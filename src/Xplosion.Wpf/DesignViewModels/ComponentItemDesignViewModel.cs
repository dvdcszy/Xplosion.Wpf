using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xplosion.Wpf
{
    /// <summary>
    /// Interaction logic for ComponentItemControl design time 
    /// </summary>
    public class ComponentItemDesignViewModel : BaseDesignViewModel
    {
        public string Code { get; set; }

        public string Value { get; set; }

        public string Operation { get; set; }

        public double OperationResult { get; set; }

        public double Cost { get; set; }

        public string Description { get; set; }

        public string UnitOfMeasure { get; set; }

        /// <summary>
        /// Singleton static insance for design time ViewModel
        /// </summary>
        public static ComponentItemDesignViewModel Instance = new ComponentItemDesignViewModel();

        /// <summary>
        /// Default constructor
        /// </summary>
        public ComponentItemDesignViewModel()
        {
            Code = "[15400]";

            Value = "129.00";

            Operation = "[15400]*78.8";

            OperationResult = 10165.2;

            Cost = 286659;

            Description = "NO. OF CS HARPS (2 ROW)";

            UnitOfMeasure = "EA";


        }
    }
}
