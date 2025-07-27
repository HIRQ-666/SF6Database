public class AttackData
{
    public string Name { get; set; } = string.Empty;            // 技名
    public string Command { get; set; } = string.Empty;         // コマンド
    public CommandStrength CommandType { get; set; }            // コマンド強度

    public int StartFrame { get; set; }                         // 発生フレーム
    public int ActiveFrame { get; set; }                        // 持続フレーム
    public int StiffnessFrame { get; set; }                     // 硬直フレーム
    public int GuardFrame { get; set; }                         // ガード時フレーム
    public int HitFrame { get; set; }                           // ヒット時フレーム

    public CancelType CancelType { get; set; }                  // キャンセルタイプ
    public int Damage { get; set; }                             // ダメージ

    public float StartupCorrectionValue { get; set; }           // 始動補正値
    public float ComboCorrectionValue { get; set; }             // コンボ補正値
    public float InstantCorrectionValue { get; set; }           // 即時補正値
    public float MultiplicationCorrectionValue { get; set; }    // 乗算補正値

    public int DriveGaugeIncrease { get; set; }                 // Dゲージ増加量
    public int DriveGaugeDecreaseGuard { get; set; }            // Dゲージ減少量（ガード）
    public int DriveGaugeDecreasePanish { get; set; }           // Dゲージ減少量（パニッシュカウンター）
    public int SAGaugeIncrease { get; set; }                    // SAゲージ増加量

    public AttackAttribute AttackType { get; set; }             // 攻撃の属性
    public string Notes { get; set; } = string.Empty;           // 備考
}
