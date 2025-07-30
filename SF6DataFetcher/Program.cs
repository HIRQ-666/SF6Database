using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Playwright;
using SF6DataFetcher.Parsers;
using SF6CharacterDatabaseModels.Models;
using System.Text.Json;

class Program
{
    public static async Task Main()
    {
        using var playwright = await Playwright.CreateAsync();
        var browser = await playwright.Chromium.LaunchAsync(new() { Headless = false });

        var context = await browser.NewContextAsync(new()
        {
            Locale = "ja-JP",
            TimezoneId = "Asia/Tokyo",
            ViewportSize = new() { Width = 1280, Height = 800 },
            UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/115.0.0.0 Safari/537.36"
        });

        var page = await context.NewPageAsync();

        Console.WriteLine("ページを読み込み中...");
        await page.GotoAsync("https://www.streetfighter.com/6/ja-jp/character/ryu/frame", new() { Timeout = 60000 });

        // ページ読み込み後、frameareaを待機
        Console.WriteLine("frameareaを探しています...");
        await page.WaitForSelectorAsync("#framearea", new() { Timeout = 10000 });

        // HTML取得
        var frameArea = await page.QuerySelectorAsync("#framearea");
        if (frameArea == null)
        {
            Console.WriteLine("❌ frameareaが見つかりませんでした。");
            return;
        }

        string innerHtml = await frameArea.InnerHTMLAsync();

        // 保存（デバッグ用）
        File.WriteAllText("debug_framearea.html", innerHtml);

        // パース実行
        var attacks = FrameDataParser.ParseFromHtml(innerHtml);

        // 結果を表示
        Console.WriteLine($"技データ数: {attacks.Count}");
        foreach (var atk in attacks)
        {
            Console.WriteLine($"技名: {atk.Name}, 発生: {atk.StartFrame}, ダメージ: {atk.Damage}");
        }

        // JSON保存
        string json = JsonSerializer.Serialize(attacks, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText("attackdata.json", json);

        await browser.CloseAsync();
    }
}
