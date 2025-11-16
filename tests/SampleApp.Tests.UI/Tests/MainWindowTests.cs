using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Support.UI;
using SampleApp.Tests.UI.Drivers;
using SampleApp.Tests.UI.Pages;
using System;
using System.IO;

namespace SampleApp.Tests.UI.Tests
{
    [TestClass]
    public class MainWindowTests
    {
        private static MainPage _page;
        private static WindowsDriver<WindowsElement> _driver;

        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            // CI では環境変数 SAMPLEAPP_PATH を優先
#if DEBUG
            string solutionDir = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\"));
            string appPath = Path.Combine(solutionDir, @"src\SampleApp\bin\Debug\SampleApp.exe");
#else
            string appPath = Environment.GetEnvironmentVariable("SAMPLEAPP_PATH")
                             ?? @"..\..\..\..\src\SampleApp\bin\Debug\SampleApp.exe";
#endif

            string winAppDriverPath = Environment.GetEnvironmentVariable("WINAPPDRIVER_PATH")
                                         ?? Path.Combine(
                                               Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
                                               "Windows Application Driver",
                                               "WinAppDriver.exe");

            if (!File.Exists(winAppDriverPath))
            {
                throw new FileNotFoundException($"WinAppDriver が見つかりません: {winAppDriverPath}. 環境変数 WINAPPDRIVER_PATH を確認してください。");
            }

            _driver = WinAppDriverSession.Start(appPath, winAppDriverPath);
            _page = new MainPage(_driver);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            WinAppDriverSession.Quit();
        }

        [TestMethod]
        public void ボタンクリック時に結果テキストが更新されることをテスト()
        {
            var text = _page.ResultText.Text;

            // ボタンをクリックしてテキストが変わることを確認
            _page.ClickButton.Click();

            // UI 更新待ち（タイムアウト=5秒）
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));
            wait.Until(driver =>
            {
                return !_page.ResultText.Text.Equals(text);
            });

            // 結果の検証
            Assert.AreEqual("Button clicked!", _page.ResultText.Text);
        }
    }
}
