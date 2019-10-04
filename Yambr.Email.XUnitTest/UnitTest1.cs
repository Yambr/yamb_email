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
            // ���������������� ��� ������������ �����������
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

        private const string html = "��� ��� ������������, ��� ���� �������� �� ������� ������ �������.\n� ������-������� ���� �� ��������?\n\n\n� ���������,\n���� �������\n��������� ������������ ���������� ��������� ������ � �������� �������\n��� ���� �����\n������, ����������� �.5 ���.1\n���.: +7 (495) 777-57-07; 937-07-37, ���.2834\nE-mail: y.gusarov@zenit.ru\nwww.zenit.ru\n\n\n\n��:        \"��������� ������������\" <akozarzhevsky@impeltech.ru>\n����:        \"���� � �������\" <y.gusarov@zenit.ru>, abramyan@elewise.com\n�����:        \"������� ���������� ����������\" <yambroskin@impeltech.ru>\n����:        24.05.2018 23:14\n����:        Fwd: ������ �� ������������\n\n\n\n��� ����������.\n \n�.�. �������� � �������� �� � ������� \"�������� ��\"\n \n-------- ������������ ���������--------\n24.05.2018, 17:05, \"yambroskin@impeltech.ru\" <yambroskin@impeltech.ru>:\n \n������� ��������, �������� ����������� ������ ����� ��� ������ �� �������������\n \n \n \n-- \n� ���������,\n������� ����������\n+7 (977) 336-96-15\nyambroskin@impeltech.ru\n \n \n \n-------- ����� ������������� ��������� --------\n \n \n-- \n� ���������,\n������������ ��������� ���������\n \n������������ �������� ��� \"��������\"\n���. 8(495) 778-0121\n���. +7(968) 693-63-89\n \nhttp://www.impeltech.ru";

        public AutofacServiceProvider ServiceProvider { get; }



    }
}