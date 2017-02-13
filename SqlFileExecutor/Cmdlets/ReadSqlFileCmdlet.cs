using System;
using System.Collections.Generic;
using System.IO;
using System.Management.Automation;
using SqlFileExecutor.Constants;
using SqlFileExecutor.Helpers;

namespace SqlFileExecutor.Cmdlets
{
    [Cmdlet("Read", "SqlFile")]
    [OutputType(typeof(IEnumerable<string>))]
    public class ReadSqlFileCmdlet : PSCmdlet
    {
        [Parameter(Position = 0)]
        [Alias("sql")]
        [ValidateNotNullOrEmpty]
        public string[] Path { get; set; }

        private string LoadedSql;

        private IEnumerable<string> Batches;
        private List<string> ParsedPaths = new List<string>();

        protected override void BeginProcessing()
        {
            base.BeginProcessing();
            ProviderInfo provider;

            try
            {
                foreach (var sqlFile in Path)
                {
                    ParsedPaths.AddRange(GetResolvedProviderPathFromPSPath(sqlFile, out provider));
                }

                foreach (var sqlToLoad in ParsedPaths)
                {
                    LoadedSql += File.ReadAllText(sqlToLoad) + ResourceStrings.BatchTerminator;
                }
            }
            catch (Exception ex)
            {
                var errorExecuting = new ErrorRecord(ex, ErrorStrings.FileError, ErrorCategory.InvalidData, ex.Source);
                WriteError(errorExecuting);
            }

        }

        protected override void ProcessRecord()
        {
            base.ProcessRecord();
            try
            {
                Batches = BatchFileHelper.GetBatches(LoadedSql);
                WriteObject(Batches);
            }
            catch (Exception ex)
            {
                var errorExecuting = new ErrorRecord(ex, ErrorStrings.BatchParserError, ErrorCategory.InvalidData, ex.Source);
                WriteError(errorExecuting);
            }
        }

    }
}
