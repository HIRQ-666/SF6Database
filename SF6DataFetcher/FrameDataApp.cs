using Microsoft.Extensions.Configuration;
using Microsoft.Playwright;
using SF6CharacterDatabaseModels.Models;
using SF6CharacterDatabaseModels.Utilities;
using SF6DataFetcher.Config;
using SF6DataFetcher.Constants;
using SF6DataFetcher.Parsers;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace SF6DataFetcher
{
    public class FrameDataApp
    {
        private readonly FrameDataSettings _settings;

        public FrameDataApp()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            _settings = config.GetRequiredSection("FrameData").Get<FrameDataSettings>()!;
        }

        public async Task RunAsync()
        {
            Console.WriteLine(FrameDataMessages.InitPlaywright);
            using var playwright = await Playwright.CreateAsync();
            var browser = await playwright.Chromium.LaunchAsync(new() { Headless = false });

            var page = await CreateConfiguredPageAsync(browser);

            Console.WriteLine(FrameDataMessages.LoadingPage);
            await page.GotoAsync(_settings.CharacterUrl, new() { Timeout = _settings.GotoTimeout });

            Console.WriteLine(FrameDataMessages.WaitingFrameArea);
            await page.WaitForSelectorAsync("#framearea table", new() { Timeout = _settings.WaitForSelectorTimeout });
            await Task.Delay(_settings.ExtraWaitMilliseconds);

            string innerHtml = await ExtractHtmlFromFrameAreaAsync(page);
            if (string.IsNullOrWhiteSpace(innerHtml))
            {
                Console.WriteLine(FrameDataMessages.FrameAreaNotFound);
                return;
            }

            File.WriteAllText(_settings.DebugHtmlFile, innerHtml);

            Console.WriteLine(FrameDataMessages.ParsingFrameData);

            var commandMapper = new CommandMapper(_settings.CommandMappingCsvPath);
            var attacks = FrameDataParser.ParseFrameDataFromHtml(innerHtml, commandMapper);

            Console.WriteLine($"{FrameDataMessages.AttackCount}{attacks.Count}");

            await SaveJsonAsync(attacks, _settings.OutputJsonFile);

            await browser.CloseAsync();
        }

        private async Task<IPage> CreateConfiguredPageAsync(IBrowser browser)
        {
            var context = await browser.NewContextAsync(new()
            {
                Locale = _settings.Locale,
                TimezoneId = _settings.TimezoneId,
                ViewportSize = new() { Width = _settings.ViewportWidth, Height = _settings.ViewportHeight },
                UserAgent = _settings.UserAgent
            });

            return await context.NewPageAsync();
        }

        private async Task<string> ExtractHtmlFromFrameAreaAsync(IPage page)
        {
            var frameArea = await page.QuerySelectorAsync("#framearea");
            return frameArea != null ? await frameArea.InnerHTMLAsync() : string.Empty;
        }

        private async Task SaveJsonAsync(List<AttackData> data, string path)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
            var json = JsonSerializer.Serialize(data, options);
            await File.WriteAllTextAsync(path, json);
            Console.WriteLine($"{FrameDataMessages.OutputJsonSaved}{path}");
        }
    }
}
