using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;

namespace SeleniumSample
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var webdriver = new ChromeDriver())
            {
                var fluentWait = new DefaultWait<IWebDriver>(webdriver);
                fluentWait.Timeout = TimeSpan.FromSeconds(10);
                fluentWait.PollingInterval = TimeSpan.FromMilliseconds(500);
                fluentWait.IgnoreExceptionTypes(typeof(NoSuchElementException));

                webdriver.Navigate().GoToUrl("https://disclosure.edinet-fsa.go.jp/E01EW/BLMainController.jsp?uji.bean=ee.bean.parent.EECommonSearchBean&uji.verb=W0EZA230CXP001007BLogic&TID=W1E63010&PID=W0EZ0001&SESSIONKEY=&lgKbn=2&dflg=0&iflg=0");

                var edinetInputElement = fluentWait.Until(x => x.FindElement(By.Id("mul_t")));

                edinetInputElement.SendKeys("SONY");

                var edinetButtonElement = fluentWait.Until(x => x.FindElement(By.Id("sch")));

                edinetButtonElement.Click();

                var html = webdriver.PageSource;

                var document = new HtmlDocument();
                document.LoadHtml(html);

                var hasTodayDocument = false;
                foreach (var node in document.DocumentNode.ChildNodes) {

                    if (node.InnerText.Contains("H31.02.13")) {
                        hasTodayDocument = true;
                        break;
                    }
                }

                Console.WriteLine(hasTodayDocument ? "Exist!" : "Nothing!");

                if (hasTodayDocument) {
                    webdriver.Navigate().GoToUrl("https://www.google.com/?hl=ja");

                    var googleInputElement = fluentWait.Until(x => x.FindElement(By.Name("q")));

                    googleInputElement.SendKeys("SONY トップニュース");
                    googleInputElement.SendKeys(Keys.Enter);
                }
                

                Console.ReadKey(intercept: true);
            }
        }
    }
}
