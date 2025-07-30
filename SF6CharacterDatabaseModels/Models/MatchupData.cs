namespace SF6CharacterDatabaseModels.Models
{
    public class MatchupData
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();     // 一意なID（Guid）
        public string SelfCharacter { get; set; } = string.Empty;       // 自キャラ
        public string OpponentCharacter { get; set; } = string.Empty;   // 相手キャラ
        public string Title { get; set; } = string.Empty;               // 人間が識別しやすいタイトル
        public List<string> Tags { get; set; } = new();                 // タグ
        public string Content { get; set; } = string.Empty;             // 対策の詳細
    }
}
