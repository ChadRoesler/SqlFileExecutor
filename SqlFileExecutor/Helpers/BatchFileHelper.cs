using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using SqlFileExecutor.Constants;

namespace SqlFileExecutor.Helpers
{
    /// <summary>
    /// Refactored from: https://github.com/chucknorris/roundhouse
    /// </summary>
    public static class BatchFileHelper
    {
        internal static bool ValidateHasTextToRun(string sqlStatement, string seperatorRegexPattern)
        {
            sqlStatement = Regex.Replace(sqlStatement, ResourceStrings.RegexSplit, string.Empty, RegexOptions.IgnoreCase | RegexOptions.Multiline);
            return !string.IsNullOrWhiteSpace(sqlStatement);
        }

        internal static string ReplaceBatchSplitItems(Match matchedItem, Regex regex)
        {
            if(matchedItem.Groups[ResourceStrings.BatchSplitterGroup].Success)
            {
                return matchedItem.Groups[ResourceStrings.Keep1Group].Value + ResourceStrings.BatchTerminatorReplacement + matchedItem.Groups[ResourceStrings.Keep2Group].Value;
            }
            else
            {
                return matchedItem.Groups[ResourceStrings.Keep1Group].Value + matchedItem.Groups[ResourceStrings.Keep2Group].Value;
            }
        }

        public static IEnumerable<string> GetBatchStatementList(string sqlToRun)
        {
            IList<string> statementList = new List<string>();

            var regexPattern = string.Format(ResourceStrings.RegexFormat, ResourceStrings.SqlStrings, ResourceStrings.DashComments, ResourceStrings.StarComments, ResourceStrings.Separator);

            var regexReplace = new Regex(regexPattern, RegexOptions.IgnoreCase | RegexOptions.Multiline);

            var scrubbedSQLStatement = regexReplace.Replace(sqlToRun, match => ReplaceBatchSplitItems(match, regexReplace));

            var regexSplit = Regex.Split(scrubbedSQLStatement, ResourceStrings.RegexSplit, RegexOptions.IgnoreCase | RegexOptions.Multiline);

            foreach (string sqlStatement in regexSplit)
            {
                if (ValidateHasTextToRun(sqlStatement, regexPattern))
                {
                    statementList.Add(sqlStatement);
                }
            }
            return statementList;
        }
    }
}
