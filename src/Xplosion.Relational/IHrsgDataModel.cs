using System.Collections.Generic;

namespace Xplosion.Relational
{
    public interface IHrsgDataModel
    {
        List<MlFormula> GetFormulaList();
        List<MlParameters> GetParameterList();
    }
}