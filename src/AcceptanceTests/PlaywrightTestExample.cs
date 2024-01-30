using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

namespace ProgrammingWithPalermo.ChurchBulletin.AcceptanceTests
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public class PlaywrightTestExample : PageTest
    {
        public override BrowserNewContextOptions ContextOptions()
        {
            return new BrowserNewContextOptions()
            {
                BaseURL = $"https://{Environment.GetEnvironmentVariable("containerAppURL", EnvironmentVariableTarget.User)}"
            };
        }

        [TestCase(1, 1)]
        [TestCase(2, 2)]
        [TestCase(5, 5)]
        [TestCase(9, 9)]
        public async Task Counter(int presses, int count)
        {
            // Arrange
            await Page.GotoAsync($"/counter");

            var button = Page.GetByRole(AriaRole.Button, new() { Name = "Click me" });

            // Act
            for (int i = 0; i < presses; i++)
            {
                await button.ClickAsync();
            }

            // Assert
            var totalCount = Page.GetByRole(AriaRole.Status);

            await Expect(totalCount).ToContainTextAsync($"{count}");
        }
    }
}
