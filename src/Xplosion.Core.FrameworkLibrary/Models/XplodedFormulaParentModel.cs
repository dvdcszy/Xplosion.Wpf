using System.Collections.Generic;
using MongoDB.Bson;

namespace Xplosion.Core.FrameworkLibrary
{
    public class XplodedFormulaParentModel
    {
        public ObjectId Id { get; set; }

        public string FormulaCode { get; set; }

        public double FormulaValue { get; set; }

        public string Formula { get; set; }

        public string FormulaDescription { get; set; }

        public List<XplodedFormulaChildModel> ChildrenCollection { get; set; } = new List<XplodedFormulaChildModel>();

    }
}
