public class ComboData
{
    public string Id { get; set; } = string.Empty;           // ���ʎq�i��FRyu_001�j
    public List<string> Tags { get; set; } = new();          // �^�O�i��F��Βn�A�[����Ȃǁj
    public int Damage { get; set; }                          // �_���[�W
    public string Starter { get; set; } = string.Empty;      // �n���Z�i��F���Ⴊ�ݒ�K�j
    public string Description { get; set; } = string.Empty;  // �R���{���e�i��F���Ⴊ�ݒ�K �� �g���� �� SA1�j
}