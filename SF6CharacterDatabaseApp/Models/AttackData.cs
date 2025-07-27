public class AttackData
{
    public string Name { get; set; } = string.Empty;            // �Z��
    public string Command { get; set; } = string.Empty;         // �R�}���h
    public CommandStrength CommandType { get; set; }            // �R�}���h���x

    public int StartFrame { get; set; }                         // �����t���[��
    public int ActiveFrame { get; set; }                        // �����t���[��
    public int StiffnessFrame { get; set; }                     // �d���t���[��
    public int GuardFrame { get; set; }                         // �K�[�h���t���[��
    public int HitFrame { get; set; }                           // �q�b�g���t���[��

    public CancelType CancelType { get; set; }                  // �L�����Z���^�C�v
    public int Damage { get; set; }                             // �_���[�W

    public float StartupCorrectionValue { get; set; }           // �n���␳�l
    public float ComboCorrectionValue { get; set; }             // �R���{�␳�l
    public float InstantCorrectionValue { get; set; }           // �����␳�l
    public float MultiplicationCorrectionValue { get; set; }    // ��Z�␳�l

    public int DriveGaugeIncrease { get; set; }                 // D�Q�[�W������
    public int DriveGaugeDecreaseGuard { get; set; }            // D�Q�[�W�����ʁi�K�[�h�j
    public int DriveGaugeDecreasePanish { get; set; }           // D�Q�[�W�����ʁi�p�j�b�V���J�E���^�[�j
    public int SAGaugeIncrease { get; set; }                    // SA�Q�[�W������

    public AttackAttribute AttackType { get; set; }             // �U���̑���
    public string Notes { get; set; } = string.Empty;           // ���l
}
