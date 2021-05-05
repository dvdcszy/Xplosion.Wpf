using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using NCalc;
using Xplosion.Relational;

namespace Xplosion.Core.FrameworkLibrary
{
    public class ComponentBuilder : IComponentBuilder
    {
        #region Private memebers
        private string _currentFormula { get; set; }

        private string _formulaLastState { get; set; }

        private IHrsgDataModel hrsgDm { get; set; }

        private List<MlParameters> mlParameters { get; set; }

        #endregion

        /// <summary>
        /// Default constructor
        /// </summary>
        public ComponentBuilder(IHrsgDataModel hrsgDm)
        {
            // Store the datamodel
            this.hrsgDm = hrsgDm;

            // Get the list of all Parmeters and their properties
            this.mlParameters = hrsgDm.GetParameterList();
        }

        /// <summary>
        /// Method to split (deserialize) each formula with a RegEx and build the actual objects
        /// </summary>
        /// <returns> A <see cref="List{T}"/> of Pressure Part objects </returns>
        public List<XplodedFormulaParentModel> RunXplosion()
        {
            // Result collection to return
            var res = new List<XplodedFormulaParentModel>();

            foreach (var item in this.hrsgDm.GetFormulaList())
            {

                // Create the component functional builder for parent parts
                var parentBuilder = new MLParameterParentBuilder();

                // Store the current formula for later use
                _currentFormula = item.formula;

                // Add parent details
                parentBuilder.AddParentFormula(CleanFormula(item.formula))
                            .AddParentCode(item.ml_code)
                            .AddParentDescription(item.ml_description)
                            .AddParentValue(item.formula_value)
                            .AddChildCollectionToParentCollection(DeserializeChildParametersToObjects());

                // Add built parent object to result list
                res.Add(parentBuilder.Build());
            }

            // Return result collection
            return res;
        }

        /// <summary>
        /// Deserializes the PCS ML formula string into compiled <see cref="XplodedFormulaChildModel"/> components
        /// </summary>
        /// <returns>A generic <see cref="List{XplodedFormulaChildModel}"/></returns>
        private List<XplodedFormulaChildModel> DeserializeChildParametersToObjects()
        {
            // Create a list of components
            var result = new List<XplodedFormulaChildModel>();

            // Create the component functional builder for children parameters
            var childBuilder = new MLParameterChildBuilder();

            // Build "If()" parts
            result.AddRange(EvaluateIfs(childBuilder));

            // Check to see if anything remained in the formula
            if (_formulaLastState == "" || _formulaLastState.Replace(" ", "") == "")
                return result;
            // Build the "Lookup()" parts
            result.AddRange(Lookups(childBuilder));

            // Check to see if anything remained in the formula
            if (_formulaLastState == "" || _formulaLastState.Replace(" ", "") == "")
                return result;
            // Build parts with more brackets
            result.AddRange(ComplexBranch(childBuilder));

            // Check to see if anything remained in the formula
            if (_formulaLastState == "" || _formulaLastState.Replace(" ", "") == "")
                return result;
            // Build the simple expression parts
            result.AddRange(SimpleBranch(childBuilder));

            // Return the XplodedFormulaChildModel list
            return result;
        }

        private double CompileValue(string src)
        {
            Expression e = new Expression(src);

            object result = e.Evaluate();

            if (result.GetType() == typeof(Int32))
                return src == "0" ? 0 : (int)e.Evaluate();


            return src == "0" ? 0 : (double)e.Evaluate();
        }

        private string CleanFormula(string frm)
        {
            //List<string> collection = this.GetParametersOnly(_currentFormula);

            //List<string> filteredList = collection.Where(c => _mlParameters.Any(a => a.parameter == c)).ToList();

            string strToReplace = _currentFormula;

            strToReplace = strToReplace.Replace("if([15003]<>0,LOOKUP([15003][17800][17900],STRUC_MTL,[15000],[15001],{ml_code}),", "")
                                       .Replace("if([15003]<>0,lookup([15003][17800],STRUC_HRS,[15000],[15001],{ML_CODE}),", "")
                                       .Replace("\"", "");

            strToReplace = strToReplace.Remove(strToReplace.Length - 1, 1);

            //foreach (var item in filteredList)
            //{
            //    var r = _mlParameters.Where(m => m.parameter == item).FirstOrDefault().value;

            //    strToReplace = strToReplace.Replace(item, r);
            //}

            _formulaLastState = strToReplace;

            return _formulaLastState;
        }

