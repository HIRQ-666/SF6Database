public class CharacterData
{
    public string Name { get; set; } = string.Empty;

    public List<AttackData> Attacks { get; set; } = new();             // �Z�̏��
    public string Strategy { get; set; } = string.Empty;               // ��{�������
    public List<ComboData> Combos { get; set; } = new();               // �R���{
    public Dictionary<string, string> MatchupTips { get; set; } = new(); // �΃L�����΍�
}
