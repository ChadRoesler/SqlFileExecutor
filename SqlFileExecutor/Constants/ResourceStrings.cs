namespace SqlFileExecutor.Constants
{
    internal sealed class ResourceStrings
    {
        public const string LeftOfBatchGroupMarker = "LEFT";
        public const string RightOfBatchGroupMarker = "RIGHT";
        public const string BatchGroupMarker = "BATCHGROUP";
        public const string RemovalMarker = "_REMOVE_";
        public const string BatchTerminator = "GO";

        public readonly static string FormattedBatchTerminator = $@"{System.Environment.NewLine}{BatchTerminator}{System.Environment.NewLine}";
        public readonly static string SqlStringPattern = $@"(?<{LeftOfBatchGroupMarker}>'[^']*')";
        public readonly static string SingleLineCommentsPattern = $@"(?<{LeftOfBatchGroupMarker}>--.*$)";
        public readonly static string BlockCommentPattern = $@"(?<{LeftOfBatchGroupMarker}>/\*[\S\s]*?\*/)";
        public readonly static string BatchPattern = $@"(?<{LeftOfBatchGroupMarker}>^|\s)(?<{BatchGroupMarker}>{BatchTerminator})(?<{RightOfBatchGroupMarker}>\s|$)";
        public readonly static string SplitPattern = $@"\|\{{\[{RemovalMarker}\]\}}\|";
        public readonly static string RemovalPattern = $@" |{{[{RemovalMarker}]}}| ";
        public readonly static string MarkerPattern = $@"{SqlStringPattern}|{SingleLineCommentsPattern}|{BlockCommentPattern }|{BatchPattern}";
    }
}