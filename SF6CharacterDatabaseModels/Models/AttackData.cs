using SF6CharacterDatabaseModels.Enums;
using System;
using System.Text;

namespace SF6CharacterDatabaseModels.Models
{
    public class AttackData
    {
        public string AttackId { get; set; } = Guid.NewGuid().ToString();                 // �Z��ID
        public AttackCategory AttackCategory { get; set; } = AttackCategory.Other;        // �Z�̃J�e�S���i�ʏ�Z�A�K�E�Z�A�X�[�p�[�A�[�c�Ȃǁj
        public string Name { get; set; } = string.Empty;                                  // �Z��
        public string Command { get; set; } = string.Empty;                               // �R�}���h
        public string CommandNote { get; set; } = string.Empty;                           // �R�}���h�̔��l
        public CommandStrength CommandType { get; set; } = CommandStrength.None;          // �R�}���h���x
        public CancelType CancelType { get; set; } = CancelType.None;                     // �L�����Z���^�C�v
        public int Damage { get; set; }                                                   // �_���[�W
        public FrameInfo FrameInfo { get; set; } = new();                                 // �t���[�����
        public HitResults HitResults { get; set; } = new();                               // �q�b�g����
        public CorrectionValues Corrections { get; set; } = new();                        // �␳���
        public GaugeEffect GaugeEffect { get; set; } = new();                             // �Q�[�W�ւ̉e��
        public AttackAttribute AttackType { get; set; } = AttackAttribute.Other;          // �U���̑���
        public string Notes { get; set; } = string.Empty;                                 // ���l
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;                      // �ŏI�X�V����  

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
