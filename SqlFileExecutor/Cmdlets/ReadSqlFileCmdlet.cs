using System;
using System.Collections.Generic;
using System.IO;
using System.Management.Automation;
using SqlFileExecutor.Constants;
using SqlFileExecutor.Helpers;

namespace SqlFileExecutor.Cmdlets
{
    [Cmdlet("Read", "ReadSqlFile")]
    [OutputType(typeof(IEnumerable<string>))]
    public class ReadSqlFileCmdlet: PSCmdlet
    {
        [Parameter(Position =0)]
        [Alias("sql")]
        [ValidateNotNullOrEmpty]
        public string[] SqlFilePath { get; set; }

        private string LoadedSql;

        private IEnumerable<string> SqlBatchList;
        private List<string> PrasedSqlFilePaths = new List<string>();

        protected override void BeginProcessing()
        {
            base.BeginProcessing();
            try
            {
                foreach (var sqlFile in SqlFilePath)
                {
                    ProviderInfo provider;
                    PrasedSqlFilePaths.AddRange(GetResolvedProviderPathFromPSPath(sqlFile, out provider));
                }

                foreach (var sqlToLoad in SqlFilePath)
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
            SqlBatchList = BatchFileHelper.GetBatchStatementList(LoadedSql);
            WriteObject(SqlBatchList);
        }

    }
}
