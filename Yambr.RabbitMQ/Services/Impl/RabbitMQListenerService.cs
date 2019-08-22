﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Yambr.Email.SDK.ComponentModel;
using Yambr.RabbitMQ.Exceptions;
using Yambr.RabbitMQ.ExtensionPoints;

namespace Yambr.RabbitMQ.Services.Impl
{

    /// <summary>
    /// Реализация подписки на Rabbit
    /// </summary>
    [Service]
    // ReSharper disable once InconsistentNaming
    public class RabbitMQService : IRabbitMQService
    {
        private const string RabbitMQServiceRegion = nameof(RabbitMQService);
        private readonly ConcurrentDictionary<string, IEnumerable<IRabbitMessageHandler>> _map;
        private readonly IEnumerable<IRabbitMessageHandler> _handlers;
        private readonly IEnumerable<IRabbitDeclareHandler> _rabbitDeclareHandlers;
        private readonly RabbitMQSettings _rabbitMQSettings;

        private readonly ILogger _logger;

        public RabbitMQService(
            IEnumerable<IRabbitMessageHandler> handlers,
            IEnumerable<IRabbitDeclareHandler> rabbitDeclareHandlers,
            RabbitMQSettings rabbitMQSettings,
            ILoggerFactory loggerFactory)
        {
            _handlers = handlers;
            _rabbitDeclareHandlers = rabbitDeclareHandlers;
            _rabbitMQSettings = rabbitMQSettings;
            _map = new ConcurrentDictionary<string, IEnumerable<IRabbitMessageHandler>>();
            _logger = loggerFactory.CreateLogger(RabbitMQServiceRegion);
            Connections = new List<IModel>();
        }

        public ICollection<IModel> Connections { get; }

        private void DisposeConnection(IModel connection)
        {
            if (connection == null) return;
            if (connection.IsOpen)
            {
                connection.Close();
            }

            connection.Dispose();
            _logger.LogDebug($"Connection #{connection.ChannelNumber} closed.");
        }

        public void DisposeConnection()
        {
            foreach (var connection in Connections)
            {
                DisposeConnection(connection);
            }
        }

        public string GetModelFromMessage(BasicDeliverEventArgs eventArgs, string message)
        {
            if (eventArgs == null) throw new ArgumentNullException(nameof(eventArgs));
            var settings = _rabbitMQSettings;

            if (!eventArgs.BasicProperties.Headers.ContainsKey(settings.ModelHeaderKey))
                throw new ArgumentNullException(settings.ModelHeaderKey, $"Не содержится заголовок {settings.ModelHeaderKey} сообщения в сообщении");

            var header = eventArgs.BasicProperties.Headers[settings.ModelHeaderKey] as byte[];
            if (header == null)
                throw new ArgumentNullException(settings.ModelHeaderKey, $"Не указан заголовок {settings.ModelHeaderKey} сообщения в сообщении");
            return Encoding.UTF8.GetString(header);
        }

        public void Init()
        {

            try
            {
                DisposeConnection();
                if (_rabbitMQSettings.HostName == null) return;
                var connectionFactory = new ConnectionFactory
                {
                    HostName = _rabbitMQSettings.HostName,
                    VirtualHost = _rabbitMQSettings.VirtualHost,
                    Protocol = Protocols.AMQP_0_9_1,
                    Port = _rabbitMQSettings.Port,
                    UserName = _rabbitMQSettings.UserName,
                    Password = _rabbitMQSettings.Password,
                    AutomaticRecoveryEnabled = true,
                    TopologyRecoveryEnabled = false
                };

                var connection = connectionFactory.CreateConnection();
                var model = connection.CreateModel();
                //Читаем по одному сообщению по умолчанию
                model.BasicQos(0, 1, true);

                foreach (var rabbitDeclareHandler in _rabbitDeclareHandlers)
                {
                    rabbitDeclareHandler.OnConnect(model);
                }



                foreach (var queueName in _rabbitDeclareHandlers.SelectMany(c => c.ConsumeQueues()))
                {
                    var consumer = new EventingBasicConsumer(model);
                    consumer.Received += (sender, args) => { OnReceived(sender, args, model); };

                    model.BasicConsume(
                        queueName,
                        false,
                        consumer);

                    _logger.LogDebug($"Waiting for messages {queueName}.");
                }



                Connections.Add(model);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Не удалось подключиться.", ex);
                throw;
            }
        }




        #region Обработка входящего сообщения

        /// <summary>
        /// При получении сообщения из очереди
        /// </summary>
        /// <param name="model"></param>
        /// <param name="eventArgs"></param>
        /// <param name="connection"></param>
        private void OnReceived(object model, BasicDeliverEventArgs eventArgs, IModel connection)
        {
            try
            {
                var body = eventArgs.Body;
                var message = Encoding.UTF8.GetString(body);
                _logger.LogDebug($"{eventArgs.Exchange}: { eventArgs.RoutingKey}: {model}: {message}");


                PrepareMessage(message, eventArgs, connection);

            }
            catch (Exception ex)
            {
                _logger.LogError("OnReceived error", ex);
            }
        }

        /// <summary>
        /// Обработать сообщение
        /// </summary>
        /// <param name="messageJson"></param>
        /// <param name="eventArgs"></param>
        /// <param name="connection"></param>
        private void PrepareMessage(string messageJson, BasicDeliverEventArgs eventArgs, IModel connection)
        {
            try
            {
                var model = GetModelFromMessage(eventArgs, messageJson);
                var rMessageHandlers = GetRabbitMessageHandlers(model);

                foreach (var handler in rMessageHandlers)
                {
                    handler.Execute(messageJson, model);
                }
                connection.BasicAck(eventArgs.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                _logger.LogError("PrepareMessage error", ex);
                connection.BasicAck(eventArgs.DeliveryTag, false);
            }
        }

        private IEnumerable<IRabbitMessageHandler> GetRabbitMessageHandlers(string model)
        {
            if (_map.ContainsKey(model))
            {
                return _map[model];
            }

            var rMessageHandlers =
                _handlers
                    .Where(a => a.CheckModel(model)).ToList();

            if (!rMessageHandlers.Any())
                throw new RabbitException($"Не существует обработчика для {model}");

            _map.TryAdd(model, rMessageHandlers);
            return rMessageHandlers;
        }



        #endregion



    }
}
