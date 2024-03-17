using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Order.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Application.Consumers
{
    public class ProductExistsConsumerService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<ProductExistsConsumerService> _logger;

        public ProductExistsConsumerService(IConfiguration configuration, ILogger<ProductExistsConsumerService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public bool Execute(CancellationToken stoppingToken, string requestId, string productId)
        {
            var consumerConfig = new ConsumerConfig
            {
                BootstrapServers = _configuration["Kafka:BootstrapServers"],
                GroupId = "InventoryConsumerGroup",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            var consumer = new ConsumerBuilder<string, string>(consumerConfig).Build();


            consumer.Subscribe("ItemExists");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var consumeResult = consumer.Consume(stoppingToken);


                    var responseRequestId = consumeResult.Message.Key;

                    if (requestId == responseRequestId)
                    {
                        var responseValue = JsonConvert.DeserializeObject<ItemExistsResponseDto>(consumeResult.Message.Value);
                        consumer.Close();
                        return responseValue!.IsValid;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error processing Kafka message: {ex.Message}");
                }

                Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
            }

            consumer.Close();

            return false;
        }
    }
}

public class ItemExistsResponseDto
{
    public string ProductId { get; set; }
    public bool IsValid { get; set; }
}
