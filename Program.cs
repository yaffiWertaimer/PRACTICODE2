using practicode2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace YourNamespace
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // קריאה לקבצים והמרה לרשימות של תגיות ותגיות Void
            List<string> tags = new List<string> { "<html>", "<head>", "<title>", "</title>", "</head>", "<body>", "<h1>", "</h1>", "<p>", "</p>", "</body>", "</html>" };
            List<string> voidTags = new List<string> { "!DOCTYPE", "area", "base", "br", "col", "embed", "hr", "img", "input", "link", "meta", "param", "source", "track", "wbr" };

            // בניית עץ האלמנטים
            var rootElement = HtmlElement.BuildHtmlTree(tags, voidTags);

            // הדפסת עץ האלמנטים
            PrintHtmlTree(rootElement, 0);

            string url = "https://www.mehalev.co.il/";
            await RunAllFunctions(url); // או RunAllFunctions(url); אם תשנה את הסימון של Main ל async
        }

        // פונקציה להדפסת עץ האלמנטים
        static void PrintHtmlTree(HtmlElement element, int depth)
        {
            Console.WriteLine($"{new string(' ', depth * 2)}<{element.Name}>");

            if (!string.IsNullOrEmpty(element.InnerHtml))
            {
                Console.WriteLine($"{new string(' ', (depth + 1) * 2)}{element.InnerHtml}");
            }

            foreach (var child in element.Children)
            {
                PrintHtmlTree(child, depth + 1);
            }

            Console.WriteLine($"{new string(' ', depth * 2)}</{element.Name}>");
        }

        static async Task RunAllFunctions(string url)
        {
            // קריאה לתוכן HTML מה-URL
            var html = await Load(url);
            var cleanHtml = new Regex("\\s+").Replace(html, " ");
            var htmlLines = new Regex("<(.*?)>").Split(cleanHtml).Where(s => s.Length > 0).ToList();

            // בניית עץ האלמנטים
            List<string> tags = new List<string> { "<html>", "<head>", "<title>", "</title>", "</head>", "<body>", "<h1>", "</h1>", "<p>", "</p>", "</body>", "</html>" };
            List<string> voidTags = new List<string> { "!DOCTYPE", "area", "base", "br", "col", "embed", "hr", "img", "input", "link", "meta", "param", "source", "track", "wbr" };
            var rootElement = HtmlElement.BuildHtmlTree(tags, voidTags);

            // הדפסת עץ האלמנטים
            PrintHtmlTree(rootElement, 0);
        }

        static async Task<string> Load(string url)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(url);
            var html = await response.Content.ReadAsStringAsync();
            return html;
        }
    }
}
