namespace SF6CharacterDatabaseModels.Models
{
    public class AttackIdMapEntry
    {
        public string Name { get; set; } = string.Empty;     // 技名
        public string AttackId { get; set; } = string.Empty; // 一意なID（Guid）
    }

}
