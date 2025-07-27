public class StrategyPoint
{
    public string Id { get; set; } = Guid.NewGuid().ToString();   // 一意な識別子
    public string SelfCharacter { get; set; } = string.Empty;     // 自キャラ名（例：Ryu）
    public string Title { get; set; } = string.Empty;             // タイトル（例：端の立ち回り）
    public List<string> Tags { get; set; } = new();               // タグ（例：端、対空など）
    public string Content { get; set; } = string.Empty;           // 内容
}