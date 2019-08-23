using System;
using System.Net.Mime;
using Autofac;
using Yambr.Email.Common.Enums;
using Yambr.Email.Common.Models;
using Yambr.Email.Loader.Components;
using Yambr.Email.Loader.ExtensionPoints;
using Yambr.Email.Loader.Services;
using Yambr.Email.Loader.Services.Impl;
using Yambr.Loader.Services;
using Yambr.SDK.ExtensionPoints;

namespace Yambr.Email.Loader.Handlers
{
    /// <summary>
    /// Инициализация загрузчика
    /// </summary>
    public class LoaderInitHandler : IInitHandler
    {
        public int Order => 100;
        public void InitComplete()
        {
            var logger = MediaTypeNames.Application.Container.Resolve<ILog>();
            logger.Info($"{nameof(InitComplete)} в {nameof(LoaderInitHandler)}");

            var scheduler = MediaTypeNames.Application.Container.Resolve<IScheduler>();
            var job = new CheckMessageWorkJob();
            job.ScheduleJob(scheduler);

        }

        public void Shutdown()
        {
        }

        public void Init(ContainerBuilder builder)
        {
            //Регистрация сервисов модуля
            builder.RegisterType<MessageWorkService>()
                .As<IMessageWorkService>();

            //регистрируем сборщики почты
            builder.RegisterType<Pop3Loader>()
                .As<IPop3Loader>();
            builder.RegisterType<ImapLoader>()
                .As<IImapLoader>();

            // сервис работы с сообщениями
            builder.RegisterType<EmailMesageService>()
                .As<IEmailMessageService>();

            //сервис работы с контрагентами
            builder.RegisterType<ContractorService>()
                .As<IContractorService>();

            //сервис работы с контактами
            builder.RegisterType<ContactService>()
                .As<IContactService>();

            //сервис работы с ящиками
            builder.RegisterType<MailBoxService>()
                .As<IMailBoxService>();

            //сервис работы с публичными доменами
            builder.RegisterType<PublicDomainService>()
                .As<IPublicDomainService>();

        

            // регистрируем базовый загрузчик и его определение
            builder.Register(c =>
            {
                //в этот момент уже должен быть зарегистрирован ящик с которым будут раотать загрузчики
                var mailBox = c.Resolve<IMailBox>();
                if (mailBox == null) throw new ArgumentNullException(nameof(mailBox));
                IEmailLoader emailLoader = null;
                // в зависимости от типа подключения вернем нуджный сервис
                switch (mailBox.Server.ConnectionType)
                {
                    case ConnectionType.POP3:
                        emailLoader = c.Resolve<IPop3Loader>();
                        break;
                    case ConnectionType.IMAP:
                        emailLoader = c.Resolve<IImapLoader>();
                        break;
                }
                return emailLoader;
            }).As<IEmailLoader>();




           
        }
    }
}
