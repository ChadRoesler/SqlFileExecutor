namespace SqlFileExecutor.Constants
{
    public class ErrorStrings
    {
        public const string ErrorText = "\r\nError:\r\n\t{0}\r\nConnection String:\r\n\t{1}\r\nSQL Executed:\r\n{2}\r\n";
        public const string SqlConnectionError = "Error Connecting to ServerInstance/Database";
        public const string FileError = "Error Loading File";
        public const string SqlExecutionError = "Error Loading File";
        public const string BatchParserError = "Error Parsing Batched Sql";
    }
}
