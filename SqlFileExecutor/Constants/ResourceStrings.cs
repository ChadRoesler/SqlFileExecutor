namespace SqlFileExecutor.Constants
{
    internal class ResourceStrings
    {
        public const string SqlStrings = @"(?<KEEP1>'[^']*')";
        public const string DashComments = @"(?<KEEP1>--.*$)";
        public const string StarComments = @"(?<KEEP1>/\*[\S\s]*?\*/)";
        public const string Separator = @"(?<KEEP1>^|\s)(?<BATCHSPLITTER>GO)(?<KEEP2>\s|$)";
        public const string RegexFormat = "{0}|{1}|{2}|{3}";
        public const string RegexSplit = @"\|\{\[_REMOVE_\]\}\|";
        public const string BatchTerminatorReplacement = @" |{[_REMOVE_]}| ";
        public const string BatchSplitterGroup = "BATCHSPLITTER";
        public const string Keep1Group = "KEEP1";
        public const string Keep2Group = "KEEP2";
        public const string AppendedGo = "\r\nGO\r\n";
    }
}
