namespace SF6DataFetcher.Config
{
    public class FrameDataSettings
    {
        public string CharacterUrl { get; set; } = "";
        public string DebugHtmlFile { get; set; } = "";
        public string OutputJsonFile { get; set; } = "";
        public string CommandMappingCsvPath { get; set; } = "";

        public int GotoTimeout { get; set; }
        public int WaitForSelectorTimeout { get; set; }
        public int ExtraWaitMilliseconds { get; set; }

        public string UserAgent { get; set; } = "";
        public string Locale { get; set; } = "";
        public string TimezoneId { get; set; } = "";
        public int ViewportWidth { get; set; }
        public int ViewportHeight { get; set; }
    }

}
