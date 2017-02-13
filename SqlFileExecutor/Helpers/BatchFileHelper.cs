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
        internal static bool HasTextToRun(string sqlStatement)
        {
            sqlStatement = Regex.Replace(sqlStatement, ResourceStrings.SplitPattern, string.Empty, RegexOptions.IgnoreCase | RegexOptions.Multiline).Replace(Environment.NewLine, string.Empty);

            if (string.IsNullOrWhiteSpace(sqlStatement))
            {
                return false;
            }

            return true;
        }

        internal static string ReplaceBatchSplitItems(Match matchedItem)
        {
            if (matchedItem.Groups[ResourceStrings.BatchGroupMarker].Success)
            {
                return $@"{matchedItem.Groups[ResourceStrings.LeftOfBatchGroupMarker].Value}{ResourceStrings.RemovalPattern}{matchedItem.Groups[ResourceStrings.RightOfBatchGroupMarker].Value}";
            }
            else
            {
                return $@"{matchedItem.Groups[ResourceStrings.LeftOfBatchGroupMarker].Value}{matchedItem.Groups[ResourceStrings.RightOfBatchGroupMarker].Value}";
            }
        }

        public static IEnumerable<string> GetBatches(string sqlToRun)
        {
            IList<string> batches = new List<string>();

            var markerRegex = new Regex(ResourceStrings.MarkerPattern, RegexOptions.IgnoreCase | RegexOptions.Multiline);

            var markedSql = markerRegex.Replace(sqlToRun, match => ReplaceBatchSplitItems(match));

            var parsedBatches = Regex.Split(markedSql, ResourceStrings.SplitPattern, RegexOptions.IgnoreCase | RegexOptions.Multiline);

            foreach (string batch in parsedBatches)
            {
                if (HasTextToRun(batch))
                {
                    batches.Add(batch);
                }
            }

            return batches;
        }
    }
}
