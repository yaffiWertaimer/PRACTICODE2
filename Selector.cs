using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace practicode2
{
    public class Selector
    {
        public string TagName { get; set; }
        public string Id { get; set; }
        public List<string> Classes { get; set; }
        public Selector Parent { get; set; }
        public Selector Child { get; set; }

        public static Selector Parse(string selectorString)
        {
            var parts = selectorString.Split(' ');
            Selector currentSelector = null;
            Selector rootSelector = null;

            foreach (var part in parts)
            {
                var newSelector = new Selector();
                var trimmedPart = part.Trim();

                if (trimmedPart.StartsWith("#"))
                {
                    newSelector.Id = trimmedPart.Substring(1);
                }
                else if (trimmedPart.StartsWith("."))
                {
                    if (newSelector.Classes == null)
                        newSelector.Classes = new List<string>();

                    newSelector.Classes.Add(trimmedPart.Substring(1));
                }
                else
                {
                    // Validate if it's a valid HTML tag
                    if (IsValidTagName(trimmedPart))
                    {
                        newSelector.TagName = trimmedPart;
                    }
                }

                if (currentSelector != null)
                {
                    currentSelector.Child = newSelector;
                }
                else
                {
                    rootSelector = newSelector;
                }

                newSelector.Parent = currentSelector;
                currentSelector = newSelector;
            }

            return rootSelector;
        }

        private static bool IsValidTagName(string tagName)
        {
            // Validating if it's a valid HTML tag - add your validation logic here
            return !string.IsNullOrWhiteSpace(tagName);
        }

    }
}