        #region Build methods

        /// <summary>
        /// Branch that evaluates If() parts of the formula
        /// </summary>
        /// <returns> A list of <see cref="XplodedFormulaChildModel"/></returns>
        private List<XplodedFormulaChildModel> EvaluateIfs(MLParameterChildBuilder childBuilder)
        {
            List<XplodedFormulaChildModel> childObjectList = new List<XplodedFormulaChildModel>();

            List<string> ifs = XplosionRegexFunctions.RunRegex(_formulaLastState, RegexType.BracketedIf);

            foreach (var item in ifs)
            {
                string itemStr = item;

                StringBuilder sbCode = new StringBuilder();
                StringBuilder sbVal = new StringBuilder();
                StringBuilder sbUom = new StringBuilder();
                StringBuilder sbDesc = new StringBuilder();

                foreach (var subItem in XplosionRegexFunctions.RunRegex(item, RegexType.ParametersOnly))
                {
                    sbCode.AppendLine(subItem);
                    sbVal.AppendLine(mlParameters.Where(m => m.parameter == XplosionRegexFunctions.RunRegex(subItem, RegexType.ParametersInBrackets).FirstOrDefault().ToString()).FirstOrDefault().value);
                    sbUom.AppendLine(mlParameters.Where(m => m.parameter == XplosionRegexFunctions.RunRegex(subItem, RegexType.ParametersInBrackets).FirstOrDefault().ToString()).FirstOrDefault().parameter_uom);
                    sbDesc.AppendLine(mlParameters.Where(m => m.parameter == XplosionRegexFunctions.RunRegex(subItem, RegexType.ParametersInBrackets).FirstOrDefault().ToString()).FirstOrDefault().parameter_desc);

                    itemStr = itemStr.Replace(subItem, mlParameters.Where(m => m.parameter == XplosionRegexFunctions.RunRegex(subItem, RegexType.ParametersInBrackets).FirstOrDefault().ToString()).FirstOrDefault().value);
                }

                var c = XplosionRegexFunctions.RunRegex(itemStr, RegexType.IfsConditionPart).FirstOrDefault().ToString().Replace(" ", "");

                childBuilder.AddChildCode(sbCode.ToString())
                            .AddChildValue(sbVal.ToString())
                            .AddChildUom(sbUom.ToString())
                            .AddChildDescription(sbDesc.ToString())
                            .AddChildOperation(itemStr)
                            .AddChildFormulaType(FormulaTypes.If);

                double r = 0;

                switch (c)
                {
                    case "if(YES=YES":
                        itemStr = itemStr.Replace("YES=YES", "1=1");
                        r = CompileValue(itemStr);
                        break;
                    case "if(HARP_BUNDLES=HARP_BUNDLES":
                        itemStr = itemStr.Replace("HARP_BUNDLES=HARP_BUNDLES", "1=1");
                        r = CompileValue(itemStr);
                        break;
                    default:
                        try
                        {
                            CompileValue(itemStr);
                            r = CompileValue(itemStr);
                        }
                        catch
                        {
                            itemStr = itemStr.Replace(c, "if(0=1");
                            r = CompileValue(itemStr);
                        }
                        break;
                }

                childBuilder.AddChildOperationResult(r);

                childObjectList.Add(childBuilder.Build());

                _formulaLastState = _formulaLastState.Replace("+" + item, "")
                    .Replace(item + "+", "")
                    .Replace(item, "");
            }

            return childObjectList;
        }

