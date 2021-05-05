using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NCalc;

namespace Xplosion.Core.FrameworkLibrary
{
    public static class XplosionRegexFunctions
    {
        private const string COMPLEX_MORE_THAN_1_BRACKET = @"\((?>\((?<c>)|[^()]+|\)(?<-c>))*(?(c)(?!))\)([^+]\((?>\((?<c>)|[^()]+|\)(?<-c>))*(?(c)(?!))\)?)";
        private const string COMPLEX_SINGLE_BRACKET = @"[^+]*\((?>\((?<c>)|[^()]+|\)(?<-c>))*(?(c)(?!))\)[^+]*";
        private const string BRACKETED_IF = @"if\((?>\((?<c>)|[^()]+|\)(?<-c>))*(?(c)(?!))\)";
        private const string LOOKUPS = @"lookup\((?>\((?<c>)|[^()]+|\)(?<-c>))*(?(c)(?!))\)";
        private const string PARAMETERS_IN_BRACKETS = @"((?:(?:\((?:\[[^\]]*\]\+?)+\))|(?:\[[^\]]*\])))";
        private const string OPERATION = @"(((\((\[[^\]]*\]\+?)+\))|(\[[^\]]*\]))([*/](\([^)]*\))|[*/]([^+]*)))";
        private const string PARAMETERS_ONLY = @"(\[[^\]]*\])";
        private const string IFS_CONDITION_PART = @"(if\()([^,]*)(?=\,[^,]*\,[^,)]*\))";

        public static List<string> RunRegex(string source, RegexType rt)
        {
            switch (rt)
            {
                case RegexType.ComplexMoreThanOneBrackets:
                    return RegexConvertList(source, s => new Regex(COMPLEX_MORE_THAN_1_BRACKET, RegexOptions.IgnoreCase).Matches(s));
                case RegexType.ComplexOnlyOneBracket:
                    return RegexConvertList(source, s => new Regex(COMPLEX_SINGLE_BRACKET, RegexOptions.IgnoreCase).Matches(s));
                case RegexType.BracketedIf:
                    return RegexConvertList(source, s => new Regex(BRACKETED_IF, RegexOptions.IgnoreCase).Matches(s));
                case RegexType.Lookups:
                    return RegexConvertList(source, s => new Regex(LOOKUPS, RegexOptions.IgnoreCase).Matches(s));
                case RegexType.ParametersInBrackets:
                    return RegexConvertList(source, s => new Regex(PARAMETERS_IN_BRACKETS, RegexOptions.None).Matches(s));
                case RegexType.Operation:
                    return RegexConvertList(source, s => new Regex(OPERATION).Matches(s));
                case RegexType.ParametersOnly:
                    return RegexConvertList(source, s => new Regex(PARAMETERS_ONLY).Matches(s));
                case RegexType.IfsConditionPart:
                    return RegexConvertList(source, s => new Regex(IFS_CONDITION_PART, RegexOptions.IgnoreCase).Matches(s));
                default:
                    return new List<string>();

            }
        }
            /// <summary>
            /// Static method which converts MatchCollection to List<>
            /// </summary> 
            /// <param name="source"> The original string pattern on which the regex is applied</param>
            /// <param name="func"> Delegate method of Regex.Matches()</param>
            /// <returns></returns>
            public static List<string> RegexConvertList(string source, Func<string, MatchCollection> func)
            {
                // Returns the calling method' MatchCollection converted to List
                return func(source).Cast<Match>().Select(m => m.Value).ToList();
            }

            public static double CompileValue(this string src)
            {
                Expression e = new Expression(src);

                object result = e.Evaluate();

                if (result.GetType() == typeof(Int32))
                    return src == "0" ? 0 : (int)e.Evaluate();


                return src == "0" ? 0 : (double)e.Evaluate();
            }
        
    }
}
