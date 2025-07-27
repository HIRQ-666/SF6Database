public class CharacterData
{
    public string Name { get; set; } = string.Empty;

    public List<AttackData> Attacks { get; set; } = new();             // �Z�̏��
    public List<StrategyPoint> Strategies { get; set; } = new();       // ��{�������
    public List<ComboData> Combos { get; set; } = new();               // �R���{
    public List<MatchupData> Matchups { get; set; } = new();           // �΃L�����΍�
}