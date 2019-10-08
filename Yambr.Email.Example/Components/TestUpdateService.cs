using System;
using System.Collections.Generic;
using System.Text;
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
            IMailAnalyzeService mailAnalyzeService )
        {
            _htmlConverterService = htmlConverterService;
            _mailAnalyzeService = mailAnalyzeService;
        }

        public void Init()
        {

        }

        public void InitComplete()
        {

          
            var convertHtml = _htmlConverterService.ConvertHtml(html);
            var p = _mailAnalyzeService.Persons(convertHtml);
        }
        private const string html = "Наш ДИТ комментирует, что есть проблема на стороне самого сервиса.\nУ банков-соседей тоже не работает?\n\n\nС уважением,\nЮрий Гусаров\nНачальник Департамента разработки продуктов малого и среднего бизнеса\nПАО Банк ЗЕНИТ\nМосква, Головинское ш.5 стр.1\nТел.: +7 (495) 777-57-07; 937-07-37, доб.2834\nE-mail: y.gusarov@zenit.ru\nwww.zenit.ru\n\n\n\nОт:        \"Александр Козаржевский\" <akozarzhevsky@impeltech.ru>\nКому:        \"Юрий М Гусаров\" <y.gusarov@zenit.ru>, abramyan@elewise.com\nКопия:        \"Николай Георгиевич Ямброськин\" <yambroskin@impeltech.ru>\nДата:        24.05.2018 23:14\nТема:        Fwd: Доступ до клирспендинг\n\n\n\nДля информации.\n \nт.е. проблема с доступом не в системе \"Платформ БГ\"\n \n-------- Пересылаемое сообщение--------\n24.05.2018, 17:05, \"yambroskin@impeltech.ru\" <yambroskin@impeltech.ru>:\n \nАлексей Харламов, временно настраивает доступ через наш прокси до клирспендинга\n \n \n \n-- \nС уважением,\nНиколай Ямброськин\n+7 (977) 336-96-15\nyambroskin@impeltech.ru\n \n \n \n-------- Конец пересылаемого сообщения --------\n \n \n-- \nС уважением,\nКозаржевский Александр Сергеевич\n \nРуководитель проектов ООО \"ИмпелТех\"\nТел. 8(495) 778-0121\nМоб. +7(968) 693-63-89\n \nhttp://www.impeltech.ru";


    }
}
