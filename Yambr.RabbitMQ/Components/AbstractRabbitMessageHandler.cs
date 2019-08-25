using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Yambr.RabbitMQ.ExtensionPoints;
using Yambr.SDK.Extensions;

namespace Yambr.RabbitMQ.Components
{

    public abstract class AbstractRabbitMessageHandler<TMessage, TResult> : IRabbitMessageHandler
        where TMessage : class
    {
        private readonly ILogger _logger;

        public AbstractRabbitMessageHandler(ILogger logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Настройки сериалайзера - можно переопределить
        /// </summary>
        protected virtual JsonSerializerSettings SerializerSettings => null;

        public abstract string[] Model { get; }
        public virtual bool CheckModel(string model)
        {
            if (Model == null || Model.Length == 0) return true;
            return Model.Contains(model);
        }

        public async Task ExecuteAsync(string message, string model)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            if (model == null) throw new ArgumentNullException(nameof(model));

            var result = default(TResult);

            async Task RunAction()
            {
                var messageObject = await BeforeAsync(message, model);
                if (messageObject == null) return;

                result = await RunAsync(messageObject);
            }

            Task ErrorCallBack(Exception exception)
            {
                _logger.Error(exception, $" Не удалось обработать {typeof(TMessage).FullName} в {this.GetType()}");
                throw exception;
            }

            async Task SuccessCallback()
            {
                //TODO обдумать реализацию
                if (result != null) //TODO: V3111 https://www.viva64.com/en/w/v3111/ Checking value of 'result' for null will always return false when generic type is instantiated with a value type.
                {
                    await AfterAsync(result);
                }
            }

            try
            {
                await RunAction();
                await SuccessCallback();
            }
            catch (Exception e)
            {
                await ErrorCallBack(e);
            }

        }
        public virtual async Task<TMessage> BeforeAsync(string message, string model)
        {

            _logger.Debug("{0} message\r\n{1}", model, message);
            var deserializeObject = CheckModel(model) && !string.IsNullOrWhiteSpace(message)
                ? JsonConvert.DeserializeObject<TMessage>(message, SerializerSettings)
                : null;
            return deserializeObject;
        }

        public  abstract Task<TResult> RunAsync(TMessage message);
        public virtual Task AfterAsync(TResult result)
        {
            return Task.CompletedTask;
        }
    }
}
