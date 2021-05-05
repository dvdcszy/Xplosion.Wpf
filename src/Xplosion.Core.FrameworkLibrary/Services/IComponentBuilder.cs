using System.Collections.Generic;

namespace Xplosion.Core.FrameworkLibrary
{
    public interface IComponentBuilder
    {
        List<XplodedFormulaParentModel> RunXplosion();
    }
}