using System.IO;
using HtmlAgilityPack;

namespace Yambr.Email.Loader.Services
{
    public interface IHtmlConverterService
    {
        string Convert(string path);
        string ConvertHtml(string html);
        void ConvertTo(HtmlNode node, TextWriter outText);
    }
}