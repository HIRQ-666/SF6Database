using HtmlAgilityPack;
using SF6CharacterDatabaseModels.Models;
using SF6CharacterDatabaseModels.Enums;
using System.Text.RegularExpressions;

namespace SF6DataFetcher.Parsers
{
    public static class FrameDataParser
    {
        public static List<AttackData> ParseFromHtml(string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            var result = new List<AttackData>();

            var rows = doc.DocumentNode.SelectNodes("//tr");
            if (rows == null) return result;

            foreach (var row in rows)
            {
                var cells = row.SelectNodes("td");
                if (cells == null || cells.Count < 15) continue;

                var attack = new AttackData
                {
                    Name = ExtractAttackName(cells[0]),
                    Command = ExtractCommandText(cells[0]),
                    StartFrame = ParseFrameSafe(cells[1].InnerText),
                    ActiveFrame = ParseFrameSafe(cells[2].InnerText),
                    StiffnessFrame = ParseFrameSafe(cells[3].InnerText),
                    AllFrame = ParseAllFrame(cells[3].InnerText),
                    HitFrame = ParseFrameSafe(cells[4].InnerText),
                    GuardFrame = ParseFrameSafe(cells[5].InnerText),
                    CancelType = ExtractCancelType(cells[6]),
                    Damage = ParseFrameSafe(cells[7].InnerText),
                    DriveGaugeIncrease = ParseFrameSafe(cells[9].InnerText),
                    DriveGaugeDecreaseGuard = ParseFrameSafe(cells[10].InnerText),
                    DriveGaugeDecreasePanish = ParseFrameSafe(cells[11].InnerText),
                    SAGaugeIncrease = ParseFrameSafe(cells[12].InnerText),
                    AttackType = ExtractAttackAttribute(cells[13]),
                    Notes = cells[14].InnerText.Trim()
                };

                ParseCorrectionValues(cells[8], attack);

                result.Add(attack);
            }

            return result;
        }

        private static string ExtractAttackName(HtmlNode cell)
        {
            var span = cell.SelectSingleNode(".//span[contains(@class, 'frame_arts')]");
            return span?.InnerText.Trim() ?? string.Empty;
        }

        private static string ExtractCommandText(HtmlNode cell)
        {
            var commandIcons = cell.SelectNodes(".//img");
            if (commandIcons == null || commandIcons.Count == 0) return string.Empty;
            return string.Join(" ", commandIcons.Select(img => img.GetAttributeValue("alt", "").Trim()));
        }

        private static CancelType ExtractCancelType(HtmlNode cell)
        {
            var span = cell.SelectSingleNode(".//span");
            if (span == null) return CancelType.None;

            return span.InnerText switch
            {
                "SA2" => CancelType.OnlySA2Or3,
                "SA3" => CancelType.OnlySA3,
                _ => CancelType.None,
            };
        }

        private static AttackAttribute ExtractAttackAttribute(HtmlNode cell)
        {
            var text = cell?.InnerText?.Trim() ?? string.Empty;
            if (text.Contains("投げ")) return AttackAttribute.Throw;
            if (text.Contains("弾")) return AttackAttribute.MidProjectile;
            if (text.Contains("上")) return AttackAttribute.High;
            if (text.Contains("中")) return AttackAttribute.Mid;
            if (text.Contains("下")) return AttackAttribute.Low;
            return AttackAttribute.Other;
        }

        private static void ParseCorrectionValues(HtmlNode correctionNode, AttackData attack)
        {
            var listItems = correctionNode.SelectNodes(".//li");
            if (listItems == null) return;

            foreach (var li in listItems)
            {
                var text = li.InnerText.Trim();
                if (text.StartsWith("始動補正"))
                    attack.StartupCorrectionValue = ParsePercentage(text);
                else if (text.StartsWith("コンボ補正"))
                    attack.ComboCorrectionValue = ParsePercentage(text);
                else if (text.StartsWith("即時補正"))
                    attack.InstantCorrectionValue = ParsePercentage(text);
                else if (text.StartsWith("乗算補正"))
                    attack.MultiplicationCorrectionValue = ParsePercentage(text);
            }
        }

        private static float ParsePercentage(string text)
        {
            var match = Regex.Match(text, @"(\d+)%");
            if (match.Success && float.TryParse(match.Groups[1].Value, out float percent))
                return percent / 100f;
            return 0f;
        }

        private static int ParseFrameSafe(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return -1;
            text = text.Replace(" ", "").Replace("F", "").Trim();
            return int.TryParse(text, out int value) ? value : -1;
        }

        private static int ParseAllFrame(string text)
        {
            var match = Regex.Match(text, @"全体\s*(\d+)");
            if (match.Success && int.TryParse(match.Groups[1].Value, out int value))
                return value;
            return -1;
        }
    }
}
