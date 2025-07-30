namespace SF6CharacterDatabaseModels.Models
{
    public class MatchupData
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();     // ��ӂ�ID�iGuid�j
        public string SelfCharacter { get; set; } = string.Empty;       // ���L����
        public string OpponentCharacter { get; set; } = string.Empty;   // ����L����
        public string Title { get; set; } = string.Empty;               // �l�Ԃ����ʂ��₷���^�C�g��
        public List<string> Tags { get; set; } = new();                 // �^�O
        public string Content { get; set; } = string.Empty;             // �΍�̏ڍ�
    }
}
