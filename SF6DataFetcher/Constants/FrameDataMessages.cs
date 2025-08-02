namespace SF6DataFetcher.Constants
{
    public static class FrameDataMessages
    {
        public const string InitPlaywright = "✅ Playwright 初期化中...";
        public const string LoadingPage = "🌐 ページを読み込み中...";
        public const string UsingLocalHtml = "🧪 ローカルHTMLファイルを読み込み中...";
        public const string LocalHtmlNotFound = "❌ ローカルHTMLファイルが見つかりません: ";
        public const string HtmlLoaded = "✅ HTMLを正常に読み込みました。";
        public const string WaitingFrameArea = "🔎 framearea テーブル待機中...";
        public const string FrameAreaNotFound = "❌ framearea が見つかりませんでした。";
        public const string ParsingFrameData = "📋 フレームデータをパース中...";
        public const string OutputJsonSaved = "✅ JSON ファイルを保存しました: ";
        public const string AttackCount = "✅ 技データ数: ";
        public const string StartupTime = "🕒 起動時間: ";
        public const string CharacterMapNotFound = "❌ キャラマッピングファイルが見つかりません: ";
        public const string CharacterMapEmptyOrInvalid = "❌ キャラマッピングが空、または読み込みに失敗しました。";
        public const string CharacterCount = "👥 対象キャラ数: {0}";
        public const string StartCharacterProcessing = "🚀 {0}（{1}）のデータ処理を開始します...";
        public const string CharacterHtmlSaved = "💾 HTMLを保存しました: ";
        public const string AllCharacterProcessingDone = "✅ すべてのキャラクターのデータ取得が完了しました。";
    }
}
