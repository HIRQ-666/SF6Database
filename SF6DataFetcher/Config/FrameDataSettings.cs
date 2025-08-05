namespace SF6DataFetcher.Config
{
    public class FrameDataSettings
    {
        public bool UseLocalHtml { get; set; }
        public string CharacterMappingsFile { get; set; } = "";
        public string CharacterUrlTemplate { get; set; } = "";
        public string DebugHtmlDirectory { get; set; } = "";
        public string OutputJsonDirectory { get; set; } = "";
        public string DebugHtmlFileFormat { get; set; } = "debug_{characterName}_framearea.html";
        public string OutputJsonFileFormat { get; set; } = "attackdata_{characterName}.json";
        public string CommandMappingCsvPath { get; set; } = "";
        public string AttackIdMapsPath { get; set; } = "";

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
