public class ComboData
{
    public string Id { get; set; } = string.Empty;           // 識別子（例：Ryu_001）
    public List<string> Tags { get; set; } = new();          // タグ（例：空対地、端限定など）
    public int Damage { get; set; }                          // ダメージ
    public string Starter { get; set; } = string.Empty;      // 始動技（例：しゃがみ中K）
    public string Description { get; set; } = string.Empty;  // コンボ内容（例：しゃがみ中K → 波動拳 → SA1）
}