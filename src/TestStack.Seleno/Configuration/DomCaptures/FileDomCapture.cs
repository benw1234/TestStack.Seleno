﻿using System.IO;
using System.Text;
using OpenQA.Selenium;
using TestStack.Seleno.Configuration.Contracts;
using TestStack.Seleno.Extensions;

namespace TestStack.Seleno.Configuration.DomCaptures
{
    /// <summary>
    /// Captures the DOM to a file
    /// </summary>
    public class FileDomCapture : IDomCapture
    {
        private readonly string _capturePath;
        public FileDomCapture(string capturePath)
        {
            _capturePath = capturePath;
        }

        public IWebDriver Browser { get; set; }

        public void CaptureDom(string fileName = null)
        {
            if (!Directory.Exists(_capturePath))
                Directory.CreateDirectory(_capturePath);

            var windowTitle = Browser.Title;
            fileName = fileName ?? $"{windowTitle}{SystemTime.Now().ToFileTime()}.html".Replace(':', '.');

            var outputPath = Path.Combine(_capturePath, fileName);
            var pathChars = Path.GetInvalidPathChars();
            var stringBuilder = new StringBuilder(outputPath);
            var array = pathChars;

            for (int i = 0; i < array.Length; i++)
            {
                var item = array[i];
                stringBuilder.Replace(item, '.');
            }
            var screenShotPath = stringBuilder.ToString();

            using (var writer = new StreamWriter(screenShotPath))
                writer.Write(Browser.PageSource);
        }
    }
}
