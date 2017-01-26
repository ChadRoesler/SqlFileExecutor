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
        public string[] SqlFilePath { get; set; }

        private string LoadedSql = string.Empty;
        private List<string> PrasedSqlFilePaths = new List<string>();

        protected override void BeginProcessing()
        {
            base.BeginProcessing();
            try
            {
                SqlHelper.SqlConnectionTest(ConnectionString);
            }
            catch(Exception ex)
            {
                var errorExecuting = new ErrorRecord(ex, ErrorStrings.SqlConnectionError, ErrorCategory.InvalidData, ex.Source);
                ThrowTerminatingError(errorExecuting);
            }
            try
            {
                foreach(var sqlFile in SqlFilePath)
                {
                    ProviderInfo provider;
                    PrasedSqlFilePaths.AddRange(GetResolvedProviderPathFromPSPath(sqlFile, out provider));
                }

                foreach (var sqlToLoad in PrasedSqlFilePaths)
                {
                    LoadedSql += File.ReadAllText(sqlToLoad) + ResourceStrings.AppendedGo;
                }
            }
            catch (Exception ex)
            {
                var errorExecuting = new ErrorRecord(ex, ErrorStrings.FileError, ErrorCategory.InvalidData, ex.Source);
                ThrowTerminatingError(errorExecuting);
            }
        }

        protected override void ProcessRecord()
        {
            base.ProcessRecord();
            try
            {
                SqlHelper.SqlInfoExecutor(ConnectionString, LoadedSql);
            }
            catch (Exception ex)
            {
                var errorExecuting = new ErrorRecord(ex, ErrorStrings.SqlExecutionError, ErrorCategory.InvalidData, ConnectionString);
                WriteError(errorExecuting);
            }
        }
    }
}
