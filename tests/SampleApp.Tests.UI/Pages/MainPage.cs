using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium;

namespace SampleApp.Tests.UI.Pages
{
    public class MainPage
    {
        private readonly WindowsDriver<WindowsElement> _driver;

        public MainPage(WindowsDriver<WindowsElement> driver)
        {
            _driver = driver;
        }

        public WindowsElement TitleText =>
            _driver.FindElementByAccessibilityId("TitleText");

        public WindowsElement ClickButton =>
            _driver.FindElementByAccessibilityId("ClickButton");

        public WindowsElement ResultText =>
            _driver.FindElementByAccessibilityId("ResultText");

        // メモ
        // ・ItemsControl内の要素を探す場合は以下のようにAutomationIdを指定すると良い
        //  <TextBlock AutomationProperties.AutomationId="{Binding Index, StringFormat=Item_{0}}"/>
        // ・4つ目をクリックする場合:
        //    var item4 = _driver.FindElementByAccessibilityId("Item_3");
        // あるいは
        //    var items = _driver.FindElementsByAccessibilityId("ItemRoot");
        //    items[3].Click();   // 4行目をクリック
        // "Apple"という名前のアイテムをクリックする場合:
        //    var items = _driver.FindElementsByAccessibilityId("ItemRoot");
        //    var target = items.First(x =>
        //        x.FindElementByAccessibilityId("ItemName").Text == "Apple");
        //    target.Click();
    }
}
