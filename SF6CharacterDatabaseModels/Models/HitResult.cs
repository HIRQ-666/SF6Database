using SF6CharacterDatabaseModels.Enums;

namespace SF6CharacterDatabaseModels.Models
{
    public class HitResult
    {
        public int Frame { get; set; } = -999;
        public HitEffectType Effect { get; set; } = HitEffectType.None;

        public HitResult(int frame = -999, HitEffectType effect = HitEffectType.None)
        {
            Frame = frame;
            Effect = effect;
        }
    }
}
