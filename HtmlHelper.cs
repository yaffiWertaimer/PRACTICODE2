using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using System.Text.RegularExpressions;


namespace practicode2
{
    public class HtmlHelper
    {
        private readonly static HtmlHelper _instance = new HtmlHelper();
        public static HtmlHelper Instance => _instance;
        private string[] allHtmlTags { get; set; }
        private string[] selfClosingHtmlTags { get; set; }

        // בנאי סטטי - אוטומטי ישומל וייצר מופע חדש בפעם הראשונה שישומל במחלקה
        static HtmlHelper()
        {
            _instance.allHtmlTags = LoadTagsFromFile("JsonFiles/HtmlTags.json");
            _instance.selfClosingHtmlTags = LoadTagsFromFile("JsonFiles/HtmlVoidTags.json");
        }

        private HtmlHelper()
        {
            // ריק כדי לא לקרוא לבנאי המופעל ידנית
        }

        // פעולת קריאה אסינכרונית לקובץ על מנת לטעון את התגיות ממנו
        private static string[] LoadTagsFromFile(string filePath)
        {
            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<string[]>(json);
        }
       


    }
}
