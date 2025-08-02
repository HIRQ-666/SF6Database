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

        public static string ParseCommandFromIcons(HtmlNode cell, CommandMapper commandMapper)
        {
            var icons = cell.SelectNodes(".//img");
            if (icons == null || icons.Count == 0) return string.Empty;

            return string.Join(" ", icons.Select(img =>
            {
                var src = img.GetAttributeValue("src", "").Trim();
                var filename = Path.GetFileName(src); // ファイル名だけ取得
                return commandMapper.GetSymbolByImageName(filename);
            }));
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

        public static void ParseCorrectionValues(HtmlNode node, AttackData attack)
        {
            var listItems = node.SelectNodes(".//li");
            if (listItems == null) return;

            foreach (var li in listItems)
            {
                var text = li.InnerText.Trim();

                if (text.StartsWith(FrameParseConstants.StartupCorrectionLabel))
                    attack.StartupCorrectionValue = ParsePercentage(text);
                else if (text.StartsWith(FrameParseConstants.ComboCorrectionLabel))
                    attack.ComboCorrectionValue = ParsePercentage(text);
                else if (text.StartsWith(FrameParseConstants.InstantCorrectionLabel))
                    attack.InstantCorrectionValue = ParsePercentage(text);
                else if (text.StartsWith(FrameParseConstants.MultiplicationCorrectionLabel))
                    attack.MultiplicationCorrectionValue = ParsePercentage(text);
            }
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

        public static int ParseActiveFrame(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return 0;  // 弾などで空欄の場合

            text = text.Replace("F", "").Trim();

            // 数字だけをすべて抽出
            var matches = Regex.Matches(text, @"\d+").Cast<Match>().Select(m => int.Parse(m.Value)).ToList();

            if (matches.Count == 0)
                return 0;

            if (matches.Count == 1)
                return 1; // 1値のみ（例: "3"） → 最低1F持続とみなす

            int min = matches.Min();
            int max = matches.Max();
            return max - (min - 1);  // 例: 5 - (3 - 1) = 3
        }

        public static int ParseAllFrameValue(string text)
        {
            var match = Regex.Match(text, @"全体\s*(\d+)");
            return match.Success && int.TryParse(match.Groups[1].Value, out int value) ? value : -999;
        }

        public static HitResult ParseHitResult(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return new HitResult(); // Frame = -999, Effect = None

            text = text.Trim();

            if (text.Contains("※") || text.Contains("D") || text.Contains("ダウン"))
            {
                // ダウンだがフレーム数があるかをチェック
                var frameMatch = Regex.Match(text, @"\d+");
                int frame = frameMatch.Success ? int.Parse(frameMatch.Value) : -999;
                return new HitResult(frame, HitEffectType.Down); // または HardKnockDown に切り替え可能
            }

            // フレーム数のみ
            if (int.TryParse(text.Replace("F", "").Replace("ｆ", "").Trim(), out int value))
                return new HitResult(value, HitEffectType.None);

            return new HitResult(); // 解析失敗
        }
    }
}
