using SF6CharacterDatabaseModels.Shared;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SF6CharacterDatabaseModels.Utilities
{
    public class CommandMapper
    {
        private readonly Dictionary<string, CommandMapping> _mappingByImage;

        public CommandMapper(string csvPath)
        {
            _mappingByImage = File.ReadAllLines(csvPath)
                .Skip(1) // ヘッダーをスキップ
                .Select(line => line.Split(','))
                .Where(cols => cols.Length >= 4)
                .Select(cols => new CommandMapping
                {
                    Name = cols[0].Trim(),
                    ImageName = cols[1].Trim(),
                    Symbol = cols[2].Trim(),
                    TenKey = cols[3].Trim()
                })
                .ToDictionary(m => m.ImageName, m => m);

            foreach (var kv in _mappingByImage)
            {
                Console.WriteLine($"[マッピング] {kv.Key} => {kv.Value.Symbol}");
            }
        }

        public string GetSymbol(string imageName)
        {
            return _mappingByImage.TryGetValue(imageName, out var mapping)
                ? mapping.Symbol
                : imageName; // fallback
        }

        public string GetTenKey(string imageName)
        {
            return _mappingByImage.TryGetValue(imageName, out var mapping)
                ? mapping.TenKey
                : imageName;
        }

        public bool TryGetMapping(string imageName, out CommandMapping? mapping)
        {
            return _mappingByImage.TryGetValue(imageName, out mapping);
        }

        public string GetSymbolByImageName(string imageName)
        {
            return _mappingByImage.TryGetValue(imageName, out var mapping) ? mapping.Symbol : imageName;
        }
    }
}
