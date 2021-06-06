using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using SeleniumWebFinder.Commands;
using SeleniumWebFinder.Commands.AsyncCommand;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumWebFinder.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        IWebDriver Browser;
        ChromeDriverService chromeDriverService;
        public string DepartmentQuery { get; set; }
        public string LanguageQuery { get; set; }
        public int ExpectedVacancies { get; set; }
        public int VacanciesCount { get; set; }
        public string VacanciesExpectationText { get; set; }

        public MainWindowViewModel()
        {
            var pathToChromeDriver = Directory.GetCurrentDirectory() + "\\chromedrive";
            chromeDriverService = ChromeDriverService.CreateDefaultService(pathToChromeDriver);
        }       

        private void ClickOnElementInDropDown(string DropDownName, string ElementName)
        {
            var AllDropDowns = Browser.FindElements(By.CssSelector(".dropdown-toggle.btn.btn-primary")).ToList();
            var RequiredDropDown = AllDropDowns.First((el) => el.Text == DropDownName);
            RequiredDropDown.Click();

            string ClassName = "";

            switch (DropDownName)
            {
                case "Все отделы":
                    ClassName = "dropdown-item";
                    break;
                case "Все языки":
                    ClassName = "custom-control-label";
                    break;                
            }

            var AllLanguages = Browser.FindElements(By.ClassName(ClassName)).ToList();
            var RequiredElementName = AllLanguages.First((el) => el.Text == ElementName);
            RequiredElementName.Click();


        }

        private void UpdateVacanciesExpectationText()
        {
            if(VacanciesCount > ExpectedVacancies || VacanciesCount < ExpectedVacancies)
            {
                VacanciesExpectationText = "Ты не очень хорош в предсказаниях";
            }
            else
            {
                VacanciesExpectationText = "Ты хорош в предсказаниях";
            }

        }

        private AsyncCommand _startSearchCommandAsync = null;
        public AsyncCommand StartSearchCommandAsync => _startSearchCommandAsync ??
            (_startSearchCommandAsync = new AsyncCommand(_startSearchCommandMethodAsync));

        private async Task _startSearchCommandMethodAsync()
        {
            await Task.Run(() =>
            {                
                chromeDriverService.HideCommandPromptWindow = true;
                Browser = new ChromeDriver(chromeDriverService);
                Browser.Manage().Window.Maximize();

                Browser.Navigate().GoToUrl("https://careers.veeam.ru/vacancies");

                ClickOnElementInDropDown("Все отделы", DepartmentQuery);
                ClickOnElementInDropDown("Все языки", LanguageQuery);

                VacanciesCount = Browser.FindElements(By.CssSelector(".card.card-no-hover.card-sm")).ToList().Count;
                UpdateVacanciesExpectationText();
            }).ConfigureAwait(false);
        }

        private AsyncCommand _goDefaultCommandAsync = null;
        public AsyncCommand GoDefaultCommandAsync => _goDefaultCommandAsync ??
            (_goDefaultCommandAsync = new AsyncCommand(_goDefaultCommandMethodAsync));

        private async Task _goDefaultCommandMethodAsync()
        {
            DepartmentQuery = "Разработка продуктов";
            LanguageQuery = "Английский";
            await _startSearchCommandMethodAsync();
        }
    }
}
