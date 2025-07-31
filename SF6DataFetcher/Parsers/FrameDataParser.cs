using HtmlAgilityPack;
using SF6CharacterDatabaseModels.Models;

namespace SF6DataFetcher.Parsers
{
    public static class FrameDataParser
    {
        public static List<AttackData> ParseFrameDataFromHtml(string html)
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
                    Command = FrameCellParser.ParseCommandFromIcons(cells[0]),
                    StartFrame = FrameCellParser.ParseFrameValue(cells[1].InnerText),
                    ActiveFrame = FrameCellParser.ParseFrameValue(cells[2].InnerText),
                    StiffnessFrame = FrameCellParser.ParseFrameValue(cells[3].InnerText),
                    AllFrame = FrameCellParser.ParseAllFrameValue(cells[3].InnerText),
                    HitFrame = FrameCellParser.ParseFrameValue(cells[4].InnerText),
                    GuardFrame = FrameCellParser.ParseFrameValue(cells[5].InnerText),
                    CancelType = FrameCellParser.ParseCancelType(cells[6]),
                    Damage = FrameCellParser.ParseFrameValue(cells[7].InnerText),
                    DriveGaugeIncrease = FrameCellParser.ParseFrameValue(cells[9].InnerText),
                    DriveGaugeDecreaseGuard = FrameCellParser.ParseFrameValue(cells[10].InnerText),
                    DriveGaugeDecreasePanish = FrameCellParser.ParseFrameValue(cells[11].InnerText),
                    SAGaugeIncrease = FrameCellParser.ParseFrameValue(cells[12].InnerText),
                    AttackType = FrameCellParser.ParseAttackAttribute(cells[13]),
                    Notes = cells[14].InnerText.Trim()
                };

                FrameCellParser.ParseCorrectionValues(cells[8], attack);
                result.Add(attack);
            }

            return result;
        }
    }
}
