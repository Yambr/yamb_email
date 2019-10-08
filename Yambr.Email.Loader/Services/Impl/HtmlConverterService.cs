using System.IO;
using HtmlAgilityPack;
using Yambr.SDK.ComponentModel;

namespace Yambr.Email.Loader.Services.Impl
{
    [Service]
    public class HtmlConverterService : IHtmlConverterService
    {
        #region Public Methods

        public string Convert(string path)
        {
            var doc = new HtmlDocument();
            doc.Load(path);

            var sw = new StringWriter();
            ConvertTo(doc.DocumentNode, sw);
            sw.Flush();
            return sw.ToString();
        }

        public string ConvertHtml(string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var sw = new StringWriter();
            ConvertTo(doc.DocumentNode, sw);
            sw.Flush();
            return sw.ToString();
        }
        public void ConvertTo(HtmlNode node, TextWriter outText)
        {
            switch (node.NodeType)
            {
                case HtmlNodeType.Comment:
                    // don't output comments
                    break;

                case HtmlNodeType.Document:
                    ConvertContentTo(node, outText);
                    break;

                case HtmlNodeType.Text:
                    // script and style must not be output
                    var parentName = node.ParentNode.Name;
                    // get text
                    var html = ((HtmlTextNode)node).Text;
                    if ((parentName == "script") || (parentName == "style"))
                    {
                        break;
                    }

                    // is it in fact a special closing node output as text?
                    if (HtmlNode.IsOverlappedClosingElement(html))
                        break;

                    // check the text is meaningful and not a bunch of whitespaces
                    if (html.Length > 0)
                    {
                        var text = HtmlEntity.DeEntitize(html);
                        outText.Write($" {text} ");
                    }
                    break;

                case HtmlNodeType.Element:
                    if (node.Name == "p" || node.Name == "pre" || node.Name == "br" || node.Name == "div")
                    {
                        outText.Write("\n\r");
                    }
                    else
                    {
                        outText.Write(" ");
                    }
                    if (node.HasChildNodes)
                    {
                        ConvertContentTo(node, outText);
                    }
                    break;
            }
        }

        #endregion

        #region Private Methods

        private void ConvertContentTo(HtmlNode node, TextWriter outText)
        {
            foreach (var subnode in node.ChildNodes)
            {
                ConvertTo(subnode, outText);
            }
        }

        #endregion
    }
}
