﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace MySeleniumApi.Api
{
    public class SeleniumApi
    {
        // class fields
        private IWebDriver Driver;

        public SeleniumApi(IWebDriver driver)
        {
            this.Driver = driver;
        }

        /// <summary>
        /// Simple Boolean-returning function to both ensure
        /// an element exists in a test flow and catch
        /// the Selenium exception thrown in the event
        /// of that same element not existing
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Boolean ElementExistsById(string id)
        {
            try
            {
                Driver.FindElement(By.Id(id));
            }
            catch (NoSuchElementException)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Quick JavaScriptExecutor function to avoid
        /// test flow errors regarding web elements not
        /// being on screen, scrolling directly to the
        /// passed element
        /// </summary>
        /// <param name="element"></param>
        public void ScrollToElement(IWebElement element)
        {
            IJavaScriptExecutor jse = (IJavaScriptExecutor) Driver;
            jse.ExecuteScript("arguments[0].scrollIntoView()", element);
        }

        /// <summary>
        /// Fuzzing function that takes in an input file line-by-line
        /// and passes them through a specified input parameter,
        /// outputs matches based on given element to a specified
        /// output file
        /// </summary>
        /// <param name="url"></param>
        /// <param name="inFile"></param>
        /// <param name="outFile"></param>
        /// <param name="inputElement"></param>
        /// <param name="checkElement"></param>
        public void InputFuzzTest(string url, string inFile, string outFile, 
                                    string inputElement, string checkElement)
        {
            //head to URL
            Driver.Navigate().GoToUrl(url);

            // create string array from file containing all elements
            string[] fileElements = File.ReadAllLines(inFile);

            // loop through the entirety of the array
            foreach (string el in fileElements)
            {
                // need to find, clear, and click input element
                IWebElement webInputElement = Driver.FindElement(By.Id(inputElement));
                webInputElement.Clear();
                webInputElement.Click();

                // enter that element
                webInputElement.SendKeys(el);
                webInputElement.SendKeys(Keys.Enter);

                // wait for page load
                Thread.Sleep(5000);

                // check if element exists, if so, append to output file
                if (ElementExistsById(checkElement))
                {
                    StreamWriter stream = new StreamWriter(outFile);
                    stream.WriteLine(el);
                    stream.Close();
                }

                // reload the page
                Driver.Navigate().GoToUrl(url);

                // wait for page load once more
                Thread.Sleep(5000);
            }
        }

        /// <summary>
        /// Intakes a string array of (assuming) test data, uses Regex to
        /// find and replace portions of text specified and appends the new text
        /// to a copy of the initial array. Use for batch conversion of data to
        /// further de-couple tests and test data
        /// </summary>
        /// <param name="urls"></param>
        /// <param name="oldSegment"></param>
        /// <param name="newSegment"></param>
        /// <returns></returns>
        public string[] ReplaceTextPortion(string[] textData, string oldText, string newText)
        {
            // data string array copy
            string[] newTextData = new string[textData.Length];

            // loop through original string array, regex matching based on the
            // oldText argument, replacing it with the new, and adding it to
            // the string data array copy
            for (int index = 0; index < textData.Length; index++)
            {
                if (Regex.IsMatch(textData[index], oldText, RegexOptions.IgnoreCase))
                {
                    var newData = Regex.Replace(textData[index], oldText, newText);
                    newTextData[index] = newData;
                }
            }

            return newTextData;
        }

        static void Main(string[] args) { }
    }
}
