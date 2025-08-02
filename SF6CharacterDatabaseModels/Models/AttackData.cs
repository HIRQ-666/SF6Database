using SF6CharacterDatabaseModels.Enums;

namespace SF6CharacterDatabaseModels.Models
{
    public class AttackData
    {
        public AttackCategory AttackCategory { get; set; } = AttackCategory.Other;        // 技のカテゴリ（通常技、必殺技、スーパーアーツなど）
        public string Name { get; set; } = string.Empty;            // 技名
        public string Command { get; set; } = string.Empty;         // コマンド
        public CommandStrength CommandType { get; set; }            // コマンド強度
        public CancelType CancelType { get; set; }                  // キャンセルタイプ
        public int Damage { get; set; }                             // ダメージ
        public FrameInfo FrameInfo { get; set; } = new();
        public HitResults HitResults { get; set; } = new();
        public CorrectionValues Corrections { get; set; } = new CorrectionValues();

        public int DriveGaugeIncrease { get; set; }                 // Dゲージ増加量
        public int DriveGaugeDecreaseGuard { get; set; }            // Dゲージ減少量（ガード）
        public int DriveGaugeDecreasePanish { get; set; }           // Dゲージ減少量（パニッシュカウンター）
        public int SAGaugeIncrease { get; set; }                    // SAゲージ増加量

        public AttackAttribute AttackType { get; set; }             // 攻撃の属性
        public string Notes { get; set; } = string.Empty;           // 備考
    }
}
