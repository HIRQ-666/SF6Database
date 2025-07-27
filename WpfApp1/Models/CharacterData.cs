public class CharacterData
{
    public string Name { get; set; } = string.Empty;

    public List<AttackData> Attacks { get; set; } = new();             // 技の情報
    public string Strategy { get; set; } = string.Empty;               // 基本立ち回り
    public List<ComboData> Combos { get; set; } = new();               // コンボ
    public Dictionary<string, string> MatchupTips { get; set; } = new(); // 対キャラ対策
}
