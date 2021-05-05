using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson.Serialization.Attributes;

namespace Xplosion.Relational
{
    public class MlParameters
    {
        public double project_id_pcs { get; set; }

        public double project_id_opp { get; set; }

        public string project_name { get; set; }

        public string parameter { get; set; }

        public string value { get; set; }

        public string parameter_desc { get; set; }

        public string parameter_uom { get; set; }

    }
}
