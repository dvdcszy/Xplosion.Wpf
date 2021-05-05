using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xplosion.Core.FrameworkLibrary
{
    public enum RegexType
    {
        ComplexMoreThanOneBrackets,
        ComplexOnlyOneBracket,
        BracketedIf,
        Lookups,
        ParametersInBrackets,
        Operation,
        ParametersOnly,
        IfsConditionPart
    }
}
