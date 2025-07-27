public class CharacterData
{
    public string Name { get; set; } = string.Empty;

    public List<AttackData> Attacks { get; set; } = new();             // 技の情報
    public List<StrategyPoint> Strategies { get; set; } = new();       // 基本立ち回り
    public List<ComboData> Combos { get; set; } = new();               // コンボ
    public List<MatchupData> Matchups { get; set; } = new();           // 対キャラ対策
}