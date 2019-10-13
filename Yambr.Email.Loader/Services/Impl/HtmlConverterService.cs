using System;
using System.IO;
using System.Linq;
using HtmlAgilityPack;
using Yambr.Email.Loader.Extensions;
using Yambr.SDK.ComponentModel;

namespace Yambr.Email.Loader.Services.Impl
{
    [Service]
    public class HtmlConverterService : IHtmlConverterService
    {
        #region Public Methods

        public string Convert(string path)
        {
            return FormatLineBreaks(File.ReadAllText(path));
        }

        public string ConvertHtml(string html)
        {
            return FormatLineBreaks(html);
        }

        public static string FormatLineBreaks(string html)
        {
            //first - remove all the existing '\n' from HTML
            //they mean nothing in HTML, but break our logic
            html = html
                .RemoveWhitespace()
                .Replace("\r", "")
                .Replace("\t", "")
                .Replace("\n", " ");
       

            //now create an Html Agile Doc object
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

           
            //remove comments, head, style and script tags
            var docDocumentNode = doc.DocumentNode;
            foreach (HtmlNode node in docDocumentNode.SafeSelectNodes("//comment() | //script | //style | //head"))
            {
                node.ParentNode.RemoveChild(node);
            }

            // ignore meanlesslinks
            foreach (HtmlNode node in docDocumentNode.SafeSelectNodes("//a"))
            {
                //TODO вынести в нормальные настройки
                var link = node.Attributes["href"]?.Value ?? string.Empty;
                if (link.StartsWith("https://www.avast.com/sig-email"))
                {
                    node.ParentNode.ParentNode.RemoveChild(node.ParentNode);
                }
            }
            // span without text = to remove
            foreach (HtmlNode node in docDocumentNode.SafeSelectNodes("//*")) //add "b", "i" if required
            {
                node.Attributes.RemoveAll();
            }

            //block-elements - convert to line-breaks
            foreach (HtmlNode node in docDocumentNode.SafeSelectNodes("//p")) //you could add more tags here
            {
                //"surround" the node with line breaks
                node.ParentNode.InsertBefore(doc.CreateTextNode("\r\n"), node);
                node.ParentNode.InsertAfter(doc.CreateTextNode("\r\n"), node);
            }
            //block-elements - convert to line-breaks
            foreach (HtmlNode node in docDocumentNode.SafeSelectNodes("//div")) //you could add more tags here
            {
               
                //div without text is a \n
                if (node.InnerHtml!= string.Empty && string.IsNullOrWhiteSpace(node.InnerHtml) || node.ChildNodes.All(c=>c.Name== "#text"))
                {
                    //"surround" the node with line breaks
                    node.ParentNode.InsertBefore(doc.CreateTextNode("\r\n"), node);
                }
            }
            //block-elements - convert to line-breaks
            foreach (HtmlNode node in docDocumentNode.SafeSelectNodes("//tr")) //you could add more tags here
            {
                //we add a "\n" ONLY if the node contains some plain text as "direct" child
                //meaning - text is not nested inside children, but only one-level deep

                //use XPath to find direct "text" in element
                var txtNode = node.SelectSingleNode("text()");

                //no "direct" text - NOT ADDDING the \n !!!!
                if (txtNode == null || txtNode.InnerHtml == "") continue;

                //"surround" the node with line breaks
                node.ParentNode.InsertBefore(doc.CreateTextNode("\r\n"), node);
            }
            //block-elements - convert to line-breaks
            foreach (HtmlNode node in docDocumentNode.SafeSelectNodes("//blockquote")) //you could add more tags here
            {
                if (node.ParentNode.Name != "blockquote")
                {
                    //"surround" the node with line breaks
                    node.ParentNode.InsertBefore(doc.CreateTextNode("\r\n"), node);
                    node.ParentNode.InsertAfter(doc.CreateTextNode("\r\n"), node);
                }
            }

            //todo: might need to replace multiple "\n\n" into one here, I'm still testing...
           
            //now remove all "meaningless" inline elements like "span"
            foreach (HtmlNode node in docDocumentNode.SafeSelectNodes("//span | //label | //a")) //add "b", "i" if required
            {
                node.InnerHtml = $" {node.InnerHtml} ";
            }

           

            //now BR tags - simply replace with "\n" and forget
            foreach (HtmlNode node in docDocumentNode.SafeSelectNodes("//br"))
                node.ParentNode.ReplaceChild(doc.CreateTextNode("\r\n"), node);

            foreach (HtmlNode node in docDocumentNode.SafeSelectNodes("//hr"))
                node.ParentNode.ReplaceChild(doc.CreateTextNode("------------------------"), node);

            //finally - return the text which will have our inserted line-breaks in it
            return   HtmlEntity.DeEntitize(docDocumentNode.InnerText.Trim());

            //todo - you should probably add "&code;" processing, to decode all the &nbsp; and such
        }


     

        #endregion

    }
}
