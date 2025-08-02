using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF6CharacterDatabaseModels.Models
{
    public class HitResults
    {
        public HitResult Normal { get; set; } = new();
        public HitResult Counter { get; set; } = new();
        public HitResult PunishCounter { get; set; } = new();
        public HitResult Guard { get; set; } = new();
    }
}
