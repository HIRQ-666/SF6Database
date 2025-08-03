using HtmlAgilityPack;
using SF6CharacterDatabaseModels.Enums;
using SF6CharacterDatabaseModels.Models;
using SF6CharacterDatabaseModels.Utilities;
using System.Text.RegularExpressions;


namespace SF6DataFetcher.Parsers
{
    public static class FrameCellParser
    {
        public static string ParseAttackName(HtmlNode cell)
        {
            var span = cell.SelectSingleNode($".//span[contains(@class, '{FrameParseConstants.FrameArtsClass}')]");
            return span?.InnerText.Trim() ?? string.Empty;
        }

        public static string ExtractNotesWithParentheses(string innerText)
        {
            var notes = new List<string>();
            int depth = 0;
            int start = -1;

            for (int i = 0; i < innerText.Length; i++)
            {
                if (innerText[i] == '（')
                {
                    if (depth == 0) start = i;
                    depth++;
                }
                else if (innerText[i] == '）')
                {
                    depth--;
                    if (depth == 0 && start != -1)
                    {
                        string note = innerText.Substring(start, i - start + 1);
                        notes.Add(note.Trim());
                        start = -1;
                    }
                }
            }

            var result = string.Join(",", notes);
            return result;
        }

        public static string ParseCommandFromIcons(HtmlNode cell, CommandMapper mapper)
        {
            string innerText = cell.InnerText?.Trim() ?? "";

            var icons = cell.SelectNodes(".//img");
            if (icons == null || icons.Count == 0)
            {
                Console.WriteLine("[DEBUG] No icons found.");
                return "";
            }

            var commandSymbols = icons.Select(img =>
            {
                var src = img.GetAttributeValue("src", "");
                var fileName = Path.GetFileName(src);
                var symbol = mapper.GetSymbolByImageName(fileName);
                return symbol;
            });

            string command = string.Join(" ", commandSymbols);

            return command;
        }



        public static CancelType ParseCancelType(HtmlNode cell)
        {
            var span = cell.SelectSingleNode(".//span");
            var text = span?.InnerText?.Trim() ?? "";

            if (text.Contains("※"))
            {
                return CancelType.SpecificMoveOnly;
            }

            return text switch
            {
                FrameParseConstants.CancelLabel_Able => CancelType.Normal,
                FrameParseConstants.CancelLabel_SA => CancelType.OnlySA,
                FrameParseConstants.CancelLabel_SA2 => CancelType.OnlySA2Or3,
                FrameParseConstants.CancelLabel_SA3 => CancelType.OnlySA3,
                _ => CancelType.None
            };
        }

        public static AttackAttribute ParseAttackAttribute(HtmlNode cell)
        {
            var text = cell?.InnerText?.Trim() ?? string.Empty;
            bool isProjectile = text.Contains("弾");

            if (text.Contains("投げ")) return AttackAttribute.Throw;
            if (text.Contains("上")) return isProjectile ? AttackAttribute.HighProjectile : AttackAttribute.High;
            if (text.Contains("中")) return isProjectile ? AttackAttribute.MidProjectile : AttackAttribute.Mid;
            if (text.Contains("下")) return isProjectile ? AttackAttribute.LowProjectile : AttackAttribute.Low;

            return AttackAttribute.Other;
        }

        public static float ParsePercentage(string text)
        {
            var match = Regex.Match(text, @"(\d+)%");
            return match.Success && float.TryParse(match.Groups[1].Value, out float value) ? value / 100f : 0f;
        }

        public static int ParseFrameValue(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return -1;

            text = text.Replace(" ", "").Replace("F", "").Trim();
            return int.TryParse(text, out int value) ? value : -999;
        }

        public static int ParseActiveFrame(string text, int start)
        {
            if (string.IsNullOrWhiteSpace(text))
                return -999;

            if (text.Contains("着地"))
                return -999;

            // 記号を除去（例: ※や*）
            text = text.Replace("※", "").Replace("*", "").Trim();

            // マッチ: 数値として解釈可能なパターンのみ（先頭0つきの誤認識防止）
            var matches = Regex.Matches(text, @"\d+");

            // 異常データチェック
            if (matches.Count == 0) return -999;

            // 数字のうち最大のものを選ぶ（例: 3(5)2 → Max=5）
            var numbers = matches
                .Select(m => int.TryParse(m.Value, out int v) ? v : -1)
                .Where(v => v >= 0 && v < 100)  // 100以上の異常値を除外（任意）
                .ToList();

            if (numbers.Count == 0) return -999;

            int max = numbers.Max();
            return max - start + 1;
        }

        public static int ParseAllFrame(string frameText, int start, int active, int stiffness)
        {
            if (string.IsNullOrWhiteSpace(frameText))
                return start + active + stiffness - 1;

            // 着地が含まれる場合は複雑なので無効値とする
            if (frameText.Contains("着地"))
                return -999;

            // 「全体◯F」のような記載があればその数値を優先
            var match = Regex.Match(frameText, @"全体\s*(\d+)");
            if (match.Success && int.TryParse(match.Groups[1].Value, out int all))
                return all;

            // それ以外は自前で計算
            return start + active + stiffness - 1;
        }

        public static HitResult ParseHitResult(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return new HitResult();

            text = text.Trim();

            if (text.Contains("※") || text.Contains("D") || text.Contains("ダウン"))
            {
                var frameMatch = Regex.Match(text, @"\d+");
                int frame = frameMatch.Success ? int.Parse(frameMatch.Value) : -999;
                return new HitResult(frame, HitEffectType.Down);
            }

            if (int.TryParse(text.Replace("F", "").Replace("ｆ", "").Trim(), out int value))
                return new HitResult(value, HitEffectType.None);

            return new HitResult();
        }

        public static CorrectionValues ParseCorrectionValues(HtmlNode node)
        {
            CorrectionValues corrections = new CorrectionValues();
            var listItems = node.SelectNodes(".//li");
            if (listItems == null) return corrections;

            foreach (var li in listItems)
            {
                var text = li.InnerText.Trim();
                if (listItems == null) return corrections;

                if (text.StartsWith(FrameParseConstants.StartupCorrectionLabel))
                    corrections.Startup = ParsePercentage(text);
                else if (text.StartsWith(FrameParseConstants.ComboCorrectionLabel))
                    corrections.Combo = ParsePercentage(text);
                else if (text.StartsWith(FrameParseConstants.InstantCorrectionLabel))
                    corrections.Instant = ParsePercentage(text);
                else if (text.StartsWith(FrameParseConstants.MultiplicationCorrectionLabel))
                    corrections.Multiplication = ParsePercentage(text);
            }

            return corrections;
        }
    }
}
