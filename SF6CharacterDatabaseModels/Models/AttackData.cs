using SF6CharacterDatabaseModels.Enums;

namespace SF6CharacterDatabaseModels.Models
{
    public class AttackData
    {
        public AttackCategory AttackCategory { get; set; } = AttackCategory.Other;        // �Z�̃J�e�S���i�ʏ�Z�A�K�E�Z�A�X�[�p�[�A�[�c�Ȃǁj
        public string Name { get; set; } = string.Empty;            // �Z��
        public string Command { get; set; } = string.Empty;         // �R�}���h
        public CommandStrength CommandType { get; set; }            // �R�}���h���x
        public CancelType CancelType { get; set; }                  // �L�����Z���^�C�v
        public int Damage { get; set; }                             // �_���[�W
        public FrameInfo FrameInfo { get; set; } = new();
        public HitResults HitResults { get; set; } = new();
        public CorrectionValues Corrections { get; set; } = new CorrectionValues();

        public int DriveGaugeIncrease { get; set; }                 // D�Q�[�W������
        public int DriveGaugeDecreaseGuard { get; set; }            // D�Q�[�W�����ʁi�K�[�h�j
        public int DriveGaugeDecreasePanish { get; set; }           // D�Q�[�W�����ʁi�p�j�b�V���J�E���^�[�j
        public int SAGaugeIncrease { get; set; }                    // SA�Q�[�W������

        public AttackAttribute AttackType { get; set; }             // �U���̑���
        public string Notes { get; set; } = string.Empty;           // ���l
    }
}
