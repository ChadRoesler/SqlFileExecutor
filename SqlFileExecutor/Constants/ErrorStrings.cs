namespace SqlFileExecutor.Constants
{
    internal class ErrorStrings
    {
        internal static readonly string ErrorText = $"{System.Environment.NewLine}Error:{System.Environment.NewLine}   {{0}}{System.Environment.NewLine}Connection String:{System.Environment.NewLine}   {{1}}{System.Environment.NewLine}SQL Executed:{System.Environment.NewLine}{{2}}{System.Environment.NewLine}";
        internal const string SqlConnectionError = "Error Connecting to ServerInstance/Database.";
        internal const string FileError = "Error Loading File.";
        internal const string SqlExecutionError = "Error Loading File.";
        internal const string BatchParserError = "Error Parsing Batched Sql.";
    }
}
