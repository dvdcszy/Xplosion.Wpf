using System.Collections.Generic;
using Dapper;
using System.Data.SqlClient;
using static System.Configuration.ConfigurationManager;
using Microsoft.Extensions.Configuration;

namespace Xplosion.Relational
{
    public class HrsgDataModel : IHrsgDataModel
    {
        public List<MlFormula> GetFormulaList()
        {
            string sql = "select * from hrsg_budget.stage.formulas where formula_value <> 0 and ml_description <> 'Standard formula for Labor cost'";

            using (var db = Setup())
            {
                return db.Query<MlFormula>(sql).AsList();
            }
        }

        public List<MlParameters> GetParameterList()
        {
            string sql = "select * from hrsg_budget.stage.parameters";

            using (var db = Setup())
            {
                return db.Query<MlParameters>(sql).AsList();
            }
        }

        private static SqlConnection Setup()
        {
            var sql = new SqlConnection("Server=GCZC9088T25E\\SC_FPNA_DEV;Database=ge_power_mrp;Trusted_Connection=True;");

            return sql;
        }
    }
}
