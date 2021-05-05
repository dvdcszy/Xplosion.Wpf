using System;
using System.Collections.Generic;
using System.Linq;

namespace Xplosion.Core.FrameworkLibrary
{
    public sealed class MLParameterParentBuilder
    {
        #region Private members

        /// <summary>
        /// Private property to store the Funcs in a <see cref="List{T}"/> until aggregation in Build().
        /// </summary>
        private readonly List<Func<XplodedFormulaParentModel, XplodedFormulaParentModel>> actions = new List<Func<XplodedFormulaParentModel, XplodedFormulaParentModel>>();
        #endregion

        #region Object building methods

        /// <summary>
        /// Adds a fluent builder method to the list of actions
        /// </summary>
        /// <param name="code">The Code property of the <see cref="XplodedFormulaParentModel"/></param>
        /// <returns> An action added to the </returns>
        public MLParameterParentBuilder AddParentCode(string parentCode) => Do(p => p.FormulaCode = parentCode);
                                           
        public MLParameterParentBuilder AddParentValue(double parentValue) => Do(p => p.FormulaValue = parentValue);
                                           
        public MLParameterParentBuilder AddParentFormula(string parentForm) => Do(p => p.Formula = parentForm);
                                           
        public MLParameterParentBuilder AddParentDescription(string parentDesc) => Do(p => p.FormulaDescription = parentDesc);
                                           
        public MLParameterParentBuilder AddChildCollectionToParentCollection(List<XplodedFormulaChildModel> mlChild) => Do(p => p.ChildrenCollection.AddRange(mlChild));

        #endregion

        #region Builder methods
        public XplodedFormulaParentModel Build() => actions.Aggregate(new XplodedFormulaParentModel(), (c, f) => f(c));

        public MLParameterParentBuilder Do(Action<XplodedFormulaParentModel> action) => AddAction(action);

        private MLParameterParentBuilder AddAction(Action<XplodedFormulaParentModel> action)
        {
            actions.Add(p =>
            {
                action(p);
                return p;
            });

            return this;
        }
        #endregion
    }

    //public static class MLParameterChildBuilderExtensions
    //{
    //    public static MLParameterChildBuilder AddParentCode(this MLParameterChildBuilder builder, string parentCode) => builder.Do(p => p.FormulaCode = parentCode);

    //    public static MLParameterChildBuilder AddParentValue(this MLParameterChildBuilder builder, double parentValue) => builder.Do(p => p.FormulaValue = parentValue);

    //    public static MLParameterChildBuilder AddParentFormula(this MLParameterChildBuilder builder, string parentForm) => builder.Do(p => p.Formula = parentForm);

    //    public static MLParameterChildBuilder AddParentFormulaDescription(this MLParameterChildBuilder builder, string parentDesc) => builder.Do(p => p.FormulaDescription = parentDesc);

    //    public static MLParameterChildBuilder AddChildToCollection(this MLParameterChildBuilder builder, XplodedFormulaChildModel mlChild) => builder.Do(p => p.ChildrenCollection.Add(mlChild));

    //}
}
