﻿using Confluent.Kafka;
using Newtonsoft.Json;

namespace Product.API
{
    public class ConsumerService
    {
        private readonly ILogger<ConsumerService> _logger;
        private readonly IApplicationBuilder _builder;
        private IConsumer<string, string> _consumer;

        public ConsumerService(IConfiguration configuration, ILogger<ConsumerService> logger, WebApplication app)
        {
            _logger = logger;
            _builder = app;
            var consumerConfig = new ConsumerConfig
            {
                BootstrapServers = configuration["Kafka:BootstrapServers"],
                GroupId = "InventoryConsumerGroup",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            _consumer = new ConsumerBuilder<string, string>(consumerConfig).Build();
        }

        public async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _consumer.Subscribe("ItemOrdered");

            while (!stoppingToken.IsCancellationRequested)
            {
                ProcessKafkaMessage(stoppingToken);

                Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }

            _consumer.Close();
        }


        public async void ProcessKafkaMessage(CancellationToken stoppingToken)
        {
            try
            {
                var consumeResult = _consumer.Consume(stoppingToken);
               
                var productId = Convert.ToInt64(consumeResult.Message.Key);

                UpdateAvailableQuantityDTO updateAvailableQuantity = JsonConvert.DeserializeObject<UpdateAvailableQuantityDTO>(consumeResult.Message.Value)!;

             var _productService =   _builder.ApplicationServices.CreateScope().ServiceProvider.GetService<IProductService>();

               await _productService.UpdateAvailableQuantity(productId,updateAvailableQuantity);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error processing Kafka message: {ex.Message}");
            }
        }
    }
}
