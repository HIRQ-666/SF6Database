using SF6CharacterDatabaseModels.Models;
using System.Text.Json;

public class AttackIdMapManager
{
    private readonly string _characterName;
    private readonly string _filePath;
    private readonly Dictionary<string, string> _map;

    public AttackIdMapManager(string characterName, string? basePath = null)
    {
        _characterName = characterName;

        // basePath を外部から受け取り、なければ AppContext.BaseDirectory を使用
        string rootPath = basePath ?? Path.Combine(AppContext.BaseDirectory, "SF6CharacterDatabaseModels");
        string directory = Path.Combine(rootPath, "AttackIdMaps");

        if (!Directory.Exists(directory))
        {
            Console.WriteLine($"[DEBUG] Directory does not exist. Creating: {directory}");
            Directory.CreateDirectory(directory);
        }

        _filePath = Path.Combine(directory, $"{_characterName}_idmap.json");

        if (File.Exists(_filePath))
        {
            var json = File.ReadAllText(_filePath);
            var entries = JsonSerializer.Deserialize<List<AttackIdMapEntry>>(json) ?? new List<AttackIdMapEntry>();
            _map = entries.ToDictionary(e => e.Name, e => e.AttackId);
        }
        else
        {
            _map = new Dictionary<string, string>();
            Save();
            Console.WriteLine($"[DEBUG] Created empty JSON file: {_filePath}");
        }
    }

    public string GetOrCreateId(string attackName)
    {
        if (_map.TryGetValue(attackName, out var id))
        {
            return id;
        }

        string newId = Guid.NewGuid().ToString();
        _map[attackName] = newId;
        return newId;
    }

    public void Save()
    {
        var entries = _map.Select(kvp => new AttackIdMapEntry { Name = kvp.Key, AttackId = kvp.Value }).ToList();
        var json = JsonSerializer.Serialize(entries, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_filePath, json);
    }
}
