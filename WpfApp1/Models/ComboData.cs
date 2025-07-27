public class ComboData
{
    public string Id { get; set; } = string.Empty;           // 識別子
    public List<string> Tags { get; set; } = new();          // タグ
    public int Damage { get; set; }                          // ダメージ
    public string Starter { get; set; } = string.Empty;      // 始動技
    public string Description { get; set; } = string.Empty;  // コンボ内容
    public string Notes { get; set; } = string.Empty;        // 備考
}