        /// <summary>
        /// Evaluates complex (nested brackets) parts of the formula
        /// </summary>
        /// <returns> A list of <see cref="XplodedFormulaChildModel"/></returns>
        private List<XplodedFormulaChildModel> ComplexBranch(MLParameterChildBuilder childBuilder)
        {
            List<XplodedFormulaChildModel> childObjectList = new List<XplodedFormulaChildModel>();

            // Part 1: Multiple parenthesis (a + b + c) * (d + e)
            foreach (var item in XplosionRegexFunctions.RunRegex(_formulaLastState, RegexType.ComplexMoreThanOneBrackets))
            {
                string itemStr = item;

                StringBuilder sbCode = new StringBuilder();
                StringBuilder sbVal = new StringBuilder();
                StringBuilder sbUom = new StringBuilder();
                StringBuilder sbDesc = new StringBuilder();

                foreach (var subItem in XplosionRegexFunctions.RunRegex(item, RegexType.ParametersOnly))
                {
                    sbCode.AppendLine(subItem);
                    sbVal.AppendLine(mlParameters.Where(m => m.parameter == XplosionRegexFunctions.RunRegex(subItem, RegexType.ParametersInBrackets).FirstOrDefault().ToString()).FirstOrDefault().value);
                    sbUom.AppendLine(mlParameters.Where(m => m.parameter == XplosionRegexFunctions.RunRegex(subItem, RegexType.ParametersInBrackets).FirstOrDefault().ToString()).FirstOrDefault().parameter_uom);
                    sbDesc.AppendLine(mlParameters.Where(m => m.parameter == XplosionRegexFunctions.RunRegex(subItem, RegexType.ParametersInBrackets).FirstOrDefault().ToString()).FirstOrDefault().parameter_desc);

                    itemStr = itemStr.Replace(subItem, mlParameters.Where(m => m.parameter == XplosionRegexFunctions.RunRegex(subItem, RegexType.ParametersInBrackets).FirstOrDefault().ToString()).FirstOrDefault().value);
                }

                childBuilder.AddChildCode(sbCode.ToString())
                            .AddChildValue(sbVal.ToString())
                            .AddChildUom(sbUom.ToString())
                            .AddChildDescription(sbDesc.ToString())
                            .AddChildOperation(itemStr)
                            .AddChildFormulaType(FormulaTypes.Complex)
                            .AddChildOperationResult(CompileValue(itemStr));

                childObjectList.Add(childBuilder.Build());

                _formulaLastState = _formulaLastState.Replace(item, "");
            }

            // Part 2: Simple parenthesis
            foreach (var item in XplosionRegexFunctions.RunRegex(_formulaLastState, RegexType.ComplexOnlyOneBracket))
            {

                string itemStr = item;

                StringBuilder sbCode = new StringBuilder();
                StringBuilder sbVal = new StringBuilder();
                StringBuilder sbUom = new StringBuilder();
                StringBuilder sbDesc = new StringBuilder();

                foreach (var subItem in XplosionRegexFunctions.RunRegex(item, RegexType.ParametersOnly))
                {
                    sbCode.AppendLine(subItem);
                    sbVal.AppendLine(mlParameters.Where(m => m.parameter == XplosionRegexFunctions.RunRegex(subItem, RegexType.ParametersInBrackets).FirstOrDefault().ToString()).FirstOrDefault().value);
                    sbUom.AppendLine(mlParameters.Where(m => m.parameter == XplosionRegexFunctions.RunRegex(subItem, RegexType.ParametersInBrackets).FirstOrDefault().ToString()).FirstOrDefault().parameter_uom);
                    sbDesc.AppendLine(mlParameters.Where(m => m.parameter == XplosionRegexFunctions.RunRegex(subItem, RegexType.ParametersInBrackets).FirstOrDefault().ToString()).FirstOrDefault().parameter_desc);

                    itemStr = itemStr.Replace(subItem, mlParameters.Where(m => m.parameter == XplosionRegexFunctions.RunRegex(subItem, RegexType.ParametersInBrackets).FirstOrDefault().ToString()).FirstOrDefault().value);
                }

                childBuilder.AddChildCode(sbCode.ToString())
                            .AddChildValue(sbVal.ToString())
                            .AddChildUom(sbUom.ToString())
                            .AddChildDescription(sbDesc.ToString())
                            .AddChildOperation(itemStr)
                            .AddChildFormulaType(FormulaTypes.Complex)
                            .AddChildOperationResult(CompileValue(itemStr));

                childObjectList.Add(childBuilder.Build());

                _formulaLastState = _formulaLastState.Replace(item, "");
            }

            return childObjectList;
        }

