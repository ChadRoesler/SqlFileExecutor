using System;
using System.Collections.Generic;
using System.IO;
using System.Management.Automation;
using SqlFileExecutor.Constants;
using SqlFileExecutor.Helpers;

namespace SqlFileExecutor.Cmdlets
{
    [Cmdlet("Invoke", "SqlFile")]
    public class SqlFileCmdlet : PSCmdlet
    {
        [Parameter(Position = 0)]
        [Alias("cs")]
        [ValidateNotNullOrEmpty]
        public string ConnectionString { get; set; }

        [Parameter(Position = 1)]
        [Alias("sql")]
        [ValidateNotNullOrEmpty]
        public string[] Path { get; set; }

        [Parameter(Position = 2)]
        [Alias("ops")]
        public SwitchParameter OutputPrintStatements
        {
            get
            {
                return OutputPrint;
            }
            set
            {
                OutputPrint = value;
            }
        }

        private bool OutputPrint;
        private string LoadedSql = string.Empty;
        private List<string> ParsedPaths = new List<string>();

        protected override void BeginProcessing()
        {
            base.BeginProcessing();
            ProviderInfo provider;

            try
            {
                SqlHelper.SqlConnectionTest(ConnectionString);
            }
            catch (Exception ex)
            {
                var errorExecuting = new ErrorRecord(ex, ErrorStrings.SqlConnectionError, ErrorCategory.InvalidData, ex.Source);
                WriteError(errorExecuting);
            }

            try
            {
                foreach (var sqlFile in Path)
                {
                    ParsedPaths.AddRange(GetResolvedProviderPathFromPSPath(sqlFile, out provider));
                }

                foreach (var sqlToLoad in ParsedPaths)
                {
                    LoadedSql += File.ReadAllText(sqlToLoad) + ResourceStrings.FormattedBatchTerminator;
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
                var infoText = SqlHelper.SqlInfoExecutor(ConnectionString, LoadedSql);
                if (OutputPrint)
                {
                    WriteObject(infoText);
                }
            }
            catch (Exception ex)
            {
                var errorExecuting = new ErrorRecord(ex, ErrorStrings.SqlExecutionError, ErrorCategory.InvalidData, ConnectionString);
                WriteError(errorExecuting);
            }
        }
    }
}
