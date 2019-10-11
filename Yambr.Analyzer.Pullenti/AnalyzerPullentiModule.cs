using Autofac;
using EP.Morph;
using EP.Ner;
using Yambr.SDK.Autofac;

namespace Yambr.Analyzer.Pullenti
{
    public class AnalyzerPullentiModule : AbstractModule
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            ProcessorService.Initialize(MorphLang.RU | MorphLang.EN);
            // инициализируются все используемые анализаторы
       
            EP.Ner.Uri.UriAnalyzer.Initialize();
            EP.Ner.Phone.PhoneAnalyzer.Initialize();
            EP.Ner.Bank.BankAnalyzer.Initialize();
            EP.Ner.Geo.GeoAnalyzer.Initialize();
            EP.Ner.Address.AddressAnalyzer.Initialize();
            EP.Ner.Org.OrganizationAnalyzer.Initialize();
            EP.Ner.Person.PersonAnalyzer.Initialize();
            EP.Ner.Mail.MailAnalyzer.Initialize();
            EP.Ner.Named.NamedEntityAnalyzer.Initialize();
        }
    }
}