        /// <summary>
        /// Evaluates lookup parts of the formula
        /// TO Do: Do the evaluation part
        /// </summary>
        /// <param name="childBuilder"> Fluent function builder </param>
        /// <returns> A list of <see cref="XplodedFormulaChildModel"/> </returns>
        private List<XplodedFormulaChildModel> Lookups(MLParameterChildBuilder childBuilder)
        {
            List<XplodedFormulaChildModel> childObjectList = new List<XplodedFormulaChildModel>();

            List<string> lookups = XplosionRegexFunctions.RunRegex(_formulaLastState, RegexType.ComplexOnlyOneBracket);

            foreach (var item in lookups)
            {
                if (!item.ToLower().Contains("lookup"))
                    continue;

                string itemStr = item;

                StringBuilder sbCode = new StringBuilder();
                StringBuilder sbVal = new StringBuilder();
                StringBuilder sbUom = new StringBuilder();
                StringBuilder sbDesc = new StringBuilder();

                foreach (var subItem in XplosionRegexFunctions.RunRegex(item, RegexType.ParametersOnly))
                {
                    sbCode.AppendLine(subItem);
                    sbVal.AppendLine(mlParameters.Where(m => m.parameter == XplosionRegexFunctions.RunRegex(subItem, RegexType.ParametersInBrackets).FirstOrDefault().ToString()).FirstOrDefault().value);
                    sbUom.AppendLine(mlParameters.Where(m => m.parameter == XplosionRegexFunctions.RunRegex(subItem, RegexType.ParametersInBrackets).FirstOrDefault().ToString()).FirstOrDefault().parameter_uom);
                    sbDesc.AppendLine(mlParameters.Where(m => m.parameter == XplosionRegexFunctions.RunRegex(subItem, RegexType.ParametersInBrackets).FirstOrDefault().ToString()).FirstOrDefault().parameter_desc);

                    itemStr = itemStr.Replace(subItem, mlParameters.Where(m => m.parameter == XplosionRegexFunctions.RunRegex(subItem, RegexType.ParametersInBrackets).FirstOrDefault().ToString()).FirstOrDefault().value);
                }

                childBuilder.AddChildCode(sbCode.ToString())
                            .AddChildValue(sbVal.ToString())
                            .AddChildUom(sbUom.ToString())
                            .AddChildDescription(sbDesc.ToString())
                            .AddChildOperation(itemStr)
                            .AddChildFormulaType(FormulaTypes.Lookup)
                            // This needs to be modified later
                            .AddChildOperationResult(0);

                childObjectList.Add(childBuilder.Build());

                _formulaLastState = _formulaLastState.Replace("+" + item, "")
                                                     .Replace(item + "+", "")
                                                     .Replace(item, "");
            }

            return childObjectList;
        }

        private List<XplodedFormulaChildModel> SimpleBranch(MLParameterChildBuilder childBuilder)
        {
            // Initialize a new list of child objects
            List<XplodedFormulaChildModel> childObjectList = new List<XplodedFormulaChildModel>();

            // Gets the list of [parameters] existing in the formula
            List<string> collection = XplosionRegexFunctions.RunRegex(_formulaLastState, RegexType.ParametersOnly);

            // Filteres the previous collection to those, which have values in db
            List<string> filteredList = collection.Where(c => mlParameters.Any(a => a.parameter == c)).ToList();

            // Get the simple operations list
            List<string> op = XplosionRegexFunctions.RunRegex(_formulaLastState, RegexType.Operation);

            // Deserialize the formula string into objects
            foreach (var item in filteredList)
            {
                childBuilder.AddChildCode(mlParameters.Where(m => m.parameter == item).FirstOrDefault().parameter)
                            .AddChildValue(mlParameters.Where(m => m.parameter == item).FirstOrDefault().value)
                            .AddChildUom(mlParameters.Where(m => m.parameter == item).FirstOrDefault().parameter_uom)
                            .AddChildDescription(mlParameters.Where(m => m.parameter == item).FirstOrDefault().parameter_desc)
                            .AddChildFormulaType(FormulaTypes.Basic);

                foreach (var subItem in op)
                {
                    if (XplosionRegexFunctions.RunRegex(subItem, RegexType.ParametersInBrackets).FirstOrDefault() == item)
                    {
                        childBuilder.AddChildOperation(subItem);
                        childBuilder.AddChildOperationResult(CompileValue(subItem.Replace(item, mlParameters.Where(m => m.parameter == item).FirstOrDefault().value)));
                        op.Remove(subItem);
                        break;
                    }
                }

                childObjectList.Add(childBuilder.Build());
            }


            return childObjectList;
        }

        #endregion
    }
}
