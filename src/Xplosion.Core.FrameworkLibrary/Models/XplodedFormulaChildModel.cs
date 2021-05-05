using MongoDB.Bson;

namespace Xplosion.Core.FrameworkLibrary
{
    public class XplodedFormulaChildModel
    {
        public ObjectId Id { get; set; }
        public string Code { get; set; }

        public string Value { get; set; }

        public string Operation { get; set; }

        public double OperationResult { get; set; }

        public double Cost { get; set; }

        public string Description { get; set; }

        public string UnitOfMeasure { get; set; }

        public XplodedFormulaParentModel Parent { get; set; }

        public FormulaTypes FormulaType { get; set; }
    }
}
