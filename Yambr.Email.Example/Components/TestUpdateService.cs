using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Yambr.Analyzer.Services;
using Yambr.Email.Loader.Services;
using Yambr.SDK.ComponentModel;
using Yambr.SDK.ExtensionPoints;

namespace Yambr.Email.Example.Components
{
    [Component]
    class TestUpdateService : IInitHandler
    {
        private readonly IHtmlConverterService _htmlConverterService;
        private readonly IMailAnalyzeService _mailAnalyzeService;

        public TestUpdateService(
            IHtmlConverterService htmlConverterService,
            IMailAnalyzeService mailAnalyzeService)
        {
            _htmlConverterService = htmlConverterService;
            _mailAnalyzeService = mailAnalyzeService;
        }

        public void Init()
        {

        }

        public void InitComplete()
        {
           
            var files = Directory.EnumerateFiles("EmailMessage");
            // var files = new List<string>(){ "EmailMessage\\34a63b8890519d312f08590563d2feea.json" };
            foreach (var file in files)
            {
                var data = JsonConvert.DeserializeObject<JObject>(File.ReadAllText(file));
                var doc = data["Body"].Value<string>();
                var isHtml = data["IsBodyHtml"].Value<bool>();
                if (!string.IsNullOrWhiteSpace(doc))
                {
                    var convertHtml = isHtml ? _htmlConverterService.ConvertHtml(doc) : doc;
                    var p = _mailAnalyzeService.Persons(convertHtml);
                    if (p.Any(c => c.Emails.Any(e =>
                    {
                        var ok = (e.EndsWith(".ru") || e.EndsWith(".com") || e.EndsWith(".info") || e.EndsWith(".su") || e.EndsWith(".org") || e.EndsWith(".tech") || e.EndsWith("D297BED0"));
                        if (!ok)
                        {
                            Console.WriteLine(file);
                            Console.WriteLine(e);
                        }
                        return !ok;
                    })))
                    {
                       // throw new Exception("криво");
                    }
                    foreach (var personReferrent in p)
                    {
                        Console.WriteLine($"{personReferrent.FirstName} {personReferrent.Description}");
                    }
                }

            }

        }
        private const string html = "<html xmlns:v=\"urn:schemas-microsoft-com:vml\" xmlns:o=\"urn:schemas-microsoft-com:office:office\" xmlns:w=\"urn:schemas-microsoft-com:office:word\" xmlns:m=\"http://schemas.microsoft.com/office/2004/12/omml\" xmlns=\"http://www.w3.org/TR/REC-html40\">\n<head>\n\n\n<!--[if !mso]><style>v\\:* {behavior:url(#default#VML);}\no\\:* {behavior:url(#default#VML);}\nw\\:* {behavior:url(#default#VML);}\n.shape {behavior:url(#default#VML);}\n</style><![endif]--><style><!--\n/* Font Definitions */\n@font-face\n\t{font-family:\"Cambria Math\";\n\tpanose-1:2 4 5 3 5 4 6 3 2 4;}\n@font-face\n\t{font-family:Calibri;\n\tpanose-1:2 15 5 2 2 2 4 3 2 4;}\n/* Style Definitions */\np.MsoNormal, li.MsoNormal, div.MsoNormal\n\t{margin:0cm;\n\tmargin-bottom:.0001pt;\n\tfont-size:11.0pt;\n\tfont-family:\"Calibri\",sans-serif;\n\tmso-fareast-language:EN-US;}\na:link, span.MsoHyperlink\n\t{mso-style-priority:99;\n\tcolor:#0563C1;\n\ttext-decoration:underline;}\na:visited, span.MsoHyperlinkFollowed\n\t{mso-style-priority:99;\n\tcolor:#954F72;\n\ttext-decoration:underline;}\nspan.EmailStyle17\n\t{mso-style-type:personal-compose;\n\tfont-family:\"Calibri\",sans-serif;\n\tcolor:windowtext;}\n.MsoChpDefault\n\t{mso-style-type:export-only;\n\tfont-family:\"Calibri\",sans-serif;\n\tmso-fareast-language:EN-US;}\n@page WordSection1\n\t{size:612.0pt 792.0pt;\n\tmargin:2.0cm 42.5pt 2.0cm 3.0cm;}\ndiv.WordSection1\n\t{page:WordSection1;}\n--></style><!--[if gte mso 9]><xml>\n<o:shapedefaults v:ext=\"edit\" spidmax=\"1026\" />\n</xml><![endif]--><!--[if gte mso 9]><xml>\n<o:shapelayout v:ext=\"edit\">\n<o:idmap v:ext=\"edit\" data=\"1\" />\n</o:shapelayout></xml><![endif]-->\n</head>\n<body lang=\"RU\" link=\"#0563C1\" vlink=\"#954F72\">\n<div class=\"WordSection1\">\n<p class=\"MsoNormal\"><span style=\"mso-fareast-language:RU\"><img width=\"1920\" height=\"1080\" style=\"width:20.0in;height:11.25in\" id=\"Рисунок_x0020_2\" src=\"cid:image001.png@01D52F5A.932CB340\"></span><o:p></o:p></p>\n<p class=\"MsoNormal\"><o:p>&nbsp;</o:p></p>\n<p class=\"MsoNormal\" style=\"margin-bottom:8.0pt\"><span style=\"mso-fareast-language:RU\"><img width=\"1920\" height=\"1080\" style=\"width:20.0in;height:11.25in\" id=\"Рисунок_x0020_3\" src=\"cid:image003.png@01D52F5A.F38E5090\"></span><span style=\"font-size:10.0pt;color:#1F497D;mso-fareast-language:RU\"><o:p></o:p></span></p>\n<p class=\"MsoNormal\" style=\"margin-bottom:8.0pt\"><span style=\"font-size:10.0pt;color:#1F497D;mso-fareast-language:RU\"><o:p>&nbsp;</o:p></span></p>\n<p class=\"MsoNormal\" style=\"margin-bottom:8.0pt\"><span style=\"font-size:10.0pt;color:#1F497D;mso-fareast-language:RU\">С уважением,<br>\n<br>\n<b>Ямброськин Николай<br>\n</b>Ведущий программист<br>\nУправление проектирования и разработки<br>\nРаб. тел.: &#43;7 (495) 221-73-63, доб. 630<br>\n<a href=\"mailto:%20ngyambroskin@alfaleasing.ru\">ngyambroskin@alfaleasing.ru</a><br>\n<br>\nГК &laquo;Альфа-Лизинг&raquo;<br>\n117335, г. Москва, Нахимовский пр-кт, дом 58<br>\n<a href=\"alfaleasing.ru\">alfaleasing.ru</a><br>\n<br>\n<img border=\"0\" width=\"184\" height=\"51\" style=\"width:1.9166in;height:.5347in\" id=\"Рисунок_x0020_1\" src=\"cid:image002.png@01D52F5A.932CB340\"><o:p></o:p></span></p>\n<p class=\"MsoNormal\"><o:p>&nbsp;</o:p></p>\n</div>\n</body>\n</html>\n";

        private static bool HasRussian(string name)
        {
            return name != null &&
                   name.Any(c => (c >= 'А' && c <= 'я') || c == 'ё' || c == 'Ё');
        }
    }
}
