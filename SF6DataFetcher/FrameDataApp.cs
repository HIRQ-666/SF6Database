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
        private DateTime _startTime = DateTime.UtcNow.ToLocalTime();

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
            Console.WriteLine($"{FrameDataMessages.StartupTime}{_startTime:yyyy-MM-dd HH:mm:ss} JST");

            // キャラマッピングの読み込み
            if (!File.Exists(_settings.CharacterMappingsFile))
            {
                Console.WriteLine($"{FrameDataMessages.CharacterMapNotFound}{_settings.CharacterMappingsFile}");
                return;
            }

            var characterMapJson = File.ReadAllText(_settings.CharacterMappingsFile);
            var characterMap = JsonSerializer.Deserialize<Dictionary<string, string>>(characterMapJson);

            if (characterMap == null || characterMap.Count == 0)
            {
                Console.WriteLine(FrameDataMessages.CharacterMapEmptyOrInvalid);
                return;
            }

            Console.WriteLine(string.Format(FrameDataMessages.CharacterCount, characterMap.Count));

            foreach (var kvp in characterMap)
            {
                string characterName = kvp.Key;
                string urlName = kvp.Value;

                Console.WriteLine(string.Format(FrameDataMessages.StartCharacterProcessing, characterName, urlName));

                string url = _settings.CharacterUrlTemplate.Replace("{urlName}", urlName);
                string debugHtmlPath = Path.Combine(_settings.DebugHtmlDirectory, _settings.DebugHtmlFileFormat.Replace("{characterName}", characterName));
                string outputJsonPath = Path.Combine(_settings.OutputJsonDirectory, _settings.OutputJsonFileFormat.Replace("{characterName}", characterName));

                string innerHtml;

                if (_settings.UseLocalHtml)
                {
                    Console.WriteLine(FrameDataMessages.UsingLocalHtml);

                    if (!File.Exists(debugHtmlPath))
                    {
                        Console.WriteLine($"{FrameDataMessages.LocalHtmlNotFound}{debugHtmlPath}");
                        continue;
                    }

                    innerHtml = File.ReadAllText(debugHtmlPath);
                }
                else
                {
                    Console.WriteLine(FrameDataMessages.InitPlaywright);
                    using var playwright = await Playwright.CreateAsync();
                    var browser = await playwright.Chromium.LaunchAsync(new() { Headless = false });

                    var page = await CreateConfiguredPageAsync(browser);

                    Console.WriteLine(FrameDataMessages.LoadingPage);
                    await page.GotoAsync(url, new() { Timeout = _settings.GotoTimeout });

                    Console.WriteLine(FrameDataMessages.WaitingFrameArea);
                    await page.WaitForSelectorAsync("#framearea table", new() { Timeout = _settings.WaitForSelectorTimeout });
                    await Task.Delay(_settings.ExtraWaitMilliseconds);

                    innerHtml = await ExtractHtmlFromFrameAreaAsync(page);
                    if (string.IsNullOrWhiteSpace(innerHtml))
                    {
                        Console.WriteLine(FrameDataMessages.FrameAreaNotFound);
                        continue;
                    }

                    Directory.CreateDirectory(Path.GetDirectoryName(debugHtmlPath)!);
                    File.WriteAllText(debugHtmlPath, innerHtml);
                    Console.WriteLine($"{FrameDataMessages.CharacterHtmlSaved}{debugHtmlPath}");

                    await browser.CloseAsync();
                }

                Console.WriteLine(FrameDataMessages.HtmlLoaded);
                Console.WriteLine(FrameDataMessages.ParsingFrameData);

                var commandMapper = new CommandMapper(_settings.CommandMappingCsvPath);
                var attacks = FrameDataParser.ParseFrameDataFromHtml(innerHtml, commandMapper, _startTime);

                Console.WriteLine($"{FrameDataMessages.AttackCount}{attacks.Count}");

                await SaveJsonAsync(attacks, outputJsonPath);
            }

            Console.WriteLine(FrameDataMessages.AllCharacterProcessingDone);
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
            Directory.CreateDirectory(Path.GetDirectoryName(path)!);

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
