using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using org.mariuszgromada.math.mxparser;
using Xplosion.Relational;

namespace Xplosion.Core
{
    public class BaseFormula
    {
        #region Private memebers
        private string _currentFormula { get; set; }

        private string _formulaLastState { get; set; }

        private List<MlParameters> _mlParameters { get; set; }

        private MLFormulaParentObject Mfp { get; set; }

        #endregion

        /// <summary>
        /// Default constructor
        /// TODO: Services constructor injection
        /// </summary>
        public BaseFormula()
        {
            HrsgDataModel hdm = new HrsgDataModel();

            _currentFormula = hdm.GetFormulaList().FirstOrDefault().formula;

            Mfp = new MLFormulaParentObject()
            {
                Formula = hdm.GetFormulaList().FirstOrDefault().formula,
                Code = hdm.GetFormulaList().FirstOrDefault().ml_code,
                Description = hdm.GetFormulaList().FirstOrDefault().ml_description,
                Value = hdm.GetFormulaList().FirstOrDefault().formula_value
                
            };

            _mlParameters = hdm.GetParameterList();

            Mfp.Formula = CleanFormula(Mfp.Formula);

            Mfp.ChildrenCollection = BuildChildParameterObjects();
        }

        private List<MLParameterChildObject> BuildChildParameterObjects()
        {
            List<string> collection = this.GetParametersOnly(_formulaLastState);

            List<string> filteredList = collection.Where(c => _mlParameters.Any(a => a.parameter == c)).ToList();

            List<string> op = GetOperation(_currentFormula);

            List<MLParameterChildObject> result = new List<MLParameterChildObject>();

            foreach (var item in filteredList)
            {
                MLParameterChildObject r = new MLParameterChildObject()
                {
                    Code = _mlParameters.Where(m => m.parameter == item).FirstOrDefault().parameter,
                    Value = _mlParameters.Where(m => m.parameter == item).FirstOrDefault().value,
                    Description = _mlParameters.Where(m => m.parameter == item).FirstOrDefault().parameter_desc,
                    UnitOfMeasure = _mlParameters.Where(m => m.parameter == item).FirstOrDefault().parameter_uom,
                    Parent = Mfp
                };

                foreach (var subItem in op)
                {
                    if(GetParametersInBrackets(subItem).FirstOrDefault() == item)
                    {
                        r.Operation = subItem;
                        op.Remove(subItem);
                        break;
                    }
                }

                result.Add(r);
            }

            foreach (MLParameterChildObject item in result)
            {
                if (item.Operation != null)
                {
                    var r = _mlParameters.Where(m => m.parameter == item.Code).FirstOrDefault().value;

                    item.Operation = item.Operation.Replace(item.Code, r);
                }

                item.OperationResult = CompileValue(item.Operation);
            }

            return result;
        }

        private double CompileValue(string src)
        {
            Expression e = new Expression(src);
            return (double)e.calculate();
            //var val = new DataTable().Compute(src, "");
            
            //return (double)val;
        }

        private string CleanFormula(string frm)
        {
            //List<string> collection = this.GetParametersOnly(_currentFormula);

            //List<string> filteredList = collection.Where(c => _mlParameters.Any(a => a.parameter == c)).ToList();

            string strToReplace = _currentFormula;

            strToReplace = strToReplace.Replace("if([15003]<>0,LOOKUP([15003][17800][17900],STRUC_MTL,[15000],[15001],{ml_code}),", "")
                                       .Replace("if([15003]<>0,lookup([15003][17800],STRUC_HRS,[15000],[15001],{ML_CODE}),","")
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

        #region Regex helpers

        /// <summary>
        /// Gets only the parameters, if grouped in brackets, those too
        /// </summary>
        /// <param name="source"> String to extract from </param>
        /// <returns >List of strings with all parameters, grouped in brackets too </returns>
        public List<string> GetParametersInBrackets(string source)
        {
           
            Regex pattern = new Regex(@"((?:(?:\((?:\[[^\]]*\]\+?)+\))|(?:\[[^\]]*\])))", RegexOptions.None);

            return pattern.Matches(source).Cast<Match>().Select(m => m.Value).ToList();
        }

        public List<string> GetOperation(string source)
        {

            Regex pattern = new Regex(@"((?:(?:\((?:\[[^\]]*\]\+?)+\))|(?:\[[^\]]*\]))(?:[*/](?:\([^)]*\))|[*/](?:[^+]*)))", RegexOptions.None);

            return pattern.Matches(source).Cast<Match>().Select(m => m.Value).ToList();
        }

        public List<string> GetParametersOnly(string source)
        {

            Regex pattern = new Regex(@"(\[[^\]]*\])", RegexOptions.None);

            return pattern.Matches(source).Cast<Match>().Select(m => m.Value).ToList();
        }

        public List<string> GetBracketedExpressions(string source)
        {

            Regex pattern = new Regex(@"(\((?:[^()]+)*\)(?:[\*\/][^+]*))", RegexOptions.None);

            return pattern.Matches(source).Cast<Match>().Select(m => m.Value).ToList();
        }

        public List<string> GetIfs(string source)
        {

            Regex pattern = new Regex(@"(if\((?:[^\)]*)\))", RegexOptions.None);

            return pattern.Matches(source).Cast<Match>().Select(m => m.Value).ToList();
        }

        public string GetIfsConditions(string source, bool part)
        {
            Regex pattern = new Regex(@"(\([^,]*,)([^,]*),([^,)]*)\)", RegexOptions.None);

           return  part ? pattern.Match(source).Groups[2].Value :  pattern.Match(source).Groups[3].Value;

        }

        public Match GetIfsConditionPart(string source)
        {

            Regex pattern = new Regex(@"(?:if\()([^,]*)(?=\,[^,]*\,[^,)]*\))", RegexOptions.None);

            GroupCollection g = pattern.Match(source).Groups;

            return pattern.Match(source);
        }

        #endregion 
        public List<string> EvaluteIfs()
        {
            List<string> ifs = this.GetIfs(_formulaLastState);

            List<string> result = new List<string>();

            foreach (var item in ifs)
            {
                Match c = GetIfsConditionPart(item);

                if (c.ToString().Replace(" ", "") == "if(YES=YES")
                {
                    result.Add(GetIfsConditions(item, true));

                } else
                {
                    result.Add(GetIfsConditions(item, false));
                }
            }

            return ifs;
        }
    }
}
