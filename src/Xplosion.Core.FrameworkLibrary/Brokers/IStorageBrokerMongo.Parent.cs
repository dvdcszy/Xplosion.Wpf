using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace Xplosion.Core.FrameworkLibrary
{
    interface IStorageBrokerMongo
    {

        void SelectAllXplodedFormulas();

        void SelectXplodedFormulaById(ObjectId XplodedFormulaId);

        void DeleteXplodedFormula();

        void InsertXplodedFormula();

    }
}
