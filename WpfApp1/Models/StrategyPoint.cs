public class StrategyPoint
{
    public string Id { get; set; } = Guid.NewGuid().ToString();   // ��ӂȎ��ʎq
    public string SelfCharacter { get; set; } = string.Empty;     // ���L�������i��FRyu�j
    public string Title { get; set; } = string.Empty;             // �^�C�g���i��F�[�̗������j
    public List<string> Tags { get; set; } = new();               // �^�O�i��F�[�A�΋�Ȃǁj
    public string Content { get; set; } = string.Empty;           // ���e
}