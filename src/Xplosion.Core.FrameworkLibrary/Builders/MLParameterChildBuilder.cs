using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xplosion.Core.FrameworkLibrary
{
    public sealed class MLParameterChildBuilder
    {
        #region Private members

        /// <summary>
        /// Private property to store the Funcs in a <see cref="List{T}"/> until aggregation in Build().
        /// </summary>
        private readonly List<Func<XplodedFormulaChildModel, XplodedFormulaChildModel>> actions = new List<Func<XplodedFormulaChildModel, XplodedFormulaChildModel>>();
        #endregion

        #region Object building methods

        /// <summary>
        /// Adds a fluent builder method to the list of actions
        /// </summary>
        /// <param name="code">The Code property of the <see cref="XplodedFormulaChildModel"/></param>
        /// <returns> An action added to the </returns>
        public MLParameterChildBuilder AddChildCode(string code) => Do(p => p.Code = code);

        public MLParameterChildBuilder AddChildValue(string val) => Do(p => p.Value = val);

        public MLParameterChildBuilder AddChildOperation(string op) => Do(p => p.Operation = op);

        public MLParameterChildBuilder AddChildOperationResult(double op) => Do(p => p.OperationResult = op);

        public MLParameterChildBuilder AddChildDescription(string desc) => Do(p => p.Description = desc);

        public MLParameterChildBuilder AddChildUom(string uom) => Do(p => p.UnitOfMeasure = uom);

        public MLParameterChildBuilder AddChildFormulaType(FormulaTypes fTyp) => Do(p => p.FormulaType = fTyp);
        #endregion

        #region Builder methods
        public XplodedFormulaChildModel Build() => actions.Aggregate(new XplodedFormulaChildModel(), (c, f) => f(c));
        public MLParameterChildBuilder Do(Action<XplodedFormulaChildModel> action) => AddAction(action);
        private MLParameterChildBuilder AddAction(Action<XplodedFormulaChildModel> action)
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
