using EP.Ner;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using EP.Morph;
using EP.Ner.Address;
using EP.Ner.Bank;
using EP.Ner.Core;
using EP.Ner.Org;
using EP.Ner.Person;
using EP.Ner.Phone;
using EP.Ner.Uri;
using Xunit;
using HtmlAgilityPack;
using EP.Ner.Geo;
using EP.Ner.Named;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Yambr.Analyzer.Models;
using Yambr.Analyzer.Pullenti.Services;
using Yambr.Analyzer.Services;
using Yambr.Email.Example;
using Yambr.Email.Loader.Services;
using Yambr.Email.Loader.Services.Impl;
using Yambr.SDK.Autofac;
using Yambr.SDK.ExtensionPoints;

namespace Yambr.Email.XUnitTest
{
    public class UnitTest1
    {

        public UnitTest1()
        {
            ProcessorService.Initialize(MorphLang.RU | MorphLang.EN);
            // инициализируются все используемые анализаторы
            EP.Ner.Money.MoneyAnalyzer.Initialize();
            EP.Ner.Uri.UriAnalyzer.Initialize();
            EP.Ner.Phone.PhoneAnalyzer.Initialize();
            EP.Ner.Definition.DefinitionAnalyzer.Initialize();
            EP.Ner.Date.DateAnalyzer.Initialize();
            EP.Ner.Bank.BankAnalyzer.Initialize();
            EP.Ner.Geo.GeoAnalyzer.Initialize();
            EP.Ner.Address.AddressAnalyzer.Initialize();
            EP.Ner.Org.OrganizationAnalyzer.Initialize();
            EP.Ner.Person.PersonAnalyzer.Initialize();
            EP.Ner.Mail.MailAnalyzer.Initialize();
            EP.Ner.Transport.TransportAnalyzer.Initialize();
            EP.Ner.Decree.DecreeAnalyzer.Initialize();
            EP.Ner.Titlepage.TitlePageAnalyzer.Initialize();
            EP.Ner.Booklink.BookLinkAnalyzer.Initialize();
            EP.Ner.Named.NamedEntityAnalyzer.Initialize();
            EP.Ner.Goods.GoodsAnalyzer.Initialize();
        }


        [Fact]
        public void Test1()
        {


            var htmlConverterService = new HtmlConverterService();
            var service = new MailAnalyzeService();
            var convertHtml = htmlConverterService.ConvertHtml(html);
            var p = service.Persons(convertHtml);
        }

        private const string html = "Наш ДИТ комментирует, что есть проблема на стороне самого сервиса.\nУ банков-соседей тоже не работает?\n\n\nС уважением,\nЮрий Гусаров\nНачальник Департамента разработки продуктов малого и среднего бизнеса\nПАО Банк ЗЕНИТ\nМосква, Головинское ш.5 стр.1\nТел.: +7 (495) 777-57-07; 937-07-37, доб.2834\nE-mail: y.gusarov@zenit.ru\nwww.zenit.ru\n\n\n\nОт:        \"Александр Козаржевский\" <akozarzhevsky@impeltech.ru>\nКому:        \"Юрий М Гусаров\" <y.gusarov@zenit.ru>, abramyan@elewise.com\nКопия:        \"Николай Георгиевич Ямброськин\" <yambroskin@impeltech.ru>\nДата:        24.05.2018 23:14\nТема:        Fwd: Доступ до клирспендинг\n\n\n\nДля информации.\n \nт.е. проблема с доступом не в системе \"Платформ БГ\"\n \n-------- Пересылаемое сообщение--------\n24.05.2018, 17:05, \"yambroskin@impeltech.ru\" <yambroskin@impeltech.ru>:\n \nАлексей Харламов, временно настраивает доступ через наш прокси до клирспендинга\n \n \n \n-- \nС уважением,\nНиколай Ямброськин\n+7 (977) 336-96-15\nyambroskin@impeltech.ru\n \n \n \n-------- Конец пересылаемого сообщения --------\n \n \n-- \nС уважением,\nКозаржевский Александр Сергеевич\n \nРуководитель проектов ООО \"ИмпелТех\"\nТел. 8(495) 778-0121\nМоб. +7(968) 693-63-89\n \nhttp://www.impeltech.ru";

        public AutofacServiceProvider ServiceProvider { get; }



    }
}