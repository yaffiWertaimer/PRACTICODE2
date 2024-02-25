using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace practicode2
{
    public class HtmlElement
    {

        public string Id { get; set; }
        public string Name { get; set; }
        public List<string> Attributes { get; set; }
        public List<string> Classes { get; set; }
        public string InnerHtml { get; set; }
        public HtmlElement Parent { get; set; }
        public List<HtmlElement> Children { get; set; }

        public HtmlElement()
        {
            Attributes = new List<string>();
            Classes = new List<string>();
            Children = new List<HtmlElement>();
        }

        public HtmlElement(string id, string name, List<string> attributes, List<string> classes, string innerHtml)
        {
            Id = id;
            Name = name;
            Attributes = attributes;
            Classes = classes;
            InnerHtml = innerHtml;
            Children = new List<HtmlElement>();
        }

        public IEnumerable<HtmlElement> Descendants()
        {
            var queue = new Queue<HtmlElement>();
            queue.Enqueue(this);

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                yield return current;

                foreach (var child in current.Children)
                {
                    queue.Enqueue(child);
                }
            }
        }

        public IEnumerable<HtmlElement> Ancestors()
        {
            var current = this.Parent;
            while (current != null)
            {
                yield return current;
                current = current.Parent;
            }
        }

        private static bool MatchesSelector(HtmlElement element, Selector selector)
        {
            if (selector.TagName != null && element.Name != selector.TagName)
                return false;

            if (selector.Id != null && element.Id != selector.Id)
                return false;

            if (selector.Classes != null && !selector.Classes.TrueForAll(cls => element.Classes.Contains(cls)))
                return false;

            return true;
        }

        public static List<HtmlElement> Search(HtmlElement root, string selector)
        {
            List<HtmlElement> result = new List<HtmlElement>();

            var parts = selector.Split(' ');

            foreach (var part in parts)
            {
                if (part.StartsWith("#"))
                {
                    var id = part.Substring(1);
                    result = result.Where(e => e.Id == id).SelectMany(e => e.Children).ToList();
                }
                else if (part.StartsWith("."))
                {
                    var className = part.Substring(1);
                    result = result.Where(e => e.Classes.Contains(className)).SelectMany(e => e.Children).ToList();
                }
                else
                {
                    result = result.Where(e => e.Name == part).SelectMany(e => e.Children).ToList();
                }
            }

            return result;
        }

        public static HtmlElement BuildHtmlTree(List<string> tags, List<string> voidTags)
        {
            var root = new HtmlElement();
            var currentElement = root;

            foreach (var tag in tags)
            {
                var trimmedTag = tag.Trim();
                if (trimmedTag.StartsWith("/"))
                {
                    currentElement = currentElement.Parent;
                }
                else if (trimmedTag.StartsWith("<"))
                {
                    var tagName = Regex.Match(trimmedTag, "<(\\w+)").Groups[1].Value;
                    var attributes = Regex.Matches(trimmedTag, "\\s(\\w+)=\\\"(.*?)\\\"");
                    var classes = new List<string>();

                    foreach (Match attribute in attributes)
                    {
                        if (attribute.Groups[1].Value.ToLower() == "class")
                        {
                            classes.AddRange(attribute.Groups[2].Value.Split(' '));
                        }
                    }

                    var newElement = new HtmlElement
                    {
                        Name = tagName,
                        Classes = classes,
                        Parent = currentElement
                    };

                    foreach (var @class in classes)
                    {
                        newElement.Classes.Add(@class);
                    }

                    currentElement.Children.Add(newElement);

                    if (!voidTags.Contains(tagName.ToLower()))
                    {
                        currentElement = newElement;
                    }
                }
                else
                {
                    currentElement.InnerHtml = trimmedTag;
                }
            }

            return root.Children[0];
        }
       
    }
}
