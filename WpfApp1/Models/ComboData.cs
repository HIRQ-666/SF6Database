public class ComboData
{
    public string Id { get; set; } = string.Empty;           // ���ʎq
    public List<string> Tags { get; set; } = new();          // �^�O
    public int Damage { get; set; }                          // �_���[�W
    public string Starter { get; set; } = string.Empty;      // �n���Z
    public string Description { get; set; } = string.Empty;  // �R���{���e
    public string Notes { get; set; } = string.Empty;        // ���l
}