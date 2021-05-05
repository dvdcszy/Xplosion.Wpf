using System;
using System.Collections.Generic;
using System.Text;

namespace Xplosion.Core
{

    public class MLFormulaParentObject
    {
        public string Code { get; set; }

        public double Value { get; set; }

        public string Formula { get; set; }

        public double Result { get; set; }

        public string Description { get; set; }

        public List<MLParameterChildObject> ChildrenCollection { get; set; } = new List<MLParameterChildObject>();
    }

    public class MLParameterChildObject
    {
        public string Code { get; set; }

        public string Value { get; set; }

        public string Operation { get; set; }

        public double OperationResult { get; set; }

        public double Cost { get; set; }

        public string Description { get; set; }

        public string UnitOfMeasure { get; set; }

        public MLFormulaParentObject Parent { get; set; }
    }
}
