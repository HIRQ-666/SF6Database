using SF6CharacterDatabaseModels.Enums;
using System;
using System.Text;

namespace SF6CharacterDatabaseModels.Models
{
    public class AttackData
    {
        public string AttackId { get; set; } = Guid.NewGuid().ToString();                 // 技のID
        public AttackCategory AttackCategory { get; set; } = AttackCategory.Other;        // 技のカテゴリ（通常技、必殺技、スーパーアーツなど）
        public string Name { get; set; } = string.Empty;                                  // 技名
        public string Command { get; set; } = string.Empty;                               // コマンド
        public string CommandNote { get; set; } = string.Empty;                           // コマンドの備考
        public CommandStrength CommandType { get; set; } = CommandStrength.None;          // コマンド強度
        public CancelType CancelType { get; set; } = CancelType.None;                     // キャンセルタイプ
        public int Damage { get; set; }                                                   // ダメージ
        public FrameInfo FrameInfo { get; set; } = new();                                 // フレーム情報
        public HitResults HitResults { get; set; } = new();                               // ヒット結果
        public CorrectionValues Corrections { get; set; } = new();                        // 補正情報
        public GaugeEffect GaugeEffect { get; set; } = new();                             // ゲージへの影響
        public AttackAttribute AttackType { get; set; } = AttackAttribute.Other;          // 攻撃の属性
        public string Notes { get; set; } = string.Empty;                                 // 備考
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;                      // 最終更新日時  

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Id: {AttackId}");
            sb.AppendLine($"Name: {Name}");
            sb.AppendLine($"Command: {Command}");
            sb.AppendLine($"Category: {AttackCategory}");
            sb.AppendLine($"CommandType: {CommandType}");
            sb.AppendLine($"CancelType: {CancelType}");
            sb.AppendLine($"Damage: {Damage}");
            sb.AppendLine($"AttackType: {AttackType}");
            sb.AppendLine($"Notes: {Notes}");

            sb.AppendLine($"[FrameInfo]");
            sb.AppendLine($"  Start: {FrameInfo.Start}");
            sb.AppendLine($"  Active: {FrameInfo.Active}");
            sb.AppendLine($"  Stiffness: {FrameInfo.Stiffness}");
            sb.AppendLine($"  All: {FrameInfo.All}");

            sb.AppendLine($"[HitResults]");
            sb.AppendLine($"  Normal: {HitResults.Normal.Frame}F ({HitResults.Normal.Effect})");
            sb.AppendLine($"  Counter: {HitResults.Counter.Frame}F ({HitResults.Counter.Effect})");
            sb.AppendLine($"  Punish: {HitResults.PunishCounter.Frame}F ({HitResults.PunishCounter.Effect})");
            sb.AppendLine($"  Guard: {HitResults.Guard}F");

            sb.AppendLine($"[Corrections]");
            sb.AppendLine($"  Startup: {Corrections.Startup * 100}%");
            sb.AppendLine($"  Combo: {Corrections.Combo * 100}%");
            sb.AppendLine($"  Instant: {Corrections.Instant * 100}%");
            sb.AppendLine($"  Multiplication: {Corrections.Multiplication * 100}%");

            sb.AppendLine($"[GaugeEffect]");
            sb.AppendLine($"  Drive+ : {GaugeEffect.DriveIncrease}");
            sb.AppendLine($"  Drive-Guard : {GaugeEffect.DriveDecreaseGuard}");
            sb.AppendLine($"  Drive-Punish : {GaugeEffect.DriveDecreasePanish}");
            sb.AppendLine($"  SA+ : {GaugeEffect.SAIncrease}");

            return sb.ToString();
        }
    }
}
