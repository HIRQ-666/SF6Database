using HtmlAgilityPack;
using SF6CharacterDatabaseModels.Models;
using SF6CharacterDatabaseModels.Utilities;

namespace SF6DataFetcher.Parsers
{
    public static class FrameDataParser
    {
        public static List<AttackData> ParseFrameDataFromHtml(
            string html,
            CommandMapper commandMapper,
            DateTime startTime,
            AttackIdMapManager idMapManager)
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
                    Name = FrameCellParser.ParseAttackName(cells[0]),
                    Command = FrameCellParser.ParseCommandFromIcons(cells[0], commandMapper),
                    CancelType = FrameCellParser.ParseCancelType(cells[6]),
                    Damage = FrameCellParser.ParseFrameValue(cells[7].InnerText),
                    AttackType = FrameCellParser.ParseAttackAttribute(cells[13]),
                    Notes = cells[14].InnerText.Trim()
                };

                // 技名からCommandNote抽出
                Console.WriteLine($"[DEBUG] AttackName: {attack.Name}");
                string innerText = cells[0].InnerText;
                if (!string.IsNullOrWhiteSpace(attack.Name) && innerText.Contains(attack.Name))
                {
                    innerText = innerText.Replace(attack.Name, "").Trim();
                }
                attack.CommandNote = FrameCellParser.ExtractNotesWithParentheses(innerText);

                // AttackIdの割り当て
                attack.AttackId = idMapManager.GetOrCreateId(attack.Name);

                // FrameInfo構築
                int startFrame = FrameCellParser.ParseFrameValue(cells[1].InnerText);
                int activeFrame = FrameCellParser.ParseActiveFrame(cells[2].InnerText, startFrame);
                int stiffnessFrame = FrameCellParser.ParseFrameValue(cells[3].InnerText);
                int allFrame = FrameCellParser.ParseAllFrame(
                    cells[3].InnerText,
                    startFrame,
                    activeFrame,
                    stiffnessFrame
                );

                attack.FrameInfo = new FrameInfo
                {
                    Start = startFrame,
                    Active = activeFrame,
                    Stiffness = stiffnessFrame,
                    All = allFrame
                };

                // ヒットフレーム
                HitResult normalHit = FrameCellParser.ParseHitResult(cells[4].InnerText);
                HitResult counterHit = new HitResult(
                    frame: normalHit.Frame != -999 ? normalHit.Frame + 2 : -999,
                    effect: normalHit.Effect
                );
                HitResult punishCounterHit = new HitResult(
                    frame: normalHit.Frame != -999 ? normalHit.Frame + 4 : -999,
                    effect: normalHit.Effect
                );
                HitResult guardHit = new HitResult(
                    frame: FrameCellParser.ParseFrameValue(cells[5].InnerText),
                    effect: 0
                );

                attack.HitResults = new HitResults
                {
                    Normal = normalHit,
                    Counter = counterHit,
                    PunishCounter = punishCounterHit,
                    Guard = guardHit
                };

                // 補正値
                attack.Corrections = FrameCellParser.ParseCorrectionValues(cells[8]);

                // ゲージ影響
                attack.GaugeEffect = new GaugeEffect
                {
                    DriveIncrease = FrameCellParser.ParseFrameValue(cells[9].InnerText),
                    DriveDecreaseGuard = FrameCellParser.ParseFrameValue(cells[10].InnerText),
                    DriveDecreasePanish = FrameCellParser.ParseFrameValue(cells[11].InnerText),
                    SAIncrease = FrameCellParser.ParseFrameValue(cells[12].InnerText)
                };

                attack.LastUpdated = startTime;

                result.Add(attack);
            }

            return result;
        }
    }
}